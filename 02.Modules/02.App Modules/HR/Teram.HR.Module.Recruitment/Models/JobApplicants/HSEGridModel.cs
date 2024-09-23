using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.JobApplicants
{
    public class HSEGridModel:ModelBase<JobApplicant,int>
    {
        public int JobApplicantId { get; set; }

        [GridColumn(nameof(NationalCode))]
        public string NationalCode { get; set; }

        [GridColumn(nameof(MobileNumber))]
        public string MobileNumber { get; set; }

        [GridColumn(nameof(FullName))]
        public string FullName {  get; set; }

        public string FirstName {  get; set; }
        public string LastName { get; set; }

        public OccupationalMedicineApproveStatus OccupationalMedicineApproveStatus { get; set; }

        [GridColumn(nameof(OccupationalMedicineApproveStatusText))]
        public string OccupationalMedicineApproveStatusText => OccupationalMedicineApproveStatus.GetDescription();

        public DateTime? OccupationalMedicineDate { get; set; }

        [GridColumn(nameof(PersinaOccupationalMedicineDate))]
        public string PersinaOccupationalMedicineDate => (OccupationalMedicineDate!=null) ? OccupationalMedicineDate.Value.ToPersianDate() : "";

        public Guid? OccupationalMedicineApprovedBy { get; set; }

        [GridColumn(nameof(OccupationalMedicineApprovedByName))]
        public string? OccupationalMedicineApprovedByName { get; set; }

        public string? OccupationalMedicineRemarks { get; set; }

        public ProcessStatus ProcessStatus {  get; set; }

        [GridColumn(nameof(ProcessStatusText))]
        public string ProcessStatusText => ProcessStatus.GetDescription();

        [GridColumn(nameof(JobPositionTitle))]
        public string JobPositionTitle { get; set; }       

        public ApproveStatus BaseInformationApproveStatus { get; set; }

        public Guid? ReferralAtachmentId { get; set; }
        public Guid? FileSummaryAttchmanetId { get; set; }

    }
}
