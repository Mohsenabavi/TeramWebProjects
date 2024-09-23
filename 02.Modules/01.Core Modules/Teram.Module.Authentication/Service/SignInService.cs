using Teram.Module.Authentication.Models;
using Teram.ServiceContracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Teram.Module.Authentication.Service
{
    public class SignInService : ISignInSharedService
    {

        private readonly SignInManager<TeramUser> signInManager;
        private readonly UserManager<TeramUser> userManager;

        public SignInService(UserManager<TeramUser> userManager, SignInManager<TeramUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        

        public bool IsSignedIn(ClaimsPrincipal principal)
        {
            var result=signInManager.IsSignedIn(principal);
            return result;
        }
         
        public async Task RefreshSignInAsync(UserInfo userInfo)
        {
            var user = await userManager.FindByIdAsync(userInfo.UserId.ToString());
            await signInManager.RefreshSignInAsync(user);
        }

        public async Task SignInAsync(UserInfo userInfo, bool isPersistent, string authenticationMethod = null)
        {
            var user = await userManager.FindByIdAsync(userInfo.UserId.ToString());
            await signInManager.SignInAsync(user, isPersistent: false);
        }

        public async Task SignOutAsync()
        {
            await signInManager.SignOutAsync();
        }
    }
}
