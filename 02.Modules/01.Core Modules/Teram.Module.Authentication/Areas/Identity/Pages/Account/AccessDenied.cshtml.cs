using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teram.GlobalConfiguration;
using Teram.Web.Core.Security;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Teram.Module.Authentication.Areas.Identity.Pages.Account
{
    public class AccessDeniedModel : PageModel
    {
        private readonly ILogger<AccessDeniedModel> logger;
        private readonly IUserPrincipal userPrincipal;

        public AccessDeniedModel(ILogger<AccessDeniedModel> logger,IUserPrincipal userPrincipal)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.userPrincipal = userPrincipal ?? throw new ArgumentNullException(nameof(userPrincipal));
        }
        public void OnGet()
        {
            var user = "Anonymous";
            if (Request.HttpContext.User.Identity.IsAuthenticated)
            {
                user = userPrincipal.CurrentUserId.ToString();
            }
            
            logger.LogWarning(TeramEvents.AccessDenied, "User {0} wants to access to the {1} from {2} IP address.",user, Request.Path.Value, userPrincipal.CurrentIpAddress);
        }
    }
}

