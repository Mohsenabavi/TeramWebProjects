using Teram.Framework.Core.Logic;
using Teram.IT.Module.Employee.Entities;
using Teram.IT.Module.Employee.Models;

namespace Teram.IT.Module.Employee.Logic.Interface
{   
    public interface IHREmployeeLogic : IBusinessOperations<HREmployeeModel,Entities.HREmployee, int>
    {
        Task<BusinessOperationResult<List<HREmployeeModel>>> UpdateEmployeesList(List<HREmployeeModel> employees);
    }

}
