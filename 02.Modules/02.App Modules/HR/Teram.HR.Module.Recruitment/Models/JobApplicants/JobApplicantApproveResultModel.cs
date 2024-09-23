using Teram.Framework.Core.Logic;

namespace Teram.HR.Module.Recruitment.Models.JobApplicants
{
    public class JobApplicantApproveResultModel
    {
        public JobApplicantApproveResultModel()
        {
            Errors=new List<string>();
            FileLinks=new List<FileLinkModel>();
        }
        public string Info {  get; set; }
        public string Message {  get; set; }
        public List<string> Errors { get; set; }
        public List<FileLinkModel> FileLinks { get; set; }
        public OperationResultStatus OperationResultStatus { get; set; }
    }
}
