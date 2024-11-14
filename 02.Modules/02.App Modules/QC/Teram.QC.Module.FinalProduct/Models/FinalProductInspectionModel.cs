using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models
{
    public class FinalProductInspectionModel : ModelBase<FinalProductInspection, int>
    {
        public int FinalProductInspectionId { get; set; }

        [GridColumn(nameof(PersianCreateDate))]
        public string PersianCreateDate => CreateDate.ToPersianDateTime();


        [GridColumn(nameof(OrderNo))]
        public int OrderNo { get; set; }

        [GridColumn(nameof(ProductCode))]
        public string ProductCode { get; set; }

        [GridColumn(nameof(ProductName))]
        public string ProductName { get; set; }

        public string OrderTitle { get; set; }

        [GridColumn(nameof(Number))]
        public int Number { get; set; }
        public DateTime Date { get; set; }

        [GridColumn(nameof(TracingCode))]
        public string TracingCode { get; set; }

        public string ControlPlan { get; set; }


        public long StartInterval { get; set; }
        public long EndInterval { get; set; }

        public int SampleCount { get; set; }

        [GridColumn(nameof(TotalCount))]
        public long TotalCount { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        public bool HasNonCompliance { get; set; }

        [GridColumn(nameof(HasNonComplianceText))]
        public string HasNonComplianceText => HasNonCompliance ? "رد شده" : "تایید شده";

        [GridColumn(nameof(CreatedByText))]
        public string? CreatedByText { get; set; }
        public string? NonCompianceHistory { get; set; }
        public List<FinalProductInspectionDefectModel> FinalProductInspectionDefects { get; set; }

    }
}
