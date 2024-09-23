using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Teram.Module.Authentication.Models;
using Microsoft.Extensions.Localization;
using Teram.Web.Core;
using Teram.Framework.Core.Tools;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Logging;
using Teram.GlobalConfiguration;

namespace Teram.Module.Authentication.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<TeramUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ForgotPasswordModel> logger;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly HtmlTemplateParser htmlTemplateParser;

        public ForgotPasswordModel(UserManager<TeramUser> userManager, IEmailSender emailSender, 
            ILogger<ForgotPasswordModel> logger,
            IStringLocalizer<SharedResource> localizer,
            HtmlTemplateParser htmlTemplateParser
            )
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            this.htmlTemplateParser = htmlTemplateParser ?? throw new ArgumentNullException(nameof(htmlTemplateParser));
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_Email_is_required))]
            [RegularExpression("^([\\w-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([\\w-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$", ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.Please_enter_correct_Email))]
            [MaxLength(150, ErrorMessageResourceType = typeof(Teram.Module.Authentication.Resources.AuthenticationSharedResource), ErrorMessageResourceName = nameof(Teram.Module.Authentication.Resources.AuthenticationSharedResource.The_field_Email_must_be_a_string_or_array_type_with_a_maximum_length_of__100__))]
            [Display(Name = "Email")]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    logger.LogWarning(TeramEvents.UnableToLoadUser, "Unable to load user {0} for forget password confirmation, {1} ip address", Input.Email, HttpContext.Connection.RemoteIpAddress);
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                var values = new Dictionary<string, string>
                    {
                        { "token", HtmlEncoder.Default.Encode(callbackUrl) },
                    };

                try
                {
                    logger.LogInformation(TeramEvents.SendConfirmationEmail, "Confirmation email for forget password sent at {1} to {0}.", Input.Email, DateTime.Now, Request.HttpContext.Connection.RemoteIpAddress);

                    var res = _emailSender.SendEmailAsync(Input.Email, _localizer["Reset password"], htmlTemplateParser.Parse("Email", "ResetPassword", CultureInfo.CurrentUICulture, values.ToArray()));
                }
                catch (Exception ex)
                {

                    logger.LogError(TeramEvents.FailedToSendEmailConfirmation, ex, "Confirmation email for forget password not sent to {0}.", Input.Email, DateTime.Now, Request.HttpContext.Connection.RemoteIpAddress);

                }

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }

        [AllowAnonymous]
        public string CallBackHome()
        {
            return "/Index";
        }
    }
}
