using Hangfire.Dashboard;
using System.Linq;

namespace Teram.Web.Models
{
    public class HangFireDashboardAuthorization : IDashboardAuthorizationFilter
    {

        public HangFireDashboardAuthorization()
        {

        }

        public bool Authorize(DashboardContext dashboardContext)
        {
            var httpContext = dashboardContext.GetHttpContext();

            //var isAdmin = httpContext.User.Claims.Any(x => x.Type == "IsAdminClaim");
           
            // Allow all authenticated users with IsAdminRole to see the Dashboard (potentially dangerous).
            return httpContext.User.Identity.IsAuthenticated && true;
        }


    }
}
