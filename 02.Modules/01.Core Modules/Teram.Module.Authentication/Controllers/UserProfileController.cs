using Teram.Framework.Core.Tools;
using Teram.Module.Authentication.Models;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Teram.Module.Authentication.Controllers
{
    public class UserProfileController : BasicControlPanelController
    {

        public UserProfileController(ILogger<UserProfileController> logger, IStringLocalizer<UserProfileController> localizer, IStringLocalizer<SharedResource> sharedLocalizer)
        {
            ViewBag.PageName = localizer["User Profile"];
            Model = new ViewInformation<TeramUser>(true)
            {
                
                Title = localizer["User Profile"],
                HomePage = "/Userprofile/Index",
                HasGrid = false
            };
            this.logger = logger;
            this.sharedLocalizer = sharedLocalizer;
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));

        }
        //[ControlPanelMenu("User Profile", ParentName = "Security", Icon = "fa-users", PanelType = PanelType.User | PanelType.Merchant | PanelType.Managment | PanelType.Customer,
        //    Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar, Order = 1)]
        public async Task<IActionResult> Index([FromServices] IUserPrincipal userPrincipal, [FromServices] UserManager<TeramUser> userManager)
        {
            var user = await userManager.GetUserAsync(userPrincipal.CurrentUser);
            if (user == null)
            {
                return LocalRedirect("/Error");
            }

            return View(user);
        }

        public async Task<ActionResult> Save([FromServices] IUserPrincipal userPrincipal, [FromServices] SignInManager<TeramUser> signInManager, [FromServices] UserManager<TeramUser> userManager, TeramUser model)
        {
            try
            {


                var currentUser = await userManager.GetUserAsync(userPrincipal.CurrentUser);
                //currentUser.FirstName = model.FirstName;
                //currentUser.LastName = model.LastName;
                //currentUser.DateOfBirth = model.DateOfBirth;
                //currentUser.Gender = model.Gender;
                //currentUser.City = model.City;
                //currentUser.TelegramId = model.TelegramId;
                currentUser.PhoneNumber = model.PhoneNumber;




                var r = await userManager.UpdateSecurityStampAsync(currentUser);

                var result = await userManager.UpdateAsync(currentUser);
                await signInManager.RefreshSignInAsync(currentUser);

                if (result.Succeeded)
                {
                    return Json(new { result = "ok", message = localizer["Your data has been saved"], title = localizer["SaveTitle"] });

                }
                else
                {
                    var message = result.Errors.Select(x => x.Description).Aggregate((x, c) => x + Environment.NewLine + c);
                    return Json(new { result = "fail", message = message, title = localizer["Something wrong"] });
                }

            }
            catch (Exception ex)
            {

                var message = ExceptionParser.Parse(ex);
                logger.LogError(new EventId(500), message, ex);
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            ViewBag.PageName = localizer["Change Password"];


            return View(new PasswordModel());
        }
        [HttpPost]
        [ParentalAuthorize(nameof(ChangePassword))]
        public async Task<ActionResult> ChangePassword(PasswordModel model, [FromServices] SignInManager<TeramUser> signInManager, [FromServices] UserManager<TeramUser> userManager)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = localizer["Model is not valid"] });

            }
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { result = "fail", message = localizer["Model is not valid"] });
            }
            var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                var message = result.Errors.Select(x => x.Description).Aggregate((x, c) => x + Environment.NewLine + c);
                return Json(new { result = "fail", message = message, title = localizer["Something wrong"] });
            }
            user.PassWordChanged=true;
            await userManager.UpdateAsync(user);
            await signInManager.RefreshSignInAsync(user);            
            return Json(new { result = "ok", message = localizer["Your password has been changed"], title = localizer["Change Password"] });
        }
    }
}