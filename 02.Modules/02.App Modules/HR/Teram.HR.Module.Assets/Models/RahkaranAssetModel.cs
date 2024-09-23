using System.ComponentModel;
using Teram.Framework.Core.Attributes;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Assets.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.Assets.Models
{
    public class RahkaranAssetModel : ModelBase<RahkaranAsset, int>
    {
        public int RahkaranAssetId { get; set; }


        [GridColumn(nameof(NationalID))]
        public string? NationalID { get; set; }

        [GridColumn(nameof(PersonnelCode))]
        [ExportToExcel("کدپرسنلی ")]
        public int? PersonnelCode { get; set; }

        [GridColumn(nameof(FullName))]
        [ExportToExcel("مسئول")]
        public string? FullName { get; set; }
       
        [ExportToExcel("شناسه دارایی")]
        public long AssetID { get; set; }

        [GridColumn(nameof(Code))]
        [ExportToExcel("کد کالا")]
        public string? Code { get; set; }

        [GridColumn(nameof(PlaqueNumber))]
        [ExportToExcel("شماره پلاک ")]
        public string? PlaqueNumber { get; set; }

        [GridColumn(nameof(Title))]
        [ExportToExcel("عنوان ")]
        public string? Title { get; set; }

        public DateTime? UtilizeDate { get; set; }
     
        [ExportToExcel("تاریخ بهره برداری")]
        public string? UtilizationDate { get; set; }


        [GridColumn(nameof(GroupTitle))]
        [ExportToExcel("گروه")]
        public string? GroupTitle { get; set; }
      
        [ExportToExcel("روش استهلاک")]
        public string? DepreciatedMethodTitle { get; set; }

        [GridColumn(nameof(CostCenter))]
        [ExportToExcel("مرکز هزینه")]
        public string? CostCenter { get; set; }

        [GridColumn(nameof(SettlementPlace))]
        [ExportToExcel("محل استقرار")]
        public string? SettlementPlace { get; set; }

        [GridColumn(nameof(Collector))]
        [ExportToExcel("جمع دار")]
        public string? Collector { get; set; }

        [ExportToExcel("بهای تمام شده")]
        public decimal? Cost { get; set; }

        public Guid? ApprovedBy { get; set; }

        public DateTime? ApproveDate { get; set; }

        public bool? ApproveStatus { get; set; }


        [GridColumn(nameof(ApproveStatusText))]
        public string ApproveStatusText => (ApproveStatus.HasValue && ApproveStatus.Value) ? "تایید است" : "تایید نیست";

        public string? ApproverRemarks { get; set; }

      

    }
}
