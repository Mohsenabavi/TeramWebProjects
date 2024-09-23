using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;

namespace Teram.QC.Module.FinalProduct.Models
{
    public class FinalProductNoncomplianceDetailModel : ModelBase<FinalProductNoncomplianceDetail, int>
    {
        public int FinalProductNoncomplianceDetailId { get; set; }

        public int Number { get; set; }

        public Guid CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string PersianCreateDate => CreateDate.ToPersianDateTime();
        public int? FinalProductInspectionId { get; set; }
        public int FinalProductNoncomplianceId { get; set; }

        public List<FinalProductNoncomplianceDetailSampleModel> FinalProductNoncomplianceDetailSamples { get; set; }


        #region Helper Fileds

        public string? FinalProductNoncomplianceFinalProductNoncomplianceNumber { get; set; }
        public int? FinalProductNoncomplianceControlPlanDefectId { get; set; }
        public bool FinalProductNoncomplianceIsApproved {  get; set; }

        public int FirstSample { get; set; }

        public int SecondSample { get; set; }

        public int ThirdSample { get; set; }

        public int ForthSample { get; set; }

        #endregion
    }
}
