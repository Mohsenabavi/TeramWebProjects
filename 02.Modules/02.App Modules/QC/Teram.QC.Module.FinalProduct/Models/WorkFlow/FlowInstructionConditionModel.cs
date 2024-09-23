using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.WorkFlow;

namespace Teram.QC.Module.FinalProduct.Models.WorkFlow
{
    public class FlowInstructionConditionModel : ModelBase<FlowInstructionCondition, int>
    {
        public int FlowInstructionConditionId { get; set; }       
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public int FlowInstructionId { get; set; }

    }
}
