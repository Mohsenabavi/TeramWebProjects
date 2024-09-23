using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.HR.Module.Recruitment.Models.JobApplicants;

namespace Teram.HR.Module.Recruitment.Logic.Interfaces
{
   
    public interface IJobApplicantApproveHistoryLogic : IBusinessOperations<JobApplicantApproveHistoryModel, JobApplicantApproveHistory, int>
    {
        BusinessOperationResult<JobApplicantApproveHistoryModel> AddNewHistory(int jobApplicantId, ApproveStatus approveStatus, string remark);
        public BusinessOperationResult<List<JobApplicantApproveHistoryModel>> GetByJobApplicantId(int jobApplicantId, int start, int length);
    }

}
