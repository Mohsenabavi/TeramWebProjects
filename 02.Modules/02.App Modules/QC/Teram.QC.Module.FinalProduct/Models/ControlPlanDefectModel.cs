using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models
{
    public class ControlPlanDefectModel :ModelBase<ControlPlanDefect,int>
    {
        public int ControlPlanDefectId { get; set; }

        public int QCControlPlanId { get; set; }

        public int QCDefectId { get; set; }


        [GridColumn(nameof(DefectCode))]
        public string DefectCode {  get; set; }
        
        [GridColumn(nameof(DefectTitle))]
        public string DefectTitle { get; set; }

        [GridColumn(nameof(ControlPlanTitle))]
        public string ControlPlanTitle {  get; set; }

        [GridColumn(nameof(ControlPlanDefectVal))]
        public string ControlPlanDefectVal { get; set; }
    }
}
