using Microsoft.AspNetCore.Http;
using Teram.Module.Authentication.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Teram.Web.Core.Security;
using Teram.Web.Core.Helper;
using Teram.Web.Core.Model;

namespace Teram.Module.Authentication.Models
{
    public class Security : ISecurity
    {
        private readonly HttpContext _httpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActionDiscoveryService _actionDiscoveryService;

        public Security(
            IHttpContextAccessor httpContextAccessor,
            IActionDiscoveryService actionDiscoveryService)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(_httpContextAccessor));
            _httpContext = _httpContextAccessor.HttpContext;
            _actionDiscoveryService = actionDiscoveryService ?? throw new ArgumentNullException(nameof(_actionDiscoveryService));
        }

        public bool HasAccess(string area, string controller, string action)
        {
            return _httpContext != null && HasAcces(_httpContext.User, area, controller, action);
        }

        public bool HasAcces(ClaimsPrincipal user, string area, string controller, string action)
        {
            var currentClaimValue = $"{area}:{controller}:{action}";
            
            var securedControllerActions = _actionDiscoveryService.GetAllSecureActionsWithPolicy(ConstantPolicies.RolePermission);

            var controllerInfo = securedControllerActions.FirstOrDefault(x => x.Path == currentClaimValue);
            if (controllerInfo==null)
            {
                throw new KeyNotFoundException($"The `secured` area={area}/controller={controller}/action={action} with `Permission` policy not found. Please check you have entered the area/controller/action names correctly and also it's decorated with the correct security policy.");
            }
            
            if (!user.Identity.IsAuthenticated)
            {
                return false;
            }
            if (user.IsInRole("Administrator")) return true;
            if (controllerInfo.AuthorizedByAnotherAction)
            {

                var finalAreaPath = string.IsNullOrEmpty(controllerInfo.AuthorizedAreaName) ? area : controllerInfo.AuthorizedAreaName;
                var finalControllerPath = string.IsNullOrEmpty(controllerInfo.AuthorizedControllerName) ? controller: controllerInfo.AuthorizedControllerName;

                currentClaimValue = $"{finalAreaPath}:{finalControllerPath}:{controllerInfo.AuthorizedActionName}";
            }
            
            // Check for dynamic permissions
            // A user gets its permissions claims from the `ApplicationClaimsPrincipalFactory` class automatically and it includes the role claims too.
            return user.HasClaim(claim => claim.Type == ConstantPolicies.Permission && claim.Value == currentClaimValue);
        }

        public bool HasPermission(string permissionName)
        {
            var user = _httpContext.User;
            if (user.IsInRole("Administrator")) return true;
            return user.HasClaim(claim => claim.Type == ConstantPolicies.Permission && claim.Value == permissionName);

        }

        public bool HasRole(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
