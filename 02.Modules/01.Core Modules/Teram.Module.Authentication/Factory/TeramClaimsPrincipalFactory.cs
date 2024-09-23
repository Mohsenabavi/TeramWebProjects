using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Teram.Module.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Teram.Module.Authentication.Factory
{
    public class TeramClaimsPrincipalFactory : UserClaimsPrincipalFactory<TeramUser, TeramRole>
    {
        private readonly UserManager<TeramUser> userManager;
        private readonly RoleManager<TeramRole> roleManager;
        private readonly IOptions<IdentityOptions> options;
        public static readonly string PhotoFileName = nameof(PhotoFileName);

        public TeramClaimsPrincipalFactory(UserManager<TeramUser> userManager, RoleManager<TeramRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public override async Task<ClaimsPrincipal> CreateAsync(TeramUser user)
        {
            var principal = await base.CreateAsync(user);
            var defaultRole = roleManager.Roles.FirstOrDefault(x => x.IsDefaultRole);
            if (defaultRole != null)
            {
                await userManager.AddToRoleAsync(user, defaultRole.Name);
            }
            
            AddCustomClaims(user, principal);
            return principal;

        }

        private void AddCustomClaims(TeramUser user, ClaimsPrincipal principal)
        {
            ((ClaimsIdentity)principal.Identity).AddClaims(new[]
          {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.Integer),
                //new Claim(ClaimTypes.GivenName, user.FirstName ?? string.Empty),
             

            });
        }
    }
}
