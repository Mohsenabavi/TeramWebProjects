using Microsoft.AspNetCore.Identity;
using System.Linq;
using Teram.IT.Module.Employee.Logic;
using Teram.IT.Module.Employee.Logic.Interface;
using Teram.ServiceContracts;

namespace Teram.IT.Module.Employee.Services
{
    public class UpdateUsersService : IUpdateUsersService
    {
        private readonly IUserSharedService userSharedService;
        private readonly IHREmployeeLogic hREmployeeLogic;

        public UpdateUsersService(IUserSharedService userSharedService, IHREmployeeLogic hREmployeeLogic)
        {
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
            this.hREmployeeLogic=hREmployeeLogic??throw new ArgumentNullException(nameof(hREmployeeLogic));
        }

        public List<IdentityResult> UpdateUsersList()
        {
            var finalResult = new List<IdentityResult>();

            var currentHRUsers = hREmployeeLogic.GetAll();

            var currentUsers = userSharedService.GetAllUsers();

            var currentHRUsersNationalCodes = currentHRUsers.ResultEntity.Select(x => x.NationalID).ToList();
            var currentUsersUserNames = currentUsers.Select(x => x.Username).ToList();

            var usersNotInCurrentUsers = currentHRUsers.ResultEntity.Where(hrUser => !currentUsersUserNames.Contains(hrUser.NationalID)).ToList();

            foreach (var item in usersNotInCurrentUsers)
            {
                var createUserResult = userSharedService.CreateUserAsync(item.FirstName, item.LastName, item.Mobile, " ", item.NationalID, item.NationalID).Result;

                if (createUserResult.Succeeded)
                {
                    var craetedUser = userSharedService.GetUserInfoByUserName(item.NationalID).Result;
                    if (craetedUser != null)
                    {
                        var addRoleResult = userSharedService.AddToRoleAsync(craetedUser, "Employee").Result;
                        finalResult.Add(addRoleResult);
                    }
                }               
            }
            return finalResult;
        }
    }
}
