using Teram.Framework.Core.Extensions;

namespace Teram.QC.Module.FinalProduct.Models.ReportsModels
{
    public class WrongDoerReportModel
    {
        public string OrderNu { get; set; }
        public string OrderTitle { get; set; }
        public string DefectTitle { get; set; }
        public string DesfectCode { get; set; }
        public string WorkStation { get; set; }
        public int wrongDoerId { get; set; }
        public int wrongDoerId2 { get; set; }
        public int wrongDoerId3 { get; set; }
        public int wrongDoerId4 { get; set; }
        public string WrongDoers { get; set; }
        public DateTime CreateDate { get; set; }
        public string PersianCreateDate => CreateDate.ToPersianDate();
        public string FinalProductNoncomplianceNumber { get; set; }
    }
}
