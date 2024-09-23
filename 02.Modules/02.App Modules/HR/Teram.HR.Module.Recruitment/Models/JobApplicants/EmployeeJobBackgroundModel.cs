using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.JobApplicants
{
    public class EmployeeJobBackgroundModel : ModelBase<EmployeeJobBackground, int>
    {
        public int EmployeeJobBackgroundId { get; set; }


        [GridColumn(nameof(NationalCode))]
        public string NationalCode { get; set; }


        [GridColumn(nameof(FullName))]
        public string FullName { get; set; }

    
        [GridColumn(nameof(JobTitle))]

        public string JobTitle { get; set; }

        public bool ResumeIsMatch { get; set; }

        [GridColumn(nameof(ResumeIsMatchText))]
        public string ResumeIsMatchText => ResumeIsMatch ? "تطابق دارد" : "تطابق ندارد";

        public bool PerformanceIsApproved { get; set; }

        [GridColumn(nameof(PerformanceIsApprovedText))]
        public string PerformanceIsApprovedText => PerformanceIsApproved ? "دارد" : "ندارد";

        public bool DisciplineIsApproved { get; set; }

        [GridColumn(nameof(DisciplineIsApprovedText))]
        public string DisciplineIsApprovedText => DisciplineIsApproved ? "دارد" : "ندارد";


        public bool MoralityIsApproved { get; set; }

        [GridColumn(nameof(MoralityIsApprovedText))]
        public string MoralityIsApprovedText => MoralityIsApproved ? "دارد" : "ندارد";

        public string? ApprovePerson { get; set; }

        public string? ApprovePersonRemarks { get; set; }

        public string FinalStatement { get; set; }

        [GridColumn(nameof(FinalStatementSummary))]
        public string FinalStatementSummary => (FinalStatement.Length>30) ? FinalStatement.Substring(0, 29)+ "...." : FinalStatement;


        public DateTime? ApproveDate { get; set; }

        [GridColumn(nameof(ApproveDatePersian))]
        public string ApproveDatePersian => (ApproveDate!=null) ? ApproveDate.Value.ToPersianDate() : "-";

        public Guid ApprovedBy { get; set; }

        [GridColumn(nameof(ApprovedByUserName))]
        public string ApprovedByUserName { get; set; }

        public BackgroundJobApproveStatus ApproveStatus { get; set; }


        [GridColumn(nameof(ApproveStatusText))]
        public string ApproveStatusText => ApproveStatus.GetDescription();

        public int JobApplicantId { get; set; }

        public Guid? BackgroundAttchamentId1 { get; set; }
        public Guid? BackgroundAttchamentId2 { get; set; }


    }
}
