using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Teram.Module.Authentication.Models;
using Teram.GlobalConfiguration;
using Teram.Web.Core.Security;

namespace Teram.Module.Authentication.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<TeramUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly IUserPrincipal userPrincipal;

        public LogoutModel(SignInManager<TeramUser> signInManager, ILogger<LogoutModel> logger,
            IUserPrincipal userPrincipal
            )
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.userPrincipal = userPrincipal ?? throw new ArgumentNullException(nameof(userPrincipal));
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            var user = "Anonymous";
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                user = userPrincipal.CurrentUserId.ToString();
            }
            _logger.LogInformation(TeramEvents.UserLogOut, "User {0} logged out.", HttpContext.User.Identity);
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
