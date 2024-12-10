using Teram.Framework.Core.Attributes;
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

        [ExportToExcel("کد ایراد")]
        [GridColumn(nameof(DefectCode))]
        public string DefectCode {  get; set; }

        [ExportToExcel("عنوان ایراد")]
        [GridColumn(nameof(DefectTitle))]
        public string DefectTitle { get; set; }

        [ExportToExcel("طرح کنترلی")]
        [GridColumn(nameof(ControlPlanTitle))]
        public string ControlPlanTitle {  get; set; }

        [ExportToExcel("مقدار")]
        [GridColumn(nameof(ControlPlanDefectVal))]
        public string ControlPlanDefectVal { get; set; }
    }
}
