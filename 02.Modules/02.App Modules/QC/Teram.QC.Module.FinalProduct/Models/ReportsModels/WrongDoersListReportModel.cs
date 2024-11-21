using Teram.Framework.Core.Attributes;
using Teram.Framework.Core.Extensions;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models.ReportsModels
{
    public class WrongDoersListReportModel
    {
        [GridColumn(nameof(OrderNu))]
        [ExportToExcel("شماره سفارش")]
        public string OrderNu { get; set; }
        [GridColumn(nameof(OrderTitle))]
        [ExportToExcel("عنوان سفارش")]
        public string OrderTitle { get; set; }
        [GridColumn(nameof(DefectTitle))]

        [ExportToExcel("ایراد")]
        public string DefectTitle { get; set; }
        [GridColumn(nameof(DefectCode))]
        [ExportToExcel("کد ایراد")]
        public string DefectCode { get; set; }

        [GridColumn(nameof(WorkStation))]
        [ExportToExcel("ایستگاه کاری")]
        public string WorkStation { get; set; }
        public int WrongDoerId { get; set; }
        public int WrongDoerId2 { get; set; }
        public int WrongDoerId3 { get; set; }
        public int WrongDoerId4 { get; set; }

        [GridColumn(nameof(WrongDoers))]
        [ExportToExcel("افراد خاطی")]
        public string WrongDoers { get; set; }

        public DateTime InputDate { get; set; }

        [GridColumn(nameof(PersianInputDate))]
        [ExportToExcel("تاریخ مبنا")]
        public string PersianInputDate => InputDate.ToPersianDate();

        [GridColumn(nameof(FinalProductNoncomplianceNumber))]

        [ExportToExcel("شماره عدم انطباق")]
        public string FinalProductNoncomplianceNumber { get; set; }
    }
}
