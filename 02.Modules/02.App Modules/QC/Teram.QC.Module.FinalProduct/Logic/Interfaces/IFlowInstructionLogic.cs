using System.Linq.Expressions;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Entities.WorkFlow;
using Teram.QC.Module.FinalProduct.Enums;
using Teram.QC.Module.FinalProduct.Models;
using Teram.QC.Module.FinalProduct.Models.WorkFlow;

namespace Teram.QC.Module.FinalProduct.Logic.Interfaces
{  
    public interface IFlowInstructionLogic : IBusinessOperations<FlowInstructionModel, FlowInstruction, int>
    {
        BusinessOperationResult<FlowInstructionModel> GetById(int flowInstructionId);
        BusinessOperationResult<FlowInstructionModel> GetByFiler(Guid fromRoleId, FinalProductNoncomplianceModel model);
        BusinessOperationResult<List<FlowInstructionModel>> GetByCurrentCartableRoleId(Guid roleId);
        BusinessOperationResult<List<FlowInstructionModel>> GetGridDataByFiler(ReferralStatus? fromStatus, ReferralStatus? toStatus, FormStatus? formStatus ,int? start = null, int? length = null);
    }

}
