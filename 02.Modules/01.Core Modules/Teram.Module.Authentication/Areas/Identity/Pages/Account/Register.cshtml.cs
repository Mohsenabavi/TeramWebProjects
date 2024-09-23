using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

using Teram.Module.Authentication.Models;
using Teram.Framework.Core.Tools;
using Teram.Web.Core;
using Teram.Module.Authentication.Constant;
using Teram.GlobalConfiguration;

namespace Teram.Module.Authentication.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<TeramUser> _signInManager;
        private readonly UserManager<TeramUser> _userManager;
        private readonly RoleManager<TeramRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly HtmlTemplateParser htmlTemplateParser;
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly IStringLocalizer<AuthenticationSharedResource> localizer1;

        public RegisterModel(UserManager<TeramUser> userManager, RoleManager<TeramRole> roleManager, SignInManager<TeramUser> signInManager, ILogger<RegisterModel> logger, IEmailSender emailSender,
            HtmlTemplateParser htmlTemplateParser, IStringLocalizer<SharedResource> localizer, IStringLocalizer<AuthenticationSharedResource> localizer1
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            this.htmlTemplateParser = htmlTemplateParser;
            this.localizer = localizer;
            this.localizer1 = localizer1;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {

            [Required(ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_PersonType_is_required))]
            public int PersonType { get; set; }

            [Required(ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_NationalCode_is_required))]
            [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Please_enter_valid_National_Code_as_User_Name))]
            [MinLength(10, ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_National_Code_must_be_a_string_or_array_type_with_a_minimum_length_of__10__))]
            [MaxLength(11, ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_National_Code_must_be_a_string_or_array_type_with_a_maximum_length_of__11__))]
            public string NationalCode { get; set; }

            [Required(ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_Email_is_required))]
            [RegularExpression("^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Please_enter_correct_Email))]
            [StringLength(150)]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_Password_is_required))]
            [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Please_enter_valid_Password))]
            [MinLength(8, ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Password_Min_Length_Error))]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "ConfirmPassword")]
            [MinLength(8, ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Password_Min_Length_Error))]
            [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Please_enter_valid_Password))]
            [Compare("Password", ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Password_mismatch))]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                if ((Input.PersonType == 1) && (Input.NationalCode.Length != 10))
                {
                    return new JsonResult(new { result = "Fail", title = localizer["Register"], message = localizer1["Please neter valid national code for real and legal person"] });

                }
                if ((Input.PersonType == 2) && (Input.NationalCode.Length != 11))
                {
                    return new JsonResult(new { result = "Fail", title = localizer["Register"], message = localizer1["Please neter valid national code for real and legal person"] });
                }

                var user = new TeramUser { UserName = Input.NationalCode, Email = Input.Email };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation(TeramEvents.RegisterNewUser, "User {0} created a new account with password at {1} from {2} IP address.", Input.NationalCode, DateTime.Now, Request.HttpContext.Connection.RemoteIpAddress);

                    var defaultRole = _roleManager.Roles.FirstOrDefault(x => x.IsDefaultRole);
                    await _userManager.AddToRoleAsync(user, defaultRole.Name);

                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ConstantPolicies.PersonTypeClaim, Input.PersonType.ToString()));
                    await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ConstantPolicies.NationalCodeClaim, Input.NationalCode.ToString()));

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    var values = new Dictionary<string, string>
                    {
                        { "token", HtmlEncoder.Default.Encode(callbackUrl) },
                    };

                    try
                    {
                        _logger.LogInformation(TeramEvents.SendConfirmationEmail, "Confirmation email sent at {1} to {0}.", Input.Email, DateTime.Now, Request.HttpContext.Connection.RemoteIpAddress);

                        var res = _emailSender.SendEmailAsync(Input.Email, localizer["Confirm email"], htmlTemplateParser.Parse("Email", "EmailConfirmation", CultureInfo.CurrentUICulture, values.ToArray()));

                    }
                    catch (Exception ex)
                    {

                        _logger.LogError(TeramEvents.FailedToSendEmailConfirmation, ex, "Confirmation email not sent to {0}.", Input.Email, DateTime.Now, Request.HttpContext.Connection.RemoteIpAddress);

                    }

                    //$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."); ;

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return new JsonResult(new { result = "Ok", title = localizer["Register"], message = localizer1["Successfully addedd,Please check your email to confirm your account."] });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return new JsonResult(new { result = "redirect", url = returnUrl });
                    }
                }
                var message = "";
                foreach (var error in result.Errors)
                {
                    message += error.Description + Environment.NewLine;
                }
                _logger.LogError(TeramEvents.FailedToRegisterNewUser, "Failed to create User {0} at {1} from {2} IP address." + Environment.NewLine + message, Input.NationalCode, DateTime.Now, Request.HttpContext.Connection.RemoteIpAddress);

                return new JsonResult(new { result = "Fail", title = localizer["Register"], message = message });

            }
            return new JsonResult(new { result = "Fail", title = localizer["Register"], message = localizer["UnkownError"] });

            // If we got this far, something failed, redisplay form
        }

        [AllowAnonymous]
        public string CallBackHome()
        {
            return "/Index";
        }
    }
}
