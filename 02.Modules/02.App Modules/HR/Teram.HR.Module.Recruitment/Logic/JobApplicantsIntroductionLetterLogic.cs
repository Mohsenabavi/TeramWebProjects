using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;

namespace Teram.HR.Module.Recruitment.Logic
{

    public class JobApplicantsIntroductionLetterLogic : BusinessOperations<JobApplicantsIntroductionLetterModel, JobApplicantsIntroductionLetter, int>, IJobApplicantsIntroductionLetterLogic
    {
        public JobApplicantsIntroductionLetterLogic(IPersistenceService<JobApplicantsIntroductionLetter> service) : base(service)
        {

        }

        public BusinessOperationResult<List<JobApplicantsIntroductionLetterModel>> GetByJobApplicantId(int jobApplicantId)
        {
            return GetData<JobApplicantsIntroductionLetterModel>(x => x.JobApplicantId==jobApplicantId);
        }

        public BusinessOperationResult<long> GetMaxLetterNo()
        {
            var result = new BusinessOperationResult<long>();
            try
            {
                var data = Service.DeferrQuery().ToList();

                if (data.Any() && data!=null)
                {
                    var maxLetterNumber = Service.Entities.Max(x => x.LetterNumber);
                    result.SetSuccessResult(maxLetterNumber);
                }
                else
                {
                    result.SetSuccessResult(0);
                }
                return result;
            }
            catch (Exception)
            {
                result.SetErrorMessage("Error In Get Max LetterNu");
                return result;
            }
        }
    }

}
