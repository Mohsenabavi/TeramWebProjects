using Microsoft.AspNetCore.Identity;
using Teram.ServiceContracts;

namespace Teram.IT.Module.Employee.Services
{
    public interface IUpdateUsersService
    {
        List<IdentityResult> UpdateUsersList();
    }
}
