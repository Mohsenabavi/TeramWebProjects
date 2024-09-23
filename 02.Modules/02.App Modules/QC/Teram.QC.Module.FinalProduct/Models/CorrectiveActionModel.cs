using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;

namespace Teram.QC.Module.FinalProduct.Models
{
    public class CorrectiveActionModel : ModelBase<CorrectiveAction, int>
    {
        public int CorrectiveActionId { get; set; }
        public int ActionerId {  get; set; }
        public string Descriiption { get; set; }
        public int CausationId { get; set; }
        public DateTime ActionDate {  get; set; }
        public int ApproverId {  get; set; }
        public string ActionDatePersian => ActionDate.ToPersianDate();
    }
}
