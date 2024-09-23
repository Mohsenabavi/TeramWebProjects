namespace Teram.QC.Module.FinalProduct.Models.ServiceModels
{
    public class PalletInfoModel
    {
        public int OrderNo { get; set; }

        public string OrderTitle { get; set; } = string.Empty;

        public int Number { get; set; }

        public DateTime Date { get; set; }

        public string ProductCode { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public long Quantity { get; set; }

        public string ControlPlan { get; set; } = string.Empty;
        public long StartInterval { get; set; }
        public long EndInterval { get; set; }

        public int SampleCount { get; set; }

        public long TotalCount { get; set; }
    }
}
