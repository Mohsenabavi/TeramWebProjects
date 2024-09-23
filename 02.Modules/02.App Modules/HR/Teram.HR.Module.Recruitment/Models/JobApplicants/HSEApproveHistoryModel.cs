using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.Module.AttachmentsManagement.Models;
using Teram.Web.Core.Attributes;
using static System.Net.WebRequestMethods;

namespace Teram.HR.Module.Recruitment.Models.JobApplicants
{
    public class HSEApproveHistoryModel : ModelBase<HSEApproveHistory, int>
    {
        [GridColumn(nameof(Firstname))]
        public string Firstname { get; set; } = string.Empty;

        [GridColumn(nameof(Lastname))]
        public string Lastname { get; set; } = string.Empty;
        public int HSEApproveHistoryId { get; set; }
        public OccupationalMedicineApproveStatus OccupationalMedicineApproveStatus { get; set; }

        [GridColumn(nameof(OccupationalMedicineApproveStatusText))]
        public string OccupationalMedicineApproveStatusText => OccupationalMedicineApproveStatus.GetDescription();
        public DateTime? OccupationalMedicineDate { get; set; }

        [GridColumn(nameof(PersianOccupationalMedicineDate))]
        public string PersianOccupationalMedicineDate => (OccupationalMedicineDate.HasValue) ? OccupationalMedicineDate.Value.ToPersianDate() : "";

        public Guid? OccupationalMedicineApprovedBy { get; set; }

        [GridColumn(nameof(OccupationalMedicineRemarks))]
        public string? OccupationalMedicineRemarks { get; set; }
        public Guid? ReferralAtachmentId { get; set; }

        [GridColumn(nameof(Referralfilelink))]
        public string Referralfilelink { get; set; } = string.Empty;
        public Guid? FileSummaryAttchmanetId { get; set; }

        [GridColumn(nameof(FileSummaryLink))]
        public string FileSummaryLink { get; set; } = string.Empty;
        public int JobApplicantId { get; set; }
        public Guid ApprovedByUserId { get; set; }

        [GridColumn(nameof(ApprovedByName))]
        public string? ApprovedByName { get; set; }
    }
}
