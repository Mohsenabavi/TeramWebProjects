using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Models.WorkFlow;
using Teram.QC.Module.FinalProduct.Models;
using Teram.ServiceContracts;

namespace Teram.QC.Module.FinalProduct.Logic.Interfaces
{
    public interface IManageCartableLogic
    {
        BusinessOperationResult<RoleInfo> GetUserMainRole(string? forceRole = null);
        BusinessOperationResult<RoleInfo> GetUserMainRole(Guid userId);
        BusinessOperationResult<List<FinalProductNonComplianceCartableItemModel>> AddToCartable(FlowInstructionModel flowInstructionModel, FinalProductNoncomplianceModel finalProductNoncomplianceModel, List<int>? sampleIds = null, Guid? destinationUserId = null);
        BusinessOperationResult<FlowInstructionModel> GetNextStep(FinalProductNoncomplianceModel model);
        BusinessOperationResult<FinalProductNonComplianceCartableItemModel> GetLastCauseFinder(List<FinalProductNonComplianceCartableItemModel> cartableItems);
    }
}
