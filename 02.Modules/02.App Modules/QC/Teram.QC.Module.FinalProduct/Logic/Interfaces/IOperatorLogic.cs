using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Models.CausationModels;
using Teram.QC.Module.FinalProduct.Models.ServiceModels;

namespace Teram.QC.Module.FinalProduct.Logic.Interfaces
{
    public interface IOperatorLogic : IBusinessOperations<OperatorModel, Operator, int>
    {
        BusinessOperationResult<List<OperatorModel>> GetActives();
        Task<BusinessOperationResult<List<OperatorModel>>> UpdateEmployeesList(List<EmployeeModel> employees);
        BusinessOperationResult<OperatorModel> GetByNationalId(string nationalId);
        BusinessOperationResult<OperatorModel> GetByUserId(Guid? userId);
    }

}
