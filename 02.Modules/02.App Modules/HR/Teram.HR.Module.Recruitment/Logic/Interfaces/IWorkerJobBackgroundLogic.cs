using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Models.JobApplicants;

namespace Teram.HR.Module.Recruitment.Logic.Interfaces
{
    public interface IWorkerJobBackgroundLogic : IBusinessOperations<WorkerJobBackgroundModel, WorkerJobBackground, int>
    {
        BusinessOperationResult<WorkerJobBackgroundModel> GetByJobApplicantId(int jobApplicantId);
    } 
}
