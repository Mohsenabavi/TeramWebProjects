using Microsoft.AspNetCore.Mvc.Rendering;
using Teram.Framework.Core.Attributes;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Enums;
using Teram.QC.Module.FinalProduct.Models.CausationModels;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models
{
    public class FinalProductNoncomplianceModel : ModelBase<FinalProductNoncompliance, int>
    {
        public int FinalProductNoncomplianceId { get; set; }


        [GridColumn(nameof(PersianCreateDate))]
        [ExportToExcel("تاریخ ایجاد")]
        public string PersianCreateDate => CreateDate.ToPersianDateTime();


        [GridColumn(nameof(FinalProductNoncomplianceNumber))]
        [ExportToExcel("شماره عدم انطباق")]
        public string FinalProductNoncomplianceNumber { get; set; }

        [GridColumn(nameof(OrderNo))]
        [ExportToExcel("شماره سفارش")]
        public int OrderNo { get; set; }

        public string OrderTitle { get; set; }

        [GridColumn(nameof(ProductCode))]
        [ExportToExcel("کد محصول")]
        public string ProductCode { get; set; }


        [GridColumn(nameof(ProductName))]
        [ExportToExcel("نام محصول")]
        public string ProductName { get; set; }


        [GridColumn(nameof(ControlPlanDefectTitle))]
        [ExportToExcel("نام ایراد")]
        public string? ControlPlanDefectTitle { get; set; }
        public Guid? ControlPlanDefectUserId { get; set; }
        public string ControlPlan { get; set; }

        [GridColumn(nameof(ReferralStatusText))]
        [ExportToExcel("وضعیت ارجاع")]
        public string ReferralStatusText => (ReferralStatus > 0) ? ReferralStatus.GetDescription() : "";

        [GridColumn(nameof(FormStatusText))]
        [ExportToExcel("وضعیت")]
        public string FormStatusText => FormStatus.GetDescription();

        public int ControlPlanDefectId { get; set; }

        public string? Remarks { get; set; }

        public int FirstSample { get; set; }

        public int SecondSample { get; set; }

        public int ThirdSample { get; set; }

        public int ForthSample { get; set; }

        public FinalProductNoncomplianceType FinalProductNoncomplianceType { get; set; }

        public string FinalProductNoncomplianceTypeText => (FinalProductNoncomplianceType > 0) ? FinalProductNoncomplianceType.GetDescription() : "-";

        public DateTime CreateDate { get; set; }

        public Guid CreatedBy { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsVoided { get; set; }
        public bool? NeedToAdvisoryOpinion { get; set; }

        public bool? NeedToRefferToCEO { get; set; }

        public bool? HasSeperationOrder { get; set; }

        public bool? IsSeperated { get; set; }

        public bool? NeedToCkeckByOther { get; set; }

        public bool? HasFinalResult { get; set; }

        public bool? HasWasteOrder { get; set; }

        public bool? HasLeniency { get; set; }

        public bool HasCausation { get; set; }

        public QualityControlManagerOpinion QualityControlManagerOpinion { get; set; }

        public ReferralStatus ReferralStatus { get; set; }

        public FormStatus FormStatus { get; set; }

        public DateTime? LastModifyDate { get; set; }

        public string? LastComment { get; set; }

        #region Helper Fields

        public string? ControlPlanDefectValue { get; set; }

        public string? ControlPlanDefectCode { get; set; }
        public int NewFirstSample { get; set; }

        public int NewSecondSample { get; set; }

        public int NewThirdSample { get; set; }

        public int NewForthSample { get; set; }
        public int Number { get; set; }

        public List<SelectListItem>? NonCompliancesList { get; set; }

        public List<FinalProductNoncomplianceDetailModel>? FinalProductNoncomplianceDetails { get; set; }
        public List<FinalProductNoncomplianceFileModel>? FinalProductNoncomplianceFiles { get; set; }
        public List<FinalProductNonComplianceCartableItemModel>? FinalProductNonComplianceCartableItems { get; set; }
        public List<FinalProductNonComplianceCartableItemModel>? SalesUnitCartableItems { get; set; }

        public string? ErrorMessage { get; set; }

        public bool IsEmpty { get; set; }
        public bool IsNew { get; set; }
        public int? FinalProductInspectionId { get; set; }


        public long TotalCount { get; set; }

        public long PalletCount { get; set; }


        public long ReturnedCount { get; set; }

        public long ReturnedPalletsCount { get; set; }

        public decimal? ReturnedNonCompliancePercent { get; set; }

        public decimal? OrderNonCompliancePercent { get; set; }

        public NoncomplianceDetailSampleSummaryModel? NoncomplianceDetailSampleSummary { get; set; }

        public CausationModel? Causation { get; set; }

        [GridColumn(nameof(CreatedByText))]
        [ExportToExcel("نام ثبت کننده")]
        public string? CreatedByText { get; set; }

        public string TracingCodes { get; set; }

        public Guid? DestinationUser { get; set; }

        #endregion

    }
}
