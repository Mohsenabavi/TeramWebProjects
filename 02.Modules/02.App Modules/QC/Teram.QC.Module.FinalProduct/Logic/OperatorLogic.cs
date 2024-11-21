using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models.CausationModels;
using Teram.QC.Module.FinalProduct.Models.ServiceModels;

namespace Teram.QC.Module.FinalProduct.Logic
{
    public class OperatorLogic : BusinessOperations<OperatorModel, Operator, int>, IOperatorLogic
    {
        public OperatorLogic(IPersistenceService<Operator> service) : base(service)
        {

        }

        public BusinessOperationResult<List<OperatorModel>> GetActives()
        {
            return GetData<OperatorModel>(x => x.IsActive);
        }

        public BusinessOperationResult<OperatorModel> GetByNationalId(string nationalId)
        {
            return GetFirst<OperatorModel>(x => x.NationalID==nationalId);
        }

        public BusinessOperationResult<OperatorModel> GetByUserId(Guid? userId)
        {
            return GetFirst<OperatorModel>(x => x.UserId.HasValue&&x.UserId.Value==userId);
        }

        public BusinessOperationResult<List<OperatorModel>> GetByWrongdoerIds(List<int> WrongDoerIds)
        {
            return GetData<OperatorModel>(x => WrongDoerIds.Contains(x.OperatorId));
        }

        public async Task<BusinessOperationResult<List<OperatorModel>>> UpdateEmployeesList(List<EmployeeModel> employees)
        {
            var operatorModel = employees.Select(x => new OperatorModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                MobileNumber=x.Mobile,
                PersonnelCode=int.Parse(x.Code).ToString(),
                EmployeeID=x.EmployeeID,
                IsActive=true,
                NationalID=x.NationalID
            }).ToList();

            var updateResult = await BulkMergeAsync(operatorModel, compareColumnNames: new List<string> { "EmployeeID" }, updateColumnNames: new List<string> { "FirstName", "LastName", "MobileNumber", "PersonnelCode", "EmployeeID", "NationalID" });
            return updateResult;
        }
    }

}
