using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities.WorkFlow;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models.WorkFlow;

namespace Teram.QC.Module.FinalProduct.Logic
{
    public class FlowInstructionConditionLogic : BusinessOperations<FlowInstructionConditionModel, FlowInstructionCondition, int>, IFlowInstructionConditionLogic
    {
        public FlowInstructionConditionLogic(IPersistenceService<FlowInstructionCondition> service) : base(service)
        {

        }
    }

}
