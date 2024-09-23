using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Models.JobApplicants;

namespace Teram.HR.Module.Recruitment.Logic.Interfaces
{
  
    public interface IEmployeeJobBackgroundLogic : IBusinessOperations<EmployeeJobBackgroundModel, EmployeeJobBackground, int>
    {
        BusinessOperationResult<EmployeeJobBackgroundModel> GetByJobApplicantId(int hobApplicantId);
    }

}
