using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Teram.Module.Authentication.Models;
using Microsoft.Extensions.Localization;
using Teram.Web.Core;
using Microsoft.Extensions.Logging;
using Teram.GlobalConfiguration;

namespace Teram.Module.Authentication.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<TeramUser> _userManager;
        private readonly ILogger<ConfirmEmailModel> logger;
        private readonly IStringLocalizer<SharedResource> localizer;

        public ConfirmEmailModel(UserManager<TeramUser> userManager, ILogger<ConfirmEmailModel> logger, IStringLocalizer<SharedResource> localizer)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                logger.LogWarning(TeramEvents.UnableToLoadUser, "Unable to load user with ID {0}, {1} at {2}", userId, HttpContext.Connection.RemoteIpAddress, DateTime.Now);
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? localizer["Thank you for confirming your email."] : localizer["Error confirming your email."];

            if (result.Succeeded)
            {

                logger.LogInformation(TeramEvents.EmailConfirmation, "User {0} emails has been confirmed from {1} ip address at {2}", userId, HttpContext.Connection.RemoteIpAddress, DateTime.Now);
            }
            else
            {
                var errors = result.Errors.Select(x => x.Description).Aggregate((x, c) => x + ">>" + c);
                logger.LogInformation(TeramEvents.EmailConfirmationFailed, "User {0} emails not confirmed due to {1} from {2} ip address at {3}", userId, errors, HttpContext.Connection.RemoteIpAddress, DateTime.Now);

            }


            return Page();
        }
    }
}
