using Teram.Framework.Core.Logic;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.IncomingGoods.Models
{
    public class ControlPlanModel : ModelBase<ControlPlan, int>
    {
        public int ControlPlanId { get; set; }

        [GridColumn(nameof(ControlPlanCategoryTitle))]
        public string ControlPlanCategoryTitle { get; set; }

        [GridColumn(nameof(ControlPlanParameter))]
        public string ControlPlanParameter {  get; set; }

        [GridColumn(nameof(QuantityDescription))]
        public string QuantityDescription {  get; set; }

        [GridColumn(nameof(AcceptanceCriteria))]
        public string? AcceptanceCriteria {  get; set; }

        public int ControlPlanCategoryId {  get; set; }
        
    }
}
