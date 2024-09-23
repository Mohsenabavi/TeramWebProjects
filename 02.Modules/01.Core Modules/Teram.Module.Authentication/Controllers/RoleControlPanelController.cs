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
using System.ComponentModel.DataAnnotations;
using Teram.Web.Core;
using Teram.ServiceContracts;

namespace Teram.Module.Authentication.Controllers
{
    [Display(Description = "Roles")]
    public class RoleControlPanelController : BasicControlPanelController
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IRoleSharedService roleSharedService;

        public RoleControlPanelController(IServiceProvider serviceProvider, ILogger<RoleControlPanelController> logger,
            IStringLocalizer<RoleControlPanelController> localizer, IStringLocalizer<SharedResource> sharedlocalizer
            , IRoleSharedService roleSharedService)
        {
            this.logger = logger;
            this.sharedLocalizer = sharedlocalizer;
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            this.serviceProvider = serviceProvider;
            this.roleSharedService = roleSharedService;
            ViewBag.PageName = localizer["Roles"];
            Model = new ViewInformation<RoleModel>(true)
            {
                Title = localizer["Roles"],
                HomePage = "/RoleControlPanel/Index",
                ExtraScripts = "/ExternalModule/Module/Authentication/Scripts/RoleControlPanel.js",
                HasGrid = true,
                EditInSamePage = true,  
            };
        }

        [Display(Description = "ShowPage")]
        [ControlPanelMenu("Roles", ParentName = "Security", Icon = "fa-lock", PanelType = PanelType.Managment, Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {            

            return View("Index", Model);
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetData(DatatablesSentModel model)
        {
            var sortInformation = new List<SortInformation>();
            if (model.Order != null)
            {
                sortInformation = GetSortInformation(model.Columns, model.Order);
            }
            var data = roleSharedService.GetAllRoles().Select(x => new RoleModel
            {
                Key = x.Id,
                Name = x.Name,
                IsDefaultRole = x.IsDefaultRole,
                Title = x.Title

            }).ToList();

            if (data == null) return null;
            return Json(new { model.Draw, recordsTotal = data.Count(), recordsFiltered = data.Count(), data, error = "" });
        }

        [Display(Description = "NewItem")]
        public IActionResult NewItem()
        {
            Model.ModelData = new RoleModel();
            return View("NewItem", Model);
        }


        [Display(Description = "Edit")]
        public async Task<IActionResult> EditPartialAsync(Guid id)
        {

            if (id == Guid.Empty)
                return PartialView("Add", new RoleModel());


            var roleInfo = await LoadData(id);
            var roleModel = new RoleModel
            {
                Key = roleInfo.Id,
                Name = roleInfo.Name,
                Title = roleInfo.Title,
                IsDefaultRole = roleInfo.IsDefaultRole
            };
            Model.ModelData = roleModel;
            return PartialView("Add", Model.ModelData);
        }

        [ParentalAuthorize(nameof(Index))]
        private async Task<RoleInfo> LoadData(Guid id)
        {
            var roleInfo = await roleSharedService.GetRoleById(id);//.FindByIdAsync(id.ToString());

            if (roleInfo == null)
                throw new UIException(localizer["Role not found"]);

            Model.ModelData = roleInfo;
            Model.Key = id;
            return roleInfo;
        }

        [ParentalAuthorize(nameof(Index))]
        public async Task<IActionResult> Detail(Guid id)
        {
            if (id == null || id == Guid.Empty)
                return Json(new { result = "fail", message = localizer["Key not found"] });


            await LoadData(id);
            return View("ItemDetail", Model);

        }


        [HttpDelete]
        [Display(Description = "Remove")]
        public async Task<IActionResult> Remove(Guid key)
        {
            try
            {
                if (key == Guid.Empty)
                {
                    return Json(new { result = "fail", message = localizer["Key not found"] });
                }

                var role = await LoadData(key);
                // اجازه حذف نقش پیشفرض وجود ندارد             
                if (role.IsDefaultRole)
                {
                    return Json(new { result = "fail", message = localizer["It is not possible to delete the default role"], title = sharedLocalizer["Something wrong"] });
                }

                await roleSharedService.DeleteAsync(role);

                return Json(new
                {
                    result = "ok",
                    message = sharedLocalizer["Your record removed successfully."]
                });

            }
            catch (Exception ex)
            {
                var message = ExceptionParser.Parse(ex);
                logger.LogError(message, ex);
                return Json(new { result = "fail", message = sharedLocalizer["I couldn't remove your record"] });

            }
        }

        [HttpPost]
        [Display(Description = "Save")]
        public async Task<IActionResult> Save(RoleModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Name))
                {
                    return Json(new { result = "fail", message = "نام لاتین نقش الزامی است", title = sharedLocalizer["Something wrong"] });
                }
                IdentityResult result;
                if (model.Key == Guid.Empty)
                {
                    //اگر هیچ نقشی پیشفرض انتخاب نشده بود این نقش را به عنوان پیشفرض بگذارد
                    var isExistsDefaultRole = roleSharedService.IsExistsDefaultRole();
                    if (!isExistsDefaultRole)
                    {
                        model.IsDefaultRole = true;
                    }
                    var roleInfo = new RoleInfo
                    {
                        Id = model.Key,
                        Name = model.Name,
                        Title = model.Title,
                        IsDefaultRole = model.IsDefaultRole
                    };
                    result = await roleSharedService.CreateAsync(roleInfo);
                }
                else
                {
                    var roleInfo = await LoadData(model.Key);

                    roleInfo.Name = model.Name;
                    roleInfo.Title = model.Title;
                    roleInfo.IsDefaultRole = model.IsDefaultRole;

                    // اگر تیک پیشفرض را برداشت چک میکند اگر  هیچ پیشفرضی وجود ندارد اجازه ذخیره ندهد
                    var defaultRole = roleSharedService.GetAllRoles().Any(x => x.IsDefaultRole && x.Id != roleInfo.Id);
                    if (!model.IsDefaultRole && !defaultRole)
                    {
                        return Json(new { result = "fail", message = localizer["There is no default role, select default"], title = sharedLocalizer["Something wrong"] });
                    }

                    result = await roleSharedService.UpdateAsync(roleInfo);

                    // اگر تیک پیشفرض را زد این نقش پیشفرض شود  و بقیه از حالت پیشفرض درآیند
                    if (model.IsDefaultRole)
                    {
                        var roles = roleSharedService.GetAllRoles().Where(x => x.Id != model.Key).ToList();
                        foreach (var role in roles)
                        {
                            role.IsDefaultRole = false;
                            result = await roleSharedService.UpdateAsync(role);
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
            catch (Exception ex)
            {

                var message = ExceptionParser.Parse(ex);
                logger.LogError(new EventId(500), message, ex);
                throw ex;
            }
        }

    }
}