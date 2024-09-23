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
using Microsoft.Extensions.Logging;
using Teram.GlobalConfiguration;

namespace Teram.Module.Authentication.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailChangeModel : PageModel
    {
        private readonly UserManager<TeramUser> _userManager;
        private readonly ILogger<ConfirmEmailChangeModel> logger;
        private readonly SignInManager<TeramUser> _signInManager;

        public ConfirmEmailChangeModel(UserManager<TeramUser> userManager,ILogger<ConfirmEmailChangeModel> logger, SignInManager<TeramUser> signInManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
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
            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                StatusMessage = "Error changing email.";
                var errors = result.Errors.Select(x => x.Description).Aggregate((x, c) => x + ">>" + c);
                logger.LogError(TeramEvents.ChangingEmailFailed, "User {0} with {4} address failed to change email address due to {1} from {2} ip address at {3}", userId, errors, HttpContext.Connection.RemoteIpAddress, DateTime.Now,email);

                return Page();
            }

            // In our UI email and user name are one and the same, so when we update the email
            // we need to update the user name.
            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
            if (!setUserNameResult.Succeeded)
            {
                StatusMessage = "Error changing user name.";
                var errors = result.Errors.Select(x => x.Description).Aggregate((x, c) => x + ">>" + c);
                logger.LogError(TeramEvents.ChangingEmailFailed, "User {0} with {4} address failed to change username due to {1} from {2} ip address at {3}", userId, errors, HttpContext.Connection.RemoteIpAddress, DateTime.Now, email);

                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Thank you for confirming your email change.";
            return Page();
        }
    }
}
