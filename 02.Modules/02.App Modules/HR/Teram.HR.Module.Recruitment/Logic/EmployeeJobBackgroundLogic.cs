using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;

namespace Teram.HR.Module.Recruitment.Logic
{
  
    public class EmployeeJobBackgroundLogic : BusinessOperations<EmployeeJobBackgroundModel, EmployeeJobBackground, int>, IEmployeeJobBackgroundLogic
    {
        public EmployeeJobBackgroundLogic(IPersistenceService<EmployeeJobBackground> service) : base(service)
        {

        }

        public BusinessOperationResult<EmployeeJobBackgroundModel> GetByJobApplicantId(int hobApplicantId)
        {
           return GetFirst<EmployeeJobBackgroundModel>(x=>x.JobApplicantId== hobApplicantId);
        }
    }


}
