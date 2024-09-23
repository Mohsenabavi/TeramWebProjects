using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Models;
using Teram.QC.Module.FinalProduct.Models.WorkFlow;

namespace Teram.QC.Module.FinalProduct.Logic.Interfaces
{
    public interface IFinalProductNonComplianceCartableItemLogic : IBusinessOperations<FinalProductNonComplianceCartableItemModel, FinalProductNonComplianceCartableItem, int>
    {        
        BusinessOperationResult<List<FinalProductNonComplianceCartableItemModel>> GetByUserId(Guid userId);

        BusinessOperationResult<List<FinalProductNonComplianceCartableItemModel>> GetByUserIdsAndNonComplianceId(List<Guid> userId, int finalProductNonComplianceId);

        BusinessOperationResult<List<FinalProductNonComplianceCartableItemModel>> GetByUserIdAndFinalProductNonComplianceId(Guid userId, int finalProductNonComplianceId);
        BusinessOperationResult<FinalProductNonComplianceCartableItemModel> GetLastByUserIdAndFinalProductNonComplianceId(Guid userId, int finalProductNonComplianceId);

        BusinessOperationResult<List<FinalProductNonComplianceCartableItemModel>> GetSalesUniCartableItems(int finalProductNonComplianceId);

    }
}
