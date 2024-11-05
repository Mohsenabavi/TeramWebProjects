using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models
{
    public class PalletsStatusModel
    {
        [GridColumn(nameof(Number))]
        public string Number { get; set; }
        [GridColumn(nameof(OrderNo))]
        public string OrderNo { get; set; }
        [GridColumn(nameof(ProductName))]
        public string ProductName {  get; set; }

        [GridColumn(nameof(SampleCount))]
        public int SampleCount { get; set; }

        [GridColumn(nameof(TracingCode))]
        public string TracingCode { get; set; }

        [GridColumn(nameof(ProductCode))]
        public string ProductCode { get; set; }

        [GridColumn(nameof(NoncomplianceNumbersSeparated))]
        public string NoncomplianceNumbersSeparated {  get; set; }
        public string NoncomplianceNumbers {  get; set; }

        [GridColumn(nameof(Status))]
        public string Status {  get; set; }
    }
}
