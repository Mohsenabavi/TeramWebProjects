using Teram.HR.Module.Recruitment.Models.JobApplicants;

namespace Teram.HR.Module.Recruitment.Logic.Interfaces
{
    public interface IIntroductionGenerationService
    {
        JobApplicantApproveResultModel CreateIntroductionFiles(JobApplicantModel jobApplicatnt);
    }
}
