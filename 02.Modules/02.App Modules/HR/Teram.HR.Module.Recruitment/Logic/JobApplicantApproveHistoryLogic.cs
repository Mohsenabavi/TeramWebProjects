using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.Web.Core.Security;

namespace Teram.HR.Module.Recruitment.Logic
{

    public class JobApplicantApproveHistoryLogic : BusinessOperations<JobApplicantApproveHistoryModel, JobApplicantApproveHistory, int>, IJobApplicantApproveHistoryLogic
    {
        private readonly IUserPrincipal userPrincipal;

        public JobApplicantApproveHistoryLogic(IPersistenceService<JobApplicantApproveHistory> service, IUserPrincipal userPrincipal) : base(service)
        {
            this.userPrincipal=userPrincipal??throw new ArgumentNullException(nameof(userPrincipal));
        }

        public BusinessOperationResult<JobApplicantApproveHistoryModel> AddNewHistory(int jobApplicantId, ApproveStatus approveStatus, string remark)
        {
            var InserModel = new JobApplicantApproveHistoryModel
            {
                JobApplicantId=jobApplicantId,
                ApproveDate=DateTime.Now,
                ApprovedByUserId=userPrincipal.CurrentUserId,
                ApproveStatus=approveStatus,
                Remarks=remark
            };
            return AddNew(InserModel);
        }
        public BusinessOperationResult<List<JobApplicantApproveHistoryModel>> GetByJobApplicantId(int jobApplicantId, int start, int length) => GetData<JobApplicantApproveHistoryModel>(x => x.JobApplicantId == jobApplicantId, null, null, true, row: start, max: length);
    }
}
