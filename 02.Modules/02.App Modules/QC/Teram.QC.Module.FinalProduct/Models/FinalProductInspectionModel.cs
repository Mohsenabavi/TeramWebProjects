using Teram.Framework.Core.Attributes;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models
{
    public class FinalProductInspectionModel : ModelBase<FinalProductInspection, int>
    {
        public int FinalProductInspectionId { get; set; }

        [ExportToExcel("تاریخ ایجاد")]
        [GridColumn(nameof(PersianCreateDate))]
        public string PersianCreateDate => CreateDate.ToPersianDateTime();

        [ExportToExcel("شماره سفارش")]
        [GridColumn(nameof(OrderNo))]
        public int OrderNo { get; set; }

        [ExportToExcel("کد محصول")]
        [GridColumn(nameof(ProductCode))]
        public string ProductCode { get; set; }

        [ExportToExcel("نام محصول")]
        [GridColumn(nameof(ProductName))]
        public string ProductName { get; set; }

        [ExportToExcel("عنوان سفارش")]
        public string OrderTitle { get; set; }

        [ExportToExcel("شماره پالت")]
        [GridColumn(nameof(Number))]
        public int Number { get; set; }
        public DateTime Date { get; set; }

        [ExportToExcel("کد ردیابی")]
        [GridColumn(nameof(TracingCode))]
        public string TracingCode { get; set; }

        public string ControlPlan { get; set; }

        public long StartInterval { get; set; }
        public long EndInterval { get; set; }

        public int SampleCount { get; set; }

        [ExportToExcel("تعداد کل")]
        [GridColumn(nameof(TotalCount))]
        public long TotalCount { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        public bool HasNonCompliance { get; set; }

        [ExportToExcel("وضعیت")]
        [GridColumn(nameof(HasNonComplianceText))]
        public string HasNonComplianceText => HasNonCompliance ? "رد شده" : "تایید شده";

        [ExportToExcel("ایجاد کننده")]
        [GridColumn(nameof(CreatedByText))]
        public string? CreatedByText { get; set; }
        public string? NonCompianceHistory { get; set; }
        public List<FinalProductInspectionDefectModel> FinalProductInspectionDefects { get; set; }

    }
}
