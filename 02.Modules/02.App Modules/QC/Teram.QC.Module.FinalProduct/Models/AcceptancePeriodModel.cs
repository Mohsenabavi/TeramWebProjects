using Teram.Framework.Core.Attributes;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models
{
    public class AcceptancePeriodModel:ModelBase<AcceptancePeriod,int>
    {
        public int AcceptancePeriodId { get; set; }
        public int QCControlPlanId {  get; set; }

        [ExportToExcel("ControlPlanTitle")]
        [GridColumn("ControlPlanTitle")]
        public string ControlPlanTitle { get; set; }

        [ExportToExcel("StartInterval")]
        [GridColumn(nameof(StartInterval))]
        public long StartInterval {  get; set; }

        [ExportToExcel("EndInterval")]
        [GridColumn(nameof(EndInterval))]
        public long EndInterval { get; set; }

        [ExportToExcel("SampleCount")]
        [GridColumn(nameof(SampleCount))]
        public int SampleCount { get; set; }

        [ExportToExcel("A")]
        [GridColumn(nameof(A))]
        public string A {  get; set; }

        [ExportToExcel("Total")]
        [GridColumn(nameof(Total))]
        public string Total { get; set; }

    }
}
