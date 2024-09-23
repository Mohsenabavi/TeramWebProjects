using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Teram.Module.Authentication.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Teram.Web.Core;
using System;
using Teram.GlobalConfiguration;
using DNTCaptcha.Core;
using Microsoft.Extensions.Options;

namespace Teram.Module.Authentication.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<TeramUser> _userManager;
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly IDNTCaptchaValidatorService validatorService;
        private readonly SignInManager<TeramUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly DNTCaptchaOptions _captchaOptions;

        public LoginModel(SignInManager<TeramUser> signInManager,
            ILogger<LoginModel> logger,
            UserManager<TeramUser> userManager,
            IStringLocalizer<SharedResource> localizer,
            IDNTCaptchaValidatorService _validatorService,
            IOptions<DNTCaptchaOptions> options
            )
        {
            _userManager = userManager??throw new ArgumentNullException(nameof(userManager));
            this.localizer = localizer??throw new ArgumentNullException(nameof(localizer));
            validatorService=_validatorService??throw new ArgumentNullException(nameof(_validatorService));
            _signInManager = signInManager??throw new ArgumentNullException(nameof(signInManager));
            _logger = logger??throw new ArgumentNullException(nameof(logger));
            _captchaOptions = options == null ? throw new ArgumentNullException(nameof(options)) : options.Value;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_NationalCode_is_required))]
            [Display(Name = "NationalCode")]
            public string NationalCode { get; set; }

            [Required(ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_Password_is_required))]
            [MinLength(8, ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Password_Min_Length_Error))]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "RememberMe")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!validatorService.HasRequestValidCaptchaEntry())
            {
                this.ModelState.AddModelError(_captchaOptions.CaptchaComponent.CaptchaInputName, "Please enter the security code as a number.");
            }

            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.NationalCode, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var userInfo = await _userManager.FindByNameAsync(Input.NationalCode);
                    if (userInfo!=null)
                    {
                        userInfo.LoginCount++;
                        await _userManager.UpdateAsync(userInfo);

                        var userRoles = await _userManager.GetUsersInRoleAsync("User");

                        var userIsInRole = userRoles.FirstOrDefault(x => x.UserName==userInfo.UserName);

                        if (userInfo.LoginCount ==1 && userInfo!=null && userIsInRole!=null)
                        {
                            returnUrl ="/WorkWithUs/Index";
                        }
                    }
                    _logger.LogInformation(TeramEvents.Loging, "User {0} logged in at {1} from {2} IP address.", Input.NationalCode, DateTime.Now, Request.HttpContext.Connection.RemoteIpAddress);
                    return new JsonResult(new { result = "redirect", url = returnUrl });
                }
                if (result.RequiresTwoFactor)
                {
                    _logger.LogInformation(TeramEvents.TwoFactorRequired, "Two factor required for User {0}", Input.NationalCode, DateTime.Now, Request.HttpContext.Connection.RemoteIpAddress);
                    return new JsonResult(new { result = "redirect", url = $"/Identity/Account/LoginWith2fa?ReturnUrl{returnUrl}&RememberMe{Input.RememberMe}" });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(TeramEvents.Lockout, "User {0} locked out and try to loggin at {1} from {2} IP address.", Input.NationalCode, DateTime.Now, Request.HttpContext.Connection.RemoteIpAddress);
                    return new JsonResult(new { result = "redirect", url = $"/Identity/Account/LockOut" });
                }
                else
                {
                    _logger.LogWarning(TeramEvents.InvalidLoggingAttempt, "Inavlid login attempt for User {0} try to loggin at {1} from {2} IP address.", Input.NationalCode, DateTime.Now, Request.HttpContext.Connection.RemoteIpAddress);
                    ModelState.AddModelError(string.Empty, localizer["InvalidLoginAttempt"]);
                    return new JsonResult(new { result = "invalid", title = localizer["Login"], message = localizer["InvalidLoginAttempt"] });
                }
            }
            return new JsonResult(new { result = "invalid", title = localizer["Login"], message = localizer["تلاش نا موفق برای ورود به سیستم"] });
        }

        [AllowAnonymous]
        public string CallBackHome()
        {
            return "/Index";
        }
    }
}
