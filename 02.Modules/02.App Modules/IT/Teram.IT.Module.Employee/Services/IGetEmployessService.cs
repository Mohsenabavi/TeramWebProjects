using Teram.Framework.Core.Logic;
using Teram.IT.Module.Employee.Models;

namespace Teram.IT.Module.Employee.Services
{
    public interface IGetEmployessService
    {
        Task<List<HREmployeeModel>> GetAllActiveEmployess();
    }
}
