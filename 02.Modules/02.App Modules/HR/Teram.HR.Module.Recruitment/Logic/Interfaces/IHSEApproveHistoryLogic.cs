using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Models.JobApplicants;

namespace Teram.HR.Module.Recruitment.Logic.Interfaces
{
    public interface IHSEApproveHistoryLogic : IBusinessOperations<HSEApproveHistoryModel, HSEApproveHistory, int>
    {
        BusinessOperationResult<List<HSEApproveHistoryModel>> GetbyJobApplicantId(int jobApplicantId, int start, int length);
    }

}
