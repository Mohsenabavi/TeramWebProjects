using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.WorkFlow;
using Teram.QC.Module.FinalProduct.Enums;
using Teram.ServiceContracts;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models.WorkFlow
{
    public class FlowInstructionModel : ModelBase<FlowInstruction, int>
    {
        [GridColumn(nameof(FlowInstructionId))]
        public int FlowInstructionId { get; set; }

        public ReferralStatus FromStatus { get; set; }

        public ReferralStatus ToStatus { get; set; }

        public Guid CurrentCartableRoleId { get; set; }

        public Guid NextCartableRoleId { get; set; }

        public FormStatus FormStatus { get; set; }

        public virtual List<FlowInstructionConditionModel> FlowInstructionConditions { get; set; }


        #region Helper Fileds

        [GridColumn(nameof(FromStatusText))]
        public string FromStatusText => (FromStatus>0) ? FromStatus.GetDescription() : "";


        [GridColumn(nameof(ToStatusText))]
        public string ToStatusText => (ToStatus>0) ? ToStatus.GetDescription() : "";

        [GridColumn(nameof(FormStatusText))]
        public string FormStatusText => FormStatus.GetDescription() ;

        public List<UserInfo> DestinationUsers { get; set; } = [];
        #endregion

    }
}
