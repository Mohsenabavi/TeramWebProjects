using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.Web.Core.Attributes;


namespace Teram.HR.Module.Recruitment.Models.JobApplicants
{
    public class JobApplicantModel : ModelBase<JobApplicant, int>
    {      
        public int JobApplicantId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


        [GridColumn(nameof(FullName))]
        public string FullName => $"{FirstName} {LastName}";


        [GridColumn(nameof(MobileNumber))]
        public string MobileNumber { get; set; }

        [GridColumn(nameof(JobPositionTitle))]
        public string JobPositionTitle { get; set; }


        [GridColumn(nameof(ProcessStatusText))]
        public string ProcessStatusText => ProcessStatus.GetDescription();

        [GridColumn(nameof(BaseInformationApproveStatusText))]
        public string BaseInformationApproveStatusText => BaseInformationApproveStatus.GetDescription();


        [GridColumn(nameof(BaseInformationApproveDatePersian))]
        public string BaseInformationApproveDatePersian => (BaseInformationApproveDate!=null) ? BaseInformationApproveDate.Value.ToPersianDate() : "";


        [GridColumn(nameof(PersonnelCode))]
        public string? PersonnelCode { get; set; }

        [GridColumn(nameof(NationalCode))]
        public string NationalCode { get; set; }


        public ApproveStatus BaseInformationApproveStatus { get; set; }

        public Guid? BaseInformationApprovedBy { get; set; }
        public DateTime? BaseInformationApproveDate { get; set; }

        public string? BaseInformationErrors { get; set; }

        public string? Address { get; set; }

        public ProcessStatus ProcessStatus { get; set; }


        public DateTime CreateDate { get; set; }

        [GridColumn(nameof(PersianCreateDate))]
        public string PersianCreateDate => CreateDate.ToPersianDate();
        public Guid CreatedBy { get; set; }




        public Guid UserId { get; set; }
        public GenderType? Gender { get; set; }
        public bool CanUpload { get; set; }
        public bool HasBaseInformation { get; set; }
        public bool IsAdmin { get; set; }
        public int JobPositionId { get; set; }
        public string ApprovedByUserName { get; set; }
        public JobCategory JobCategory { get; set; }
        public OccupationalMedicineApproveStatus OccupationalMedicineApproveStatus { get; set; }

        public string OccupationalMedicineApproveStatusText => OccupationalMedicineApproveStatus.GetDescription();

        public DateTime? OccupationalMedicineDate { get; set; }

        public bool OccupationalMedicineDone { get; set; }

        public string PersinaOccupationalMedicineDate => (OccupationalMedicineDate!=null) ? OccupationalMedicineDate.Value.ToPersianDate() : "";

        public Guid? OccupationalMedicineApprovedBy { get; set; }

        public string? OccupationalMedicineApprovedByName { get; set; }

        public string? OccupationalMedicineRemarks { get; set; }

        public bool NoAddictionDone { get; set; }

        public string NoAddictionDoneText => NoAddictionDone ? "مراجعه کرده" : "عدم مراجعه";
        public DateTime? NoAddictionDate { get; set; }

        public string PersianNoAddictionDate => (NoAddictionDate!=null) ? NoAddictionDate.Value.ToPersianDate() : "";

        public Guid? NoAddictionApprovedBy { get; set; }

        public string? NoAddictionApprovedByName { get; set; }

        public bool NoBadBackgroundDone { get; set; }

        public string NoBadBackgroundDoneText => NoBadBackgroundDone ? "دریافت کرده" : "عدم دریافت";

        public DateTime? NoBadBackgroundDate { get; set; }

        public string PersianNoBadBackgroundDate => (NoBadBackgroundDate!=null) ? NoBadBackgroundDate.Value.ToPersianDate() : "";

        public Guid? NoBadBackgroundApprovedBy { get; set; }

        public string? NoBadBackgroundApprovedByName { get; set; }

        public List<HSEApproveHistoryModel>? HSEApproveHistories { get; set; }

        [GridColumn(nameof(PersianOccupationalMedicineDate))]
        public string PersianOccupationalMedicineDate => OccupationalMedicineDate != null ? OccupationalMedicineDate.Value.ToPersianDate() : "";

        [GridColumn(nameof(OccupationalMedicineDone))]
        public string OccupationalMedicineDoneText => OccupationalMedicineDone ? "مراجعه کرده" : "عدم مراجعه";


        [GridColumn(nameof(CreatedByUserName))]
        public string? CreatedByUserName { get; set; }


        public DateTime? ExpreminetDeadline { get; set; }

        public string? ExpreminetDeadlineText => (ExpreminetDeadline!=null) ? ExpreminetDeadline.Value.ToPersianDate() : "";
        public int ChildCount { get; set; }
        public MaritalStatus MarriageStatus { get; set; }
        public bool NeededForBackgroundCheck { get; set; }
        public string PromissoryNoteAmount { get; set; }

        public bool IsShow { get; set; }

        public DateTime? InvitationToWorkDate { get; set; }

        public string? PersianInvitationToWorkDate => (InvitationToWorkDate.HasValue) ? InvitationToWorkDate.Value.ToPersianDate() : "";

        public Guid? InvitedBy { get; set; }

        public FlowType FlowType { get; set; }


        [GridColumn(nameof(FlowTypeText))]
        public string FlowTypeText=> FlowType.GetDescription();

        public string? IdentityNumber { get; set; }

        public string? FatherName { get; set; }
    }
}
