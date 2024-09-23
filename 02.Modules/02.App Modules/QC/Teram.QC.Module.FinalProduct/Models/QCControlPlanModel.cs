using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models
{
    public class QCControlPlanModel : ModelBase<QCControlPlan, int>
    {
        public int QCControlPlanId { get; set; }

        [GridColumn(nameof(Title))]
        public string Title { get; set; }

        public bool IsActive { get; set; }

        [GridColumn(nameof(IsActiveText))]
        public string IsActiveText => IsActive ? "فعال" : "غیر فعال";        
    }
}
