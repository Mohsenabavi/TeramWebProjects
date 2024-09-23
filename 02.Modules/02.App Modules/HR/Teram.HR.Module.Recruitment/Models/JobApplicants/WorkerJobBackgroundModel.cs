using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.JobApplicants
{
    public class WorkerJobBackgroundModel : ModelBase<WorkerJobBackground, int>
    {
        public int WorkerJobBackgroundId { get; set; }

        [GridColumn(nameof(NationalCode))]
        public string NationalCode {  get; set; }

        [GridColumn(nameof(FullName))]
        public string FullName {  get; set; }

        [GridColumn(nameof(JobTitle))]
        public string JobTitle { get; set; }

        public bool AddressIMatch { get; set; }

        [GridColumn(nameof(AddressIMatchText))]
        public string AddressIMatchText => AddressIMatch ? "تطابق دارد" : "تطابق ندارد";

        public bool ResumeIsMatch { get; set; }

        [GridColumn(nameof(ResumeIsMatchText))]
        public string ResumeIsMatchText => ResumeIsMatch ? "تایید است" : "تایید نیست";

        public string StatementOfPreviousWorkplace { get; set; }

        public string? FirstApprovePerson { get; set; }

        public string? FirstApprovePersonRemarks { get; set; }

        public string? SecondApprovePerson { get; set; }

        public string? SecondtApprovePersonRemarks { get; set; }

        public string? ThirdApprovePerson { get; set; }

        public string? ThirdApprovePersonRemarks { get; set; }

        [GridColumn(nameof(ResearcherName))]
        public string ResearcherName { get; set; }

        public DateTime? ApproveDate { get; set; }


        [GridColumn(nameof(ApproveDatePersian))]

        public string ApproveDatePersian => (ApproveDate!=null) ? ApproveDate.Value.ToPersianDateTime() : " ";

        public Guid ApprovedBy { get; set; }

        [GridColumn(nameof(ApprovedByUserName))]
        public string ApprovedByUserName { get; set; }

        public BackgroundJobApproveStatus ApproveStatus { get; set; }

        [GridColumn(nameof(ApproveStatusText))]
        public string ApproveStatusText => ApproveStatus.GetDescription();

        public int JobApplicantId { get; set; }

        public string FinalStatement { get; set; }

        [GridColumn(nameof(FinalStatementSummary))]
        public string FinalStatementSummary => (FinalStatement.Length>30) ? FinalStatement.Substring(0, 29)+ "...." : FinalStatement;


        public Guid? BackgroundAttchamentId1 { get; set; }
        public Guid? BackgroundAttchamentId2 { get; set; }

    }
}
