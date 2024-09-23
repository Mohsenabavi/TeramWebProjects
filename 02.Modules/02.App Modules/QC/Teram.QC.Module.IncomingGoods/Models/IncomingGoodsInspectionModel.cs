using DocumentFormat.OpenXml.Office2013.Excel;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Enums;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.IncomingGoods.Models
{
    public class IncomingGoodsInspectionModel : ModelBase<IncomingGoodsInspection, int>
    {

        #region Database Fields

        public int IncomingGoodsInspectionId { get; set; }

        [GridColumn(nameof(QualityInspectionNumber))]
        public string QualityInspectionNumber { get; set; }

        [GridColumn(nameof(GoodsTitle))]
        public string GoodsTitle { get; set; }

        public decimal? Quantity {  get; set; }

       [GridColumn(nameof(GoodsCode))]
        public string GoodsCode { get; set; }

        [GridColumn(nameof(VendorName))]
        public string VendorName { get; set; }
        public bool IsConsignment { get; set; }
        public bool IsSampleGoods { get; set; }
        public bool GoodsInfoIsApproved { get; set; }
        public bool HasFunctionalTest { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid CreatedBy { get; set; }
        public InspectionFormStatus InspectionFormStatus { get; set; }

        [GridColumn(nameof(InspectionFormStatusText))]
        public string InspectionFormStatusText => InspectionFormStatus.GetDescription();

        public int TotalDefectiveCount { get; set; }
        public bool? FinalApprove { get; set; }
        #endregion

        #region Calculated Fields

        [GridColumn(nameof(PersianCreateDate))]
        public string PersianCreateDate => CreateDate.ToPersianDate();

        [GridColumn(nameof(HasFunctionalTestText))]
        public string HasFunctionalTestText => HasFunctionalTest ? "دارای تست کاربردی" : "فاقد تست کاربردی";

        
        public string GoodsInfoIsApprovedText => GoodsInfoIsApproved ? "تایید است" : "تایید نیست";

        [GridColumn(nameof(IsSampleGoodsText))]
        public string IsSampleGoodsText => IsSampleGoods ? "sample می باشد" : "sample نمی باشد";

        [GridColumn(nameof(IsConsignmentText))]
        public string IsConsignmentText => IsConsignment ? "امانی" : "غیر امانی";

        public bool NeedToRefferal { get; set; }



        [GridColumn(nameof(FinalApproveText))]
        public string FinalApproveText => (FinalApprove.HasValue && FinalApprove.Value) ? "تایید نهایی" : (FinalApprove.HasValue && !FinalApprove.Value) ? "رد نهایی" : "ثبت نشده";
       
        #endregion


        #region Helper Fields

        public string CreatedByUserName { get; set; }

        public string Comments { get; set; }
        public bool IsApproved { get; set; }

        public Guid? ReferralUserId { get; set; }

        public bool HasPermissionForInsertForm { get; set; }

        #endregion

        #region Navigations

        public List<IncomingGoodsInspectionItemModel>? IncomingGoodsInspectionItems { get; set; }
        public List<IncomingGoodsInspectionFileModel>? IncomingGoodsInspectionFiles { get; set; }
        public List<IncomingGoodsInspectionCartableItemModel>? IncomingGoodsInspectionCartableItems { get; set; }

        #endregion


    }
}
