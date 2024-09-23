using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Teram.HR.Module.Assets.Models.QueryModels
{
    public class AssetQueryModel
    {
        [DisplayName("AssetID")]
        public long AssetID { get; set; }

        [DisplayName("شماره پلاک")]
        public string PlaqueNumber { get; set; }

        [DisplayName("عنوان")]
        public string Title { get; set; }

        [JsonIgnore]
        public DateTime? UtilizeDate { get; set; }

        [DisplayName("تاریخ بهره برداری")]
        public string UtilizationDate { get; set; }

        [DisplayName("گروه")]
        public string GroupTitle { get; set; }

        [DisplayName("روش استهلاک")]
        public string DepreciatedMethodTitle { get; set; }

        [DisplayName("مرکز هزینه")]
        public string CostCenter { get; set; }

        [DisplayName("محل استقرار")]
        public string SettlementPlace { get; set; }

        [DisplayName("جمع دار")]
        public string Collector { get; set; }

        [DisplayName("مسئول")]
        public string FullName { get; set; }

        [DisplayName("بهای تمام شده")]
        public decimal? Cost { get; set; }

        [DisplayName("کد کالا")]
        public string Code { get; set; }

        [DisplayName("کد پرسنلی")]
        public string? PersonnelCode {  get; set; }

        [DisplayName("کد ملی")]
        public string NationalID {  get; set; }
    }
}
