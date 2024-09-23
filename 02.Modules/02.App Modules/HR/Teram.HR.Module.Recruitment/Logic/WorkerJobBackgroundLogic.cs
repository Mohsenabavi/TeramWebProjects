using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.Web.Core.Model;
using Teram.Web.Core.Security;

namespace Teram.HR.Module.Recruitment.Logic
{
    public class WorkerJobBackgroundLogic : BusinessOperations<WorkerJobBackgroundModel, WorkerJobBackground, int>, IWorkerJobBackgroundLogic
    {       

        public WorkerJobBackgroundLogic(IPersistenceService<WorkerJobBackground> service) : base(service)
        {                        
        }
        public BusinessOperationResult<WorkerJobBackgroundModel> GetByJobApplicantId(int jobApplicantId)
        {
            return GetFirst<WorkerJobBackgroundModel>(x=>x.JobApplicantId== jobApplicantId);
        }       
    }
}
