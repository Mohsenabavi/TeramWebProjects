using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;

namespace Teram.QC.Module.FinalProduct.Models
{
    public class FinalProductNonComplianceCartableItemModel : ModelBase<FinalProductNonComplianceCartableItem, int>
    {
        public int FinalProductNonComplianceCartableItemId { get; set; }

        public Guid UserId { get; set; }
        public string Comments { get; set; }

        public DateTime InputDate { get; set; }

        public DateTime? OutputDate { get; set; }

        public Guid ReferredBy { get; set; }

        public string? SourceUserName { get; set; }
        public string? DestinationUserName {  get; set; } 

        public string InputDatePersian => InputDate.ToPersianDateTime();

        public string OutputDatePersian => (OutputDate!=null) ? OutputDate.Value.ToPersianDateTime() : "-";

        public int FinalProductNoncomplianceId { get; set; }
    }
}
