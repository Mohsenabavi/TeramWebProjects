using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Teram.Web.Core.Attributes;
using Teram.Web.Core;
using Teram.Web.Models;
using Teram.Web.Properties;
using System.Diagnostics;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Identity;
using Teram.Module.Authentication.Models;
using System.Linq;
using System.Security.Claims;
using Teram.Module.Authentication.Constant;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Teram.GlobalConfiguration;
using System.Collections.Generic;

namespace Teram.Web.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<SharedResource> localizer;       

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<SharedResource> localizer)
        {
            _logger = logger;
            this.localizer = localizer;
            //this.emailJob = emailJob ?? throw new ArgumentNullException(nameof(emailJob));
        }

        public async Task<IActionResult> CreateDefaultOption([FromServices] RoleManager<TeramRole> roleManager, [FromServices] UserManager<TeramUser> userManager, string superUserPassword)
        {
            if (superUserPassword != "Teram@CMS2021")
            {
                return Content("Your not Auhtorized");
            }
            var role = roleManager.Roles.FirstOrDefault(x => x.IsDefaultRole);
            var adminRole = await roleManager.FindByNameAsync("Administrators");
           
            if (role == null)
            {
                role = new TeramRole
                {
                    Name = "Members",
                    IsDefaultRole = true
                };
                await roleManager.CreateAsync(role);
            }
            if (adminRole == null)
            {
                await roleManager.CreateAsync(new TeramRole
                {
                    Name = "Administrators",
                });
            }

            if (await userManager.FindByNameAsync("develop_manager") == null)
            {
                var user = new TeramUser
                {
                    Email = "admin@site.com",
                    UserName = "develop_manager",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "Sup3rUz3r@2021");

                await userManager.AddToRoleAsync(user, "Administrators");

                var claim = new Claim(ConstantPolicies.Permission, ":PermissionControlPanel:SavePermission");
                await roleManager.AddClaimAsync(adminRole, claim);
                claim = new Claim(ConstantPolicies.Permission, ":PermissionControlPanel:Save");
                await roleManager.AddClaimAsync(adminRole, claim);
                claim = new Claim(ConstantPolicies.Permission, ":PermissionControlPanel:Index");
                await roleManager.AddClaimAsync(adminRole, claim);
                claim = new Claim(ConstantPolicies.Permission, ":PermissionControlPanel:GetRole");
                await roleManager.AddClaimAsync(adminRole, claim);
                claim = new Claim(ConstantPolicies.Permission, ":ControlPanel:Index");
                await roleManager.AddClaimAsync(adminRole, claim);
                claim = new Claim(ConstantPolicies.Permission, ":ControlPanel:GridOnly");
                await roleManager.AddClaimAsync(adminRole, claim);
                claim = new Claim(ConstantPolicies.Permission, ":ControlPanel:Save");
                await roleManager.AddClaimAsync(adminRole, claim);
                claim = new Claim(ConstantPolicies.Permission, ":ControlPanel:Edit");
                await roleManager.AddClaimAsync(adminRole, claim);
                claim = new Claim(ConstantPolicies.Permission, ":PermissionControlPanel:GetPermissions");
                await roleManager.AddClaimAsync(adminRole, claim);





            }
            return Content("OK");
        }
        public async Task<IActionResult> SetAdminRole([FromServices] RoleManager<TeramRole> roleManager, [FromServices] UserManager<TeramUser> userManager, string superUserPassword)
        {
            if (superUserPassword != "Teram@CMS2021")
            {
                return Content("Your not Auhtorized");
            }
            var adminRole = await roleManager.FindByNameAsync("WebAdministrators");

            if (adminRole == null)
            {
                return Content("Admin role not exists");

            }
            try
            {


                var claim = new Claim(ConstantPolicies.Permission, ":PermissionControlPanel:SavePermission");
                await roleManager.AddClaimAsync(adminRole, claim);
                claim = new Claim(ConstantPolicies.Permission, ":PermissionControlPanel:Index");
                await roleManager.AddClaimAsync(adminRole, claim);
                claim = new Claim(ConstantPolicies.Permission, ":PermissionControlPanel:Save");
                await roleManager.AddClaimAsync(adminRole, claim);
                claim = new Claim(ConstantPolicies.Permission, ":PermissionControlPanel:GetRole");
                await roleManager.AddClaimAsync(adminRole, claim);
                claim = new Claim(ConstantPolicies.Permission, ":ControlPanel:Index");
                await roleManager.AddClaimAsync(adminRole, claim);
                claim = new Claim(ConstantPolicies.Permission, ":ControlPanel:GridOnly");
                await roleManager.AddClaimAsync(adminRole, claim);
                claim = new Claim(ConstantPolicies.Permission, ":ControlPanel:Save");
                await roleManager.AddClaimAsync(adminRole, claim);
                claim = new Claim(ConstantPolicies.Permission, ":ControlPanel:Edit");
                await roleManager.AddClaimAsync(adminRole, claim);
                claim = new Claim(ConstantPolicies.Permission, ":PermissionControlPanel:GetPermissions");
                await roleManager.AddClaimAsync(adminRole, claim);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RoleClaim");
                throw;
            }

            return Content("OK");
        }

        public IActionResult Index()
        {
            var home = localizer["Home"];
            var fpwd = localizer["ForgetPassword"];
            return View();
        }
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                "UserCulture",
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("en-US", culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
        public IActionResult SetLanguage(string culture="fa-IR")
        {
            Response.Cookies.Append(
                "UserCulture",
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("en-US",culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return Content("OK");
        }
        [EmailSender(Key = "sendTest", Title = "تست ارسال میل")]
        public async Task<IActionResult> CheckMail()
        {
            try
            {
               

                //await emailJob.SendEmailAsync(typeof(HomeController), "sendTest", new System.Collections.Generic.List<string> { "hoseini.a@Teramgroup.com" }, new System.Collections.Generic.List<string> { "hoseini.a@Teramgroup.com" },
                //    new System.Collections.Generic.List<string> { "hoseini.a@Teramgroup.com" }, "testHossseini", null, "TestMailHomeController", null);

                return Content("OK");

            }
            catch (Exception ex)
            {

                return Content(ex.Message);

            }

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult NotFound()
        {
            return View("NotFound");
        }
    }
}
