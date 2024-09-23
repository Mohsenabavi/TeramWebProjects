using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Models.JobApplicants;

namespace Teram.HR.Module.Recruitment.Logic.Interfaces
{  
    public interface IJobApplicantsIntroductionLetterLogic : IBusinessOperations<JobApplicantsIntroductionLetterModel, JobApplicantsIntroductionLetter, int>
    {
        BusinessOperationResult<List<JobApplicantsIntroductionLetterModel>> GetByJobApplicantId(int jobApplicantId);
        BusinessOperationResult<long> GetMaxLetterNo();
    }
}
