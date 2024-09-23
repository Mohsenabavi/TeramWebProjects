using Teram.Framework.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models.ImportModels
{
    public class AcceptancePeriodImportModel
    {
        [ImportFromExcel(ColumnIndex = 1)]
        public string ControlPlanTitle { get; set; }

        [ImportFromExcel(ColumnIndex = 2)]
        public long StartInterval { get; set; }

        [ImportFromExcel(ColumnIndex = 3)]
        public long EndInterval { get; set; }

        [ImportFromExcel(ColumnIndex = 4)]
        public int SampleCount { get; set; }

        [ImportFromExcel(ColumnIndex = 5)]
        public string A { get; set; }

        [ImportFromExcel(ColumnIndex = 6)]
        public string Total { get; set; }
    }
}
