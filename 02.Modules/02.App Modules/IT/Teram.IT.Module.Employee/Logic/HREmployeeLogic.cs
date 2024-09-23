using DocumentFormat.OpenXml.Spreadsheet;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.IT.Module.Employee.Logic.Interface;
using Teram.IT.Module.Employee.Models;

namespace Teram.IT.Module.Employee.Logic
{   
    public class HREmployeeLogic : BusinessOperations<HREmployeeModel, Entities.HREmployee, int>, IHREmployeeLogic
    {
        public HREmployeeLogic(IPersistenceService<Entities.HREmployee> service) : base(service)
        {

        }

        public async Task<BusinessOperationResult<List<HREmployeeModel>>> UpdateEmployeesList(List<HREmployeeModel> employees)
        {
            var updateResult = await BulkMergeAsync(employees, compareColumnNames: new List<string> { "EmployeeID" });
            return updateResult;
        }
    }

}
