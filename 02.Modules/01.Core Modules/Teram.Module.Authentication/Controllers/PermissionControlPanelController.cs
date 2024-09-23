using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Teram.Module.Authentication.Constant;
using Teram.Module.Authentication.Models;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Helper;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.Security;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Teram.Web.Core;
using System.Text.Json;
using Teram.ServiceContracts;
using System.Reflection;
using Teram.Framework.Core.Extensions;

namespace Teram.Module.Authentication.Controllers
{
    [Display(Description = "Permissions")]
    [ControlPanelMenu("Security", Icon = "fa-user-lock", PanelType = PanelType.Managment, Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar, Order = 0)]
    public class PermissionControlPanelController : BasicControlPanelController
    {
        private readonly IServiceProvider serviceProvider;

        private readonly IActionDiscoveryService actionDiscoveryService; 
        private readonly IRoleSharedService roleSharedService;
        private readonly IUserSharedService userSharedService;
        private readonly ISignInSharedService signInSharedService;

        public int MyProperty { get; set; }
        public PermissionControlPanelController(IServiceProvider serviceProvider, ILogger<PermissionControlPanelController> logger,
            IStringLocalizer<PermissionControlPanelController> localizer, IActionDiscoveryService actionDiscoveryService,
          IRoleSharedService roleSharedService, IUserSharedService userSharedService,
            ISignInSharedService signInSharedService, IStringLocalizer<SharedResource> sharedlocalizer)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            this.actionDiscoveryService = actionDiscoveryService ?? throw new ArgumentNullException(nameof(actionDiscoveryService));
            //this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.roleSharedService = roleSharedService;
            this.userSharedService = userSharedService;
            this.signInSharedService = signInSharedService;
            this.logger = logger;
            this.sharedLocalizer = sharedlocalizer;
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));


            ViewBag.PageName = localizer["Permissions"];
            Model = new ViewInformation<PermissionModel>(true)
            {
                Title = localizer["Permissions"],
                HomePage = "/PermissionControlPanel/Index",
                ExtraScripts = "/ExternalModule/Module/Authentication/Scripts/PermissionControlPanel.js"
            };
            this.serviceProvider = serviceProvider;
        }

        [Display(Description = "ShowPage")]
        [ControlPanelMenu("Permissions", ParentName = "Security", Icon = "fa-user-lock", PanelType = PanelType.Managment, Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar, Order = 6)]
        public IActionResult Index()
        {
            ViewBag.Roles = GetRoles();
            return View("Index", Model);
        }
        private string GetDisplayName(string controllerName, TypeInfo controller)
        {
            var localizer = controller.GetLocalizer(serviceProvider);
            if (localizer == null) return controllerName;
            return localizer[controllerName];
        }
        [ParentalAuthorize(nameof(Index))]
        private Task<List<PermissionModel>> GetEmptyPermission()
        {
            var result = Task.Run(() =>
            {
                var actions = actionDiscoveryService.GetRootSecureActionsWithPolicy(ConstantPolicies.Permission);
                var model = actions.GroupBy(x => new { x.AreaName, x.ControllerName }).Select(x => new PermissionModel
                {
                    DisplayName = x.Key.ControllerName,
                    ControllerKey = x.Key.AreaName + "-" + x.Key.ControllerName,
                    Actions = x.Select(z => new ActionInfo { ActionName = z.ActionName, Path = z.Path, Displayname = GetDisplayName(z.ActionName,z.ControllerInfo), HasAccess = false }).ToList()
                }).ToList();
                return model;
            });
            //GetAllControllerActions();

            return result;
        }


        [ParentalAuthorize(nameof(Index))]
        public async Task<IActionResult> GetTreeJson(Guid id)
        {
            var roleInfo = await roleSharedService.GetRoleById(id);
            var model = new List<PermissionModel>();
            if (roleInfo == null)
            {
                model = await GetEmptyPermission();
            }
            else
            {
                var claims = await roleSharedService.GetClaimsAsync(roleInfo);
                var actions = actionDiscoveryService.GetRootSecureActionsWithPolicy(ConstantPolicies.RolePermission);

                model = actions.GroupBy(x => new { x.AreaName, x.ControllerName }).Select(x => new PermissionModel
                {
                    DisplayName = x.Key.ControllerName,
                    ControllerKey = x.Key.AreaName + "-" + x.Key.ControllerName,
                    Actions = x.Select(z => new ActionInfo { ActionName = z.ActionName, Path = z.Path, Displayname = GetDisplayName(z.ActionName,z.ControllerInfo), HasAccess = HasAccess(claims, z.Path) }).ToList()
                }).ToList();

            }

            var nodesList = new List<JsTreeNode>();
            JsTreeNode mainNode = null;

            // یه نود روت گذاشتم و کنترلر ها را به اون ادد کردم
            // برای اینکه با تیک زدن یا برداشتن تیک این نود قابلیت  "انتخاب همه" و "حذف همه" هم داشته باشیم
            mainNode = new JsTreeNode
            {
                Text = "",
                Icon = "fa fa-close",
                A_attr = { Class = "cheked", Path = "::", ParentName = null },
                State = { Checked = false, Opened = true },
            };
            nodesList.Add(mainNode);

            // لیست کنترلر ها
            var controllers = model.ToList();

            for (int i = 0; i < controllers.Count; i++)
            {
                var item = controllers[i];
                var actions = item.Actions.ToList();
                var hasAllAccess = actions.All(x => x.HasAccess);
                var hasAccess = actions.Any(x => x.HasAccess);
                var rootNode = new JsTreeNode
                {
                    Key = item.Key,
                    Text = item.DisplayName,
                    Icon = "fa fa-close",
                    A_attr = { Class = "cheked", Path = ":" + item.DisplayName + ":", ParentName = item.DisplayName },
                    State = { Checked = hasAllAccess, Opened = true }
                };
                mainNode.Children.Add(rootNode);
                PopulateTree(item, rootNode);
            }

            return Json(nodesList);
        }

        /// <summary>
        ///  اضافه کردن نودهای اکشن ها به کنترلر
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="node"></param>
        [ParentalAuthorize(nameof(Index))]
        private void PopulateTree(PermissionModel controller, JsTreeNode node)
        {
            // لیست اکشن ها
            var child = controller.Actions.ToList();

            foreach (var childItem in child)
            {
                var treeNode = new JsTreeNode
                {
                    Text = childItem.Displayname,
                    Icon = "fa fa-minus",
                    State = { Checked = childItem.HasAccess },
                    A_attr = { Path = ":" + controller.DisplayName + ":" + childItem.ActionName, ParentName = controller.DisplayName },
                };
                node.Children.Add(treeNode);
            }

        }

        /// <summary>
        ///  لیست نقش ها برای کمبو
        /// </summary>
        /// <returns></returns> 
        [ParentalAuthorize(nameof(Index))]
        public List<SelectListItem> GetRoles()
        {
            var roles = roleSharedService.GetAllRoles().Select(x =>
            new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();
            return roles;
        }

        /// <summary>
        ///قبلا برای نمایش دسترسی ها از این متد استفاده میشد
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        [ParentalAuthorize(nameof(Index))]
        public async Task<IActionResult> GetPermissions(Guid id)
        {
            var role = await roleSharedService.GetRoleById(id);//.Roles.FirstOrDefault(x => x.Id == id);
            var model = new List<PermissionModel>();
            if (role == null)
            {
                model = await GetEmptyPermission();
            }
            else
            {
                var claims = await roleSharedService.GetClaimsAsync(role);


                var actions = actionDiscoveryService.GetRootSecureActionsWithPolicy(ConstantPolicies.RolePermission);
                model = actions.GroupBy(x => new { x.AreaName, x.ControllerName }).Select(x => new PermissionModel
                {
                    DisplayName = x.Key.ControllerName,
                    ControllerKey = x.Key.AreaName + "-" + x.Key.ControllerName,
                    Actions = x.Select(z => new ActionInfo { ActionName = z.ActionName, Path = z.Path, Displayname = GetDisplayName(z.ActionName,z.ControllerInfo), HasAccess = HasAccess(claims, z.Path) }).ToList()
                }).ToList();


                //var ff = GetTreeJson(model);
            }

            return PartialView("_permissions", model);
        }

        /// <summary>
        /// موقع ذخیره نیاز داشتم فقط کلیم هایی را حذف کنم که الان در این پروژه هستند
        /// </summary>
        /// <param name="role">نقش</param>
        /// <returns></returns>
        [ParentalAuthorize(nameof(Index))]
        public async Task<List<Claim>> GetExistsProjectsClaims(RoleInfo role)
        {
            // همه ی کلیم های نقش در همه ی پروژه ها 
            var claims = await roleSharedService.GetClaimsAsync(role);
            // اکشن های پروژه های جاری
            var actions = actionDiscoveryService.GetRootSecureActionsWithPolicy(ConstantPolicies.RolePermission)
                .Select(x => x.Path).ToList();
            // کلیم هایی که در این پروژه هستند
            var currentProjectClaims = claims.Where(x => x.Type == ConstantPolicies.Permission && actions.Contains(x.Value)).ToList();
            return currentProjectClaims;
        }


        /// <summary>
        /// ذخیره
        /// </summary>
        /// <param name="signInManager"></param>
        /// <param name="userPrincipal"></param>
        /// <param name="userManager"></param>
        /// <param name="roleId"> آیدی نقش مورد نظر برای ذخیره دسترسی هایش</param>
        /// <param name="permissionList">لیست همه نودهایی که تیک دارند</param>
        /// <param name="removedPermissions">لیست نودهایی که تیکشون برداشته میشه</param>
        /// <returns></returns>
        [Display(Description = "Save")]
        public async Task<IActionResult> SavePermission(  [FromServices] IUserPrincipal userPrincipal,
            Guid roleId, string permissionsList, string removedPermission)
        {
            var permissionList = JsonSerializer.Deserialize<List<Permission>>(permissionsList);
            var removedPermissions = JsonSerializer.Deserialize<List<Permission>>(removedPermission);

            // فلگ برای اینکه اگر یکیش خطا داد پیغام خطا نمایش دهد
            var flag = true;

            // نقش انتخاب شده 
            var role = await roleSharedService.GetRoleById(roleId);//.Roles.FirstOrDefault(x => x.Id == roleId);
            if (role == null)
            {
                return Json(new { result = "fail", message = localizer["Role not found"] });
            }

            try
            {  // همه کلیم های نقش برای اکشن هایی که در پروژه های جاری هستند
                var allclaims = await GetExistsProjectsClaims(role);

                // اول اونهایی که تیک شون برداشته شده را حذف میکنیم

                foreach (var permission in removedPermissions)
                {
                    // از اونجایی که ممکنه یکی از پرنت ها را برداشته باشه اینجا بچه هاش را بدست می یاریم و اونها را هم حذف میکنیم
                    // کنترلر ها اخرش اسم اکشن نداره و با شرط زیر پیداش کردم
                    if (permission.Path.EndsWith(":"))
                    {
                        List<Claim> childClaims;
                        // اگر تیک روت اصلی برداشته شده یعنی همه را پاک کن
                        if (permission.Path == "::")
                        {
                            childClaims = allclaims;
                        }
                        else
                        {// اگر تیک کنترل را برداشته باشه همه ی اکشن هاش را با شرط زیر به دست می اورم
                            childClaims = allclaims.Where(x =>
                              x.Value.StartsWith(permission.Path) && x.Type == ConstantPolicies.Permission).ToList();
                        }

                        // حلقه روی لیست کلیم هایی که باید حذف شود
                        foreach (var claim in childClaims)
                        {
                            var removeResult = await roleSharedService.RemoveClaimAsync(role, claim);
                            if (!removeResult.Succeeded)
                            {
                                flag = false;
                            }
                        }
                    }
                    else
                    {//  اگر تیک نودهای برگ را برداشته باشد 
                        var claim = allclaims.FirstOrDefault(x =>
                            x.Value == permission.Path && x.Type == ConstantPolicies.Permission);

                        var removeResult = await roleSharedService.RemoveClaimAsync(role, claim);
                        if (!removeResult.Succeeded)
                        {
                            flag = false; ;
                        }
                    }
                }

                // بررسی نودهایی که تیک دارند
                // اگر وجود ندارند اینسرت میشود
                foreach (var permission in permissionList)
                {
                    // همه کلیم های نقش
                    //var claims = await roleManager.GetClaimsAsync(role);
                    var claim = allclaims.FirstOrDefault(x =>
                        x.Value == permission.Path && x.Type == ConstantPolicies.Permission);

                    if (claim == null)
                    {
                        claim = new Claim(ConstantPolicies.Permission, permission.Path);
                        var result = await roleSharedService.AddClaimAsync(role, claim);
                        if (!result.Succeeded)
                        {
                            flag = false;
                        }
                    }
                }

                // رفرش یوزر
                var currentUser = await userSharedService.GetUserById(userPrincipal.CurrentUserId);
                await signInSharedService.RefreshSignInAsync(currentUser);
                // اگر حداقل یکیش خطا داده باشه پیغام میدم بعضی دسترسی ها ست نشد
                if (!flag)
                {
                    return Json(new { result = "fail", message = localizer["Some Permissions does not save"] });
                }
                return Json(new { result = "ok", message = localizer["Permissions were set"] });
            }
            catch (Exception)
            {
                return Json(new { result = "fail", message = localizer["Some thing wrong"] });
            }
        }


        [ParentalAuthorize(nameof(Index))]
        private bool HasAccess(IList<Claim> claims, string path)
        {

            return claims.Any(x => x.Type == ConstantPolicies.Permission && x.Value == path);
        }
    }
}