using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;

namespace Teram.HR.Module.Recruitment.Logic
{
   
    public class JobPositionLogic : BusinessOperations<JobPositionModel, JobPosition, int>, IJobPositionLogic
    {
        public JobPositionLogic(IPersistenceService<JobPosition> service) : base(service)
        {

        }

        public BusinessOperationResult<List<JobPositionModel>> GetActiveJobPositions()
        {
            return GetData<JobPositionModel>(x => x.IsActive);
        }
    }

}
