using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.Web.Core.Attributes;


namespace Teram.HR.Module.Recruitment.Models.JobApplicants
{
    public class JobApplicantApproveHistoryModel:ModelBase<JobApplicantApproveHistory,int>
    {
        public int JobApplicantApproveHistoryId { get; set; }

        [GridColumn(nameof(ApprovedByName))]
        public string ApprovedByName { get; set; }
        public Guid ApprovedByUserId { get; set; }

        public DateTime ApproveDate {  get; set; }

        [GridColumn(nameof(ApproveDatePerian))]
        public string ApproveDatePerian => ApproveDate.ToPersianDateTime();

        public ApproveStatus ApproveStatus { get; set; }

        [GridColumn(nameof(ApproveStatusText))]
        public string ApproveStatusText => ApproveStatus.GetDescription();

        [GridColumn(nameof(Remarks))]
        public string Remarks {  get; set; }
        public int JobApplicantId {  get; set; }
    }
}
