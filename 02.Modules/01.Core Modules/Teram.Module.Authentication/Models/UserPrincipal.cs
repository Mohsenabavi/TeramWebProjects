using Teram.ServiceContracts;
using Teram.Web.Core.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Teram.Module.Authentication.Models
{
    public class UserPrincipal : IUserPrincipal
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<TeramUser> userManager;
        private readonly IUserSharedService userSharedService;

        public UserPrincipal(IHttpContextAccessor httpContextAccessor, UserManager<TeramUser> userManager, IUserSharedService userSharedService)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.userSharedService = userSharedService;

        }
        public ClaimsPrincipal CurrentUser => httpContextAccessor.HttpContext.User;
        public IPAddress CurrentIpAddress => httpContextAccessor.HttpContext.Connection.RemoteIpAddress;


        public Guid CurrentUserId => Guid.Parse(userManager.GetUserId(CurrentUser));
        public string CurrentUserFullName
        {
            get
            {
                return UserInfo.Name;
            }
        }

         

        public string Fullname => FullName;

        private string FullName
        {
            get
            {
                var task = Task.Run(async () =>
                {
                    var userInfo = await userSharedService.GetUserById(CurrentUserId);
                    return userInfo;
                });
                task.Wait();
                var userFullName = task.Result.Name;

                return userFullName;
            }
        }

        public UserGeneralInfo UserInfo { get; set; }
        public UserPrincipal()
        {
            var task = Task.Run(async () =>
            {
                var userInfo = await userSharedService.GetUserById(CurrentUserId);
                return userInfo;
            });
            task.Wait();
            var userFullName = task.Result.Name;
            UserInfo = new UserGeneralInfo { Name = userFullName };

        }
    }

}
