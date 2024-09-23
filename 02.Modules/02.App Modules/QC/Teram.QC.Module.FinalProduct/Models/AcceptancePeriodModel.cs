using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models
{
    public class AcceptancePeriodModel:ModelBase<AcceptancePeriod,int>
    {
        public int AcceptancePeriodId { get; set; }
        public int QCControlPlanId {  get; set; }

        [GridColumn("ControlPlanTitle")]
        public string ControlPlanTitle { get; set; }


        [GridColumn(nameof(StartInterval))]
        public long StartInterval {  get; set; }

        [GridColumn(nameof(EndInterval))]
        public long EndInterval { get; set; }

        [GridColumn(nameof(SampleCount))]
        public int SampleCount { get; set; }

        [GridColumn(nameof(A))]
        public string A {  get; set; }

        [GridColumn(nameof(Total))]
        public string Total { get; set; }


      
    }
}
