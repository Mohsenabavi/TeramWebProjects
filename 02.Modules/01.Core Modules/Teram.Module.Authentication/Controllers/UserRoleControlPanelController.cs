using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Teram.Module.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Attributes;
using Teram.Framework.Core;
using Teram.Web.Core.Model;
using Teram.Web.Core.Exceptions;
using Teram.Framework.Core.Tools;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Teram.Web.Core;
using Teram.ServiceContracts;

namespace Teram.Module.Authentication.Controllers
{
    [Display(Description = "UserRoles")]
    public class UserRoleControlPanelController : BasicControlPanelController
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IUserSharedService userSharedService;
        private readonly IRoleSharedService roleSharedService;

        public UserRoleControlPanelController(IServiceProvider serviceProvider, ILogger<UserRoleControlPanelController> logger,
            IStringLocalizer<UserRoleControlPanelController> localizer, IStringLocalizer<SharedResource> sharedLocalizer,
           IUserSharedService userSharedService, IRoleSharedService roleSharedService)
        {
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;
            this.serviceProvider = serviceProvider;
            this.userSharedService = userSharedService;
            this.roleSharedService = roleSharedService;
            ViewBag.PageName = localizer["User Roles"];

            Model = new ViewInformation<Models.UserRoleModel>(true)
            {
                
                Title = localizer["User Roles"],
                HomePage = "/UserRoleControlPanel/Index",
                ExtraScripts = "/ExternalModule/Module/Authentication/Scripts/UserRoleControlPanel.js",
                HasGrid = true,
                EditInSamePage = true
            };

        }
        [Display(Description = "ShowPage")]
        [ControlPanelMenu("User Roles", ParentName = "Security", Icon = "fa-users", PanelType = PanelType.Managment, Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {

            ViewBag.Users = GetAllUsers();
            ViewBag.Roles = GetAllRoles();

            return View("Index", Model);
        }

        [HttpPost]
        [ParentalAuthorize(nameof(Index))]
        public virtual async Task<IActionResult> GetDataAsync(DatatablesSentModel model)
        {
            var sortInformation = new List<SortInformation>();
            if (model.Order != null)
            {
                sortInformation = GetSortInformation(model.Columns, model.Order);
            }

            var dataList = userSharedService.GetUsers("", model.Start, model.Length);

            var result = new List<Models.UserRoleModel>();
            foreach (var item in dataList)
            {
                var userRoleModel = await LoadData(item.UserId);
                result.Add(userRoleModel); ;
            }
            var count = userSharedService.GetUserInfo(""); //.Users.Count();  
            return Json(new { model.Draw, recordsTotal = count, recordsFiltered = count, data = result, error = "" });
        }

        [Display(Description = "NewItem")]

        public IActionResult NewItem()
        {
            Model.ModelData = new Models.UserRoleModel();
            return View("VIndex", Model);
        }

        [Display(Description = "Edit")]
        public async Task<IActionResult> EditPartialAsync(Guid id)
        {
            var allUsers = GetAllUsers();
            if (id == Guid.Empty)
            {// موقع ایجاد همه ی کاربران را لود کند
                ViewBag.Users = allUsers;
                return PartialView("Add", new Models.UserRoleModel());
            }

            // لیست همه ی نقش های موجود
            var allRoles = GetAllRoles();
            // ست کردن وضعیت انتخاب  نقش ها برای کاربر
            var model = await LoadData(id);
            foreach (var item in allRoles)
            {
                var currentUserRoles = model.RolesList.ToList();
                item.Selected = currentUserRoles.Any(x => x.RoleId.ToString().Equals(item.Value)) ? true : false;
            }

            // دیتاسورس کمبوی نقش
            ViewBag.Roles = allRoles;

            // موقع ویرایش فقط کاربر انتخاب شده را لود کند
            ViewBag.Users = allUsers.Where(x => x.Value.ToLower() == id.ToString());

            Model.ModelData = model;
            return PartialView("Add", Model.ModelData);
        }


        [ParentalAuthorize(nameof(Index))]
        /// <summary>
        /// آیدی کاربر را میگیرد و مدلی شامل اطلاعات کاربر و نقش هایش بر میگرداند
        /// </summary>
        /// <param name="UserManager"></param>
        /// <param name="id">آیدی کاربر</param>
        /// <returns></returns>
        private async Task<Models.UserRoleModel> LoadData(Guid id)
        {
            var user = await userSharedService.GetUserById(id);
            var roles = await userSharedService.GetRolesOfUsers(new List<Guid> { user.UserId });//.GetRolesOfUser(user);

            if (user == null)
            {
                throw new UIException(localizer["User not found"]);
            }
            var userRoleModel = new Models.UserRoleModel
            {
                Key = user.UserId,
                UserName = user.Username,
                Name=user.Name,
                Email = user.Email,
                Roles = String.Join(", ", roles.Select(x => x.RoleName.ToString()).ToArray()),
                RolesList = roles.ToList()
            };
            return userRoleModel;
        }
        [ParentalAuthorize(nameof(Index))]

        public async Task<IActionResult> Detail(Guid id)
        {
            await LoadData(id);
            return View("ItemDetail", Model);

        }

        [HttpDelete]
        [Display(Description = "Remove")]
        [ParentalAuthorize(nameof(Index))]
        public async Task<IActionResult> Remove(Guid id)
        {
            try
            {
                var message = localizer["Unable to delete"];
                return Json(new { result = "fail", message = message, title = localizer["Something wrong"] });

            }
            catch (Exception ex)
            {
                var message = ExceptionParser.Parse(ex);
                logger.LogError(message, ex);
                return Json(new { result = "fail", message = localizer["I couldn't remove your record"] });

            }
        }

        [HttpPost]
        [Display(Description = "Save")]

        public async Task<IActionResult> Save(Guid userId, List<Guid> roles)
        {

            IdentityResult result = null;
            // اسم رولها را از دیتابیس میگیرد
            var roleNames = roleSharedService.GetRoleByListRoleId(roles);

            if (userId != null && userId != Guid.Empty)
            {
                // خواندن یوزرار دیتابیس
                var TeramUser = await userSharedService.GetUserById(userId);

                if (TeramUser != null)
                {
                    //get all user's roles, and remove them
                    var roless = await userSharedService.GetRolesOfUser(TeramUser);
                    result = await userSharedService.RemoveUserRoles(TeramUser, roless);

                    // Assign Role to user
                    foreach (var item in roleNames)
                    {
                        var isInRole = await userSharedService.IsInRoleAsync(TeramUser, item.Name);
                        if (!isInRole)
                        {
                            result = await userSharedService.AddToRoleAsync(TeramUser, item.Name);
                        }
                    }
                }
                if (result.Succeeded)
                {
                    return Json(new { result = "ok", message = sharedLocalizer["Your data has been saved"], title = sharedLocalizer["SaveTitle"] });
                }
                else
                {
                    var message = result.Errors.Select(x => x.Description).Aggregate((x, c) => x + Environment.NewLine + c);
                    return Json(new { result = "fail", message = message, title = sharedLocalizer["Something wrong"] });
                }
            }
            return Json(new { result = "fail", message = localizer["Select User"], title = sharedLocalizer["Something wrong"] });

        }

        /// <summary>
        /// لیست کاربران
        /// </summary>
        /// <param name="userManager"></param>
        /// <returns></returns>
        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetUsers([FromServices] UserManager<TeramUser> userManager)
        {
            var users = userManager.Users.ToList(); //.GetUsersInRoleAsync("Manager");
            //users.Wait();
            return Json(new { results = users.Select(x => new { id = x.Id, text = x.UserName }) });
        }

        /// <summary>
        /// لیست نقش های کاربر
        /// وقتی کمبوی کاربر عوض میشود این اکشن صدا میشود و تا نقش های کاربر در کمبوی نقش  را نمایش دهد
        /// </summary> 
        /// <param name="userId"></param>
        /// <returns></returns>
        [ParentalAuthorize(nameof(Index))]
        public async Task<IActionResult> GetUserRolesAsync(Guid userId)
        {
            // همه ی نقش ها
            var allRoles = GetAllRoles();

            // وضعیت انتخاب شدن نقش ها
            var model = await LoadData(userId);
            foreach (var item in allRoles)
            {
                var currentUserRoles = model.RolesList.ToList();
                item.Selected = currentUserRoles.Any(x => x.RoleId.ToString() == item.Value) ? true : false;
            }

            return Json(new { results = allRoles });
        }

        private List<SelectListItem> GetAllRoles()
        {
            return roleSharedService.GetAllRoles().Select(item =>
                                   new SelectListItem
                                   {
                                       Value = item.Id.ToString(),
                                       Text = item.Name + (!string.IsNullOrEmpty(item.Title) ? "(" + item.Title + ")" : ""),
                                   }).ToList();
        }
        private List<SelectListItem> GetAllUsers()
        {
            return userSharedService.GetUserInfo("").Select(item =>
                                   new SelectListItem
                                   {
                                       Value = item.UserId.ToString(),
                                       Text = item.Username + (!string.IsNullOrEmpty(item.Name) ? " - " + item.Name : "")
                                   }).ToList();
        }

    }


}