using Teram.ServiceContracts;
using Teram.Module.Authentication.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Teram.Web.Core.Exceptions;
using Microsoft.Extensions.Localization;
using Teram.Web.Core;
using System.Security.Claims;
using System.Transactions;

namespace Teram.Module.Authentication.Service
{
    public class UserService : IUserSharedService
    {
        private readonly UserManager<TeramUser> _userManager;
        private readonly RoleManager<TeramRole> _roleManager;
        private readonly SignInManager<TeramUser> _signInManager;
        private IStringLocalizer<SharedResource> localizer;
        private IStringLocalizer<AuthenticationSharedResource> authenticationlocalizer;

        public UserService(UserManager<TeramUser> userManager, SignInManager<TeramUser> signInManager, IStringLocalizer<SharedResource> localizer, IStringLocalizer<AuthenticationSharedResource> authenticationlocalizer, RoleManager<TeramRole> roleManager)
        {
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            this.authenticationlocalizer = authenticationlocalizer ?? throw new ArgumentNullException(nameof(authenticationlocalizer));
            userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        /// <summary>
        /// تغییر پسورد
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<IdentityResult> ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            transaction.Complete();
            return result;
        }

        /// <summary>
        /// ایجاد یوزر
        /// </summary>
        /// <param name="name"></param>
        /// <param name="family"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateUserAsync(string name, string family, string phoneNumber, string email, string userName, string password)
        {
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var TeramUser = new TeramUser() { ConcurrencyStamp = Guid.NewGuid().ToString() };
            TeramUser.Email = email;
            TeramUser.PhoneNumber = phoneNumber;
            TeramUser.UserName = userName;
            TeramUser.EmailConfirmed = true;
            TeramUser.PhoneNumberConfirmed = true;
            TeramUser.Name = $"{name} {family}";
            TeramUser.CreatedOn= DateTime.Now;
            var result = await _userManager.CreateAsync(TeramUser, password);
            transaction.Complete();
            return result;
        }

        /// <summary>
        /// گرفتن آیدی کاربر با استفاده از ایمیل
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Guid? GetByEmail(string email)
        {
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = _userManager.Users.FirstOrDefault(x => x.Email == email);
            transaction.Complete();
            return user?.Id;
        }

        /// <summary>
        /// گرفتن اطلاعات یوزر با استفاده از ایمیل
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public UserInfo GetInfoByEmail(string email)
        {
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = _userManager.Users.FirstOrDefault(x => x.Email == email);
            var userInfo = new UserInfo
            {
                Username = user.UserName,
                Email = user.Email,
                UserId = user.Id,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Name = user.Name,
                CreatedOn = user.CreatedOn,
            };
            transaction.Complete();
            return userInfo;
        }

        /// <summary>
        /// گرفتن مشخصات یوزر با استفاده از آیدی
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetUserById(Guid userId)
        {
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
            {
                throw new UIException(localizer["User not found"]);
            }
            var userInfo = new UserInfo
            {
                Username = user.UserName,
                Email = user.Email,
                UserId = user.Id,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Name = user.Name,
                CreatedOn = user.CreatedOn,

            };
            transaction.Complete();
            return userInfo;
        }

        /// <summary>
        /// گرفتن لیست آیدی یوزرها بر اساس جستجو در نام کاربری یا ایمیل
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<Guid> GetUserId(string search)
        {
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var result = _userManager.Users.Where(
                x => x.Name.Contains(search) ||
                x.UserName.Contains(search) ||
                x.Email.Contains(search) ||
                x.PhoneNumber.Contains(search) ||
                x.NormalizedUserName.Contains(search))
                .Select(x => x.Id).ToList();
            transaction.Complete();
            return result;
        }

        /// <summary>
        /// لیست مشخصات کاربران بر اساس  نام کاربری یا ایمیل
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<UserInfo> GetUserInfo(string search)
        {
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var result = _userManager.Users.Where(
                x =>
            x.Name.Contains(search) ||
            x.UserName.Contains(search) ||
            x.NormalizedUserName.Contains(search) ||
            x.PhoneNumber.Contains(search) ||
            x.Email.Contains(search))
               .Select(x => new UserInfo
               {
                   Email = x.Email,
                   Username = x.UserName,
                   UserId = x.Id,
                   PhoneNumber = x.PhoneNumber,
                   EmailConfirmed = x.EmailConfirmed,
                   PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                   Name = x.Name,
                   CreatedOn = x.CreatedOn,
               }).ToList();
            transaction.Complete();
            return result;
        }

        /// <summary>
        /// گرفتن مشخصات یوزر بر اساس لیستی از یوزر آیدی ها
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public List<UserInfo> GetUserInfos(List<Guid> userIds)
        {
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var result = _userManager.Users
                            .Where(x => userIds.Contains(x.Id)).Select(x => new UserInfo
                            {
                                Email = x.Email,
                                Username = x.UserName,
                                UserId = x.Id,
                                PhoneNumber = x.PhoneNumber,
                                EmailConfirmed = x.EmailConfirmed,
                                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                                Name = x.Name,
                                CreatedOn = x.CreatedOn,
                            }).ToList();
            transaction.Complete();
            return result;

        }

        /// <summary>
        /// ورود به سیستم
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<ResultModel> SignIn(string userName, string password)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var result = await _signInManager.PasswordSignInAsync(userName, password, true, lockoutOnFailure: true);

            var user = await _userManager.FindByNameAsync(userName);
            if (!result.Succeeded)
            {
                if (result.RequiresTwoFactor)
                {
                    return new ResultModel { Succeeded = result.Succeeded, Result = "invalid", Title = localizer["InvalidLoginAttempt"], Message = localizer["2FA required"] };

                }
                if (result.IsLockedOut)
                {
                    return new ResultModel { Succeeded = result.Succeeded, Result = "invalid", Title = localizer["InvalidLoginAttempt"], Message = localizer["Account is locked out"] };
                }
                if (result.IsNotAllowed)
                {
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        return new ResultModel { Succeeded = result.Succeeded, Result = "invalid", Title = localizer["InvalidLoginAttempt"], Message = localizer["Error confirming your email."] };
                    }

                    if (!await _userManager.IsPhoneNumberConfirmedAsync(user))
                    {
                        return new ResultModel { Succeeded = result.Succeeded, Result = "invalid", Title = localizer["InvalidLoginAttempt"], Message = authenticationlocalizer["Error confirming your phone number."] };
                    }
                }

                return new ResultModel { Succeeded = false, Result = "invalid", Title = localizer["Login"], Message = authenticationlocalizer["Username or password is incorrect"] };

            }
            transaction.Complete();

            return new ResultModel { Succeeded = result.Succeeded, Result = "ok", Title = localizer["Login"], Message = localizer["Ok"] };

        }

        /// <summary>
        /// خروج از سیستم
        /// </summary>
        public void SignOut()
        {
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            _signInManager.SignOutAsync();
            transaction.Complete();
        }

        public async Task<IdentityResult> DeleteUser(Guid userId)
        {
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.DeleteAsync(user);
            transaction.Complete();
            return result;
        }

        public async Task<IdentityResult> UpdateUser(UserInfo model)
        {
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var TeramUser = await _userManager.FindByIdAsync(model.UserId.ToString());
            TeramUser.PhoneNumber = model.PhoneNumber;
            TeramUser.EmailConfirmed = model.EmailConfirmed;
            TeramUser.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
            TeramUser.Email = model.Email;
            TeramUser.UserName = model.Username;
            TeramUser.Name = model.Name;
            var result = await _userManager.UpdateAsync(TeramUser);
            transaction.Complete();
            return result;
        }

        public async Task<List<Guid>> GetRoleIdsUser(Guid userId)
        {
            //using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new UIException(localizer["User not found"]);
            }

            var roles = await _userManager.GetRolesAsync(user);


            List<Guid> roldIds = new List<Guid>();

            foreach (var role in roles)
            {
                roldIds.Add(_roleManager.FindByNameAsync(role).Result.Id);
            }
            //transaction.Complete();
            return roldIds;
        }

        public async Task<List<UserInfo>> GetUsersInRole(string roleName)
        {
            //using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = await _userManager.GetUsersInRoleAsync(roleName);
            var result = user.Select(x => new UserInfo
            {
                Username = x.UserName,
                UserId = x.Id,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                EmailConfirmed = x.EmailConfirmed,
                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                Name = x.Name,
                CreatedOn = x.CreatedOn,

            }).ToList();    
            //transaction.Complete(); 
            return result;
        }

        public async Task<List<UserInfo>> GetUsersInRoleByList(List<string> roleNames)
        {
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var users = new List<TeramUser>();

            foreach (var item in roleNames)
            {
                users.AddRange(await _userManager.GetUsersInRoleAsync(item));
            }

            var result = users.Select(x => new UserInfo
            {
                Username = x.UserName,
                UserId = x.Id,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                EmailConfirmed = x.EmailConfirmed,
                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                Name = x.Name,
                CreatedOn = x.CreatedOn,
            }).ToList();
            transaction.Complete();
            return result;
        }

        public async Task<List<UserInfo>> GetUsers(List<Guid> userIds)
        {
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var query = _userManager.Users.Where(t => userIds.Contains(t.Id));

            var userInfos = new List<UserInfo>();

            var result = query.Select(x => new UserInfo
            {
                Username = x.UserName,
                UserId = x.Id,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                EmailConfirmed = x.EmailConfirmed,
                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                Name = x.Name,
                CreatedOn = x.CreatedOn,

            }).ToList();
            transaction.Complete();
            return result;
        }

        public async Task<List<UserRole>> GetRolesUsers(List<Guid> userIds)
        {
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            List<UserRole> userInfos = new List<UserRole>();
            foreach (var userId in userIds)
            {
                string roleNames = "";
                var user = await GetUserInfo(userId);

                var roles = await _userManager.GetRolesAsync(user);

                foreach (var role in roles)
                {
                    var title = _roleManager.FindByNameAsync(role).Result.Title;
                    if (!string.IsNullOrEmpty(title))
                        roleNames += title + "، ";
                }
                userInfos.Add(new UserRole { UserId = user.Id, UserName = user.UserName, RoleName = roleNames.Length > 1 ? roleNames.Remove(roleNames.Length - 2) : "" });
            }
            transaction.Complete();
            return userInfos;
        }

        public async Task<IdentityResult> AddToRoleAsync(UserInfo userInfo, string roleName)
        {
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = await GetUserInfo(userInfo.UserId);
            var result = await _userManager.AddToRoleAsync(user, roleName);
            transaction.Complete();
            return result;
        }

        public async Task<IdentityResult> AddToRoleAsync(UserInfo userInfo, List<string> roleNames)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            IdentityResult result = null;
            var user = await GetUserInfo(userInfo.UserId);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var roleName in roleNames)
                {
                    var isInRole = await _userManager.IsInRoleAsync(user, roleName);
                    if (!isInRole)
                    {
                        result = await _userManager.AddToRoleAsync(user, roleName);
                        if (!result.Succeeded)
                        {
                            var message = string.Join(Environment.NewLine, result.Errors.Select(x => x.Description));
                            throw new UIException(message);
                        }
                    }
                }
                scope.Complete();
            }
            transaction.Complete();
            return result;
        }

        public async Task<IdentityResult> AddToRoleAsync(UserInfo userInfo, List<RoleInfo> roles)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var roleNames = roles.Select(x => x.Name).ToList();
            var result = await AddToRoleAsync(userInfo, roleNames);
            transaction.Complete();
            return result;
        }

        public async Task<IdentityResult> AddClaimAsync(UserInfo userInfo, List<Claim> claims)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            IdentityResult result = null;
            var user = await GetUserInfo(userInfo.UserId);
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var claim in claims)
                {
                    result = await _userManager.AddClaimAsync(user, claim);
                    if (!result.Succeeded)
                    {
                        var message = string.Join(Environment.NewLine, result.Errors.Select(x => x.Description));
                        throw new UIException(message);
                    }
                }
                scope.Complete();
            }
            transaction.Complete();
            return result;
        }

        public async Task<IdentityResult> AddClaimAsync(UserInfo userInfo, Claim claims)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = await GetUserInfo(userInfo.UserId);
            var result = await _userManager.AddClaimAsync(user, claims);
            transaction.Complete();
            return result;
        }

        public async Task<List<Claim>> GetClaimsAsync(UserInfo userInfo)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = await GetUserInfo(userInfo.UserId);
            var result = await _userManager.GetClaimsAsync(user);
            transaction.Complete();
            return (List<Claim>)result;
        }

        public async Task<IdentityResult> RemoveClaimAsync(UserInfo userInfo, Claim claim)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = await GetUserInfo(userInfo.UserId);
            var result = await _userManager.RemoveClaimAsync(user, claim);
            transaction.Complete();
            return result;
        }

        public async Task<IdentityResult> RemoveClaimAsync(UserInfo userInfo, List<Claim> claims)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = await GetUserInfo(userInfo.UserId);
            var result = await _userManager.RemoveClaimsAsync(user, claims);
            transaction.Complete();
            return result;
        }

        public async Task<IdentityResult> RemoveUserRoles(UserInfo userInfo)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = await GetUserInfo(userInfo.UserId);
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
            transaction.Complete();
            return result;
        }

        public async Task<IdentityResult> RemoveUserRoles(UserInfo userInfo, List<string> roleNames)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = await GetUserInfo(userInfo.UserId);
            var result = await _userManager.RemoveFromRolesAsync(user, roleNames);
            transaction.Complete();
            return result;
        }

        public async Task<bool> IsInRoleAsync(UserInfo userInfo, string roleNames)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = await GetUserInfo(userInfo.UserId);
            var result = await _userManager.IsInRoleAsync(user, roleNames);
            transaction.Complete();
            return result;
        }

        public async Task<List<string>> GetRolesOfUser(UserInfo userInfo)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = await GetUserInfo(userInfo.UserId);
            var result = await _userManager.GetRolesAsync(user);
            transaction.Complete();
            return (List<string>)result;
        }

        public async Task<List<Claim>> GetClaimsAsync(Guid userId)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = await GetUserInfo(userId);
            var result = await _userManager.GetClaimsAsync(user);
            transaction.Complete();
            return (List<Claim>)result;
        }

        public async Task<List<string>> GetRolesOfUser(Guid userId)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = await GetUserInfo(userId);
            var result = await _userManager.GetRolesAsync(user);
            transaction.Complete();
            return (List<string>)result;
        }


        public async Task<TeramUser> GetUserInfo(Guid userId)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new UIException(localizer["User not found"]);
            }
            transaction.Complete();
            return user;
        }

        public async Task<List<ServiceContracts.UserRoleModel>> GetRolesOfUsers(List<Guid> userIds)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var userInfos = new List<ServiceContracts.UserRoleModel>();
            foreach (var userId in userIds)
            {
                var user = await GetUserInfo(userId);
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    var roleModel = _roleManager.FindByNameAsync(role).Result;
                    if (roleModel is not null)
                        userInfos.Add(new ServiceContracts.UserRoleModel { UserId = user.Id, UserName = user.UserName, RoleName = roleModel.Name, RoleId = roleModel.Id });
                }
            }
            transaction.Complete();
            return userInfos;
        }



        public List<UserInfo> GetAllUsers()
        {
            var users = _userManager.Users.Select(x => new UserInfo
            {
                Username = x.UserName,
                UserId = x.Id,
                PhoneNumber = x.PhoneNumber,
                Email = x.Email,
                EmailConfirmed = x.EmailConfirmed,
                PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                Name = x.Name,
                CreatedOn = x.CreatedOn,

            }).ToList();
            return users;
        }

        public List<UserInfo> GetUsers(string search, int? start = 0, int? length = 10)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var result = _userManager.Users.Where(
                 x =>
             x.Name.Contains(search) ||
             x.UserName.Contains(search) ||
             x.NormalizedUserName.Contains(search) ||
             x.PhoneNumber.Contains(search) ||
             x.Email.Contains(search))
                .Select(x => new UserInfo
                {
                    Email = x.Email,
                    Name = x.Name,
                    Username = x.UserName,
                    UserId = x.Id,
                    PhoneNumber = x.PhoneNumber,
                    CreatedOn = x.CreatedOn,
                    EmailConfirmed = x.EmailConfirmed,
                    PhoneNumberConfirmed = x.PhoneNumberConfirmed,
                }).Skip(start.Value).Take(length.Value).ToList();
            transaction.Complete();
            return result;
        }

        public async Task<IdentityResult> CreateUserAsync(UserInfo userInfo, string password)
        {

            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var TeramUser = new TeramUser() { ConcurrencyStamp = Guid.NewGuid().ToString() };
            TeramUser.Email = userInfo.Email;
            TeramUser.PhoneNumber = userInfo.PhoneNumber;
            TeramUser.UserName = userInfo.Username;
            TeramUser.EmailConfirmed = true;
            TeramUser.PhoneNumberConfirmed = true;
            TeramUser.Name = userInfo.Name;
            //TeramUser.LastName = userInfo.Family;
            var result = await _userManager.CreateAsync(TeramUser, password);
            transaction.Complete();
            return result;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(UserInfo userInfo)
        {
            var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = await _userManager.FindByIdAsync(userInfo.UserId.ToString());
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            transaction.Complete();
            return token;
        }

        public string GetByPhoneNumber(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task<UserResult> CreateUserInfoAsync(string name, string family, string phoneNumber, string email, string userName, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<UserInfo> GetUserInfoByUserName(string userName)
        {
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == userName);
            if (user is null)
            {
                return null;
            }
            var userInfo = new UserInfo
            {
                Username = user.UserName,
                Email = user.Email,
                UserId = user.Id,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Name = user.Name,
                CreatedOn = user.CreatedOn,
            };
            transaction.Complete();
            return userInfo;
        }

        public Task<List<UserInfo>> GetUsers(List<Guid> userIds, int? start = 0, int? length = 10)
        {
            throw new NotImplementedException();
        }

        public Task<UserInfo> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<UserInfo> GetUserInfoByPhoneNumber(string phoneNumber)
        {
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            var user = _userManager.Users.FirstOrDefault(x => x.PhoneNumber == phoneNumber);
            if (user is null)
            {
                return null;
            }
            var userInfo = new UserInfo
            {
                Username = user.UserName,
                Email = user.Email,
                UserId = user.Id,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Name = user.Name,
                CreatedOn = user.CreatedOn,

            };
            transaction.Complete();
            return userInfo;
        }

        public Task<IdentityResult> ReplaceClaimAsync(UserInfo userInfo, Claim claim, Claim newClaim)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateActiveDirectoryUser(string personCode)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> ChangePasswordAsync(Guid userId, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> AddPasswordAsync(Guid userId, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<List<Guid>> GetCurrentUserRoleIdsFromClaims()
        {
            throw new NotImplementedException();
        }

        public Task<List<UserRole>> GetCurrentUserRolesFromClaims()
        {
            throw new NotImplementedException();
        }

        public Task<List<UserInfo>> GetUsersByClaims(string claimType)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserInfo>> GetUsersByClaims(string claimType, string claimValue)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserInfo>> SearchUserWithClaim(string search, string claimType, string claimValue)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserInfo>> SearchUserWithClaim(string search, string claimType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasClaim(Guid userId, string claimeType)
        {
            throw new NotImplementedException();
        }

        public Task<List<IdentityUserClaim<Guid>>> GetUsersClaims(List<Guid> userIds, List<string> claimTypes)
        {
            throw new NotImplementedException();
        }

        public List<UserInfo> GetUnverifiedPhoneNumberUsers(string search)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserInfo>> GetUnverifiedPhoneNumberUsers(string search, List<string> claimTypes)
        {
            throw new NotImplementedException();
        }
    }
}
