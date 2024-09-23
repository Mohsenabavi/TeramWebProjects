using Teram.Module.Authentication.Models;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;

namespace Teram.Module.Authentication.Service
{
    public class RoleService : IRoleSharedService
    {
        private IStringLocalizer<SharedResource> localizer;
        private readonly RoleManager<TeramRole> roleManager;
        public RoleService(IStringLocalizer<SharedResource> localizer, RoleManager<TeramRole> roleManager)
        {
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            this.roleManager = roleManager;
        }

        public async Task<RoleInfo> GetRoleById(Guid roleId)
        {
            //var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var role = await roleManager.FindByIdAsync(roleId.ToString());
            var roleInfo = new RoleInfo
            {
                Id = role.Id,
                Name = role.Name,
                NormalizedName = role.NormalizedName,
                ConcurrencyStamp = role.ConcurrencyStamp,
                IsDefaultRole = role.IsDefaultRole,
                Title = role.Title,
            };
            //transaction.Complete();
            return roleInfo;
        }

        public Task<RoleInfo> GetRoleByNameAndApplicationId(string roleName, int applicationId)
        {
            throw new NotImplementedException();
        }

        public List<RoleInfo> GetRoleByListRoleId(List<Guid> roleIds)
        {
            //var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            List<RoleInfo> lstRoleInfo = new List<RoleInfo>();

            lstRoleInfo = roleManager.Roles.Select(x => new RoleInfo
            {
                Id = x.Id,
                Name = x.Name,
                NormalizedName = x.NormalizedName,
                ConcurrencyStamp = x.ConcurrencyStamp,
                IsDefaultRole = x.IsDefaultRole,
                Title = x.Title,
            }).Where(x => roleIds.Distinct().Contains(x.Id)).ToList();
            //transaction.Complete(); 
            return lstRoleInfo;
        }

        public List<RoleInfo> GetAllRoles()
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var data = roleManager.Roles.Select(x => new RoleInfo()
            {
                Id = x.Id,
                Name = x.Name,
                NormalizedName = x.NormalizedName,
                ConcurrencyStamp = x.ConcurrencyStamp,
                Title = x.Title,
                IsDefaultRole = x.IsDefaultRole,
            }).ToList();
            transaction.Complete();
            return data;
        }

        public List<RoleInfo> GetAllApplicationRoles(int applicationId)
        {
            throw new NotImplementedException();
        }

        public RoleInfo GetDefaultRole()
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var roleInfo = roleManager.Roles.Select(x => new RoleInfo
            {
                Name = x.Name,
                NormalizedName = x.NormalizedName,
                ConcurrencyStamp = x.ConcurrencyStamp,
                IsDefaultRole = x.IsDefaultRole,
                Title = x.Title,
            }).FirstOrDefault(x => x.IsDefaultRole);
            transaction.Complete();
            return roleInfo;
        }

        public async Task<List<Claim>> GetClaimsAsync(RoleInfo roleInfo)
        {            
            var role = await roleManager.FindByIdAsync(roleInfo.Id.ToString());
            var result = await roleManager.GetClaimsAsync(role);
            return (List<Claim>)result;
        }

        public async Task<IdentityResult> RemoveClaimAsync(RoleInfo roleInfo, Claim claim)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var role = await roleManager.FindByIdAsync(roleInfo.Id.ToString());
            var result = await roleManager.RemoveClaimAsync(role, claim);
            transaction.Complete();
            return result;
        }

        public async Task<RoleInfo> GetRoleByName(string roleName)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var role = await roleManager.FindByNameAsync(roleName);
            var roleInfo = new RoleInfo
            {
                Name = role.Name,
                NormalizedName = role.NormalizedName,
                ConcurrencyStamp = role.ConcurrencyStamp,
                IsDefaultRole = role.IsDefaultRole,
                Title = role.Title,
            };
            transaction.Complete();
            return roleInfo;
        }

        public async Task<IdentityResult> DeleteAsync(RoleInfo roleInfo)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var TeramRole = await roleManager.FindByIdAsync(roleInfo.Id.ToString());
            var result = await roleManager.DeleteAsync(TeramRole);
            transaction.Complete();
            return result;
        }

        public async Task<IdentityResult> CreateAsync(RoleInfo roleInfo)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var TeramRole = new TeramRole() { ConcurrencyStamp = Guid.NewGuid().ToString() };
            TeramRole.Name = roleInfo.Name; 
            TeramRole.Title = roleInfo.Title;
            TeramRole.IsDefaultRole = roleInfo.IsDefaultRole;
            TeramRole.NormalizedName = roleInfo.Name.ToUpper();
            var result = await roleManager.CreateAsync(TeramRole);
            transaction.Complete();
            return result;
        }

        public async Task<IdentityResult> UpdateAsync(RoleInfo roleInfo)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var TeramRole = await roleManager.FindByIdAsync(roleInfo.Id.ToString());
            TeramRole.Name = roleInfo.Name;
            TeramRole.Title = roleInfo.Title;
            TeramRole.IsDefaultRole = roleInfo.IsDefaultRole;
            TeramRole.NormalizedName = roleInfo.Name.ToUpper();
            var result = await roleManager.UpdateAsync(TeramRole);
            transaction.Complete();
            return result;
        }

        public bool IsExistsDefaultRole()
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var result = roleManager.Roles.Any(x => x.IsDefaultRole);
            transaction.Complete();
            return result;
        }

        public async Task<IdentityResult> AddClaimAsync(RoleInfo roleInfo, Claim claim)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var role = await roleManager.FindByIdAsync(roleInfo.Id.ToString());
            var result = await roleManager.AddClaimAsync(role, claim);
            transaction.Complete();
            return result;
        }       
    }
}
