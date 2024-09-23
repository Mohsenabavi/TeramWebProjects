using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.HR.Module.Recruitment.Models.JobApplicants;

namespace Teram.HR.Module.Recruitment.Logic.Interfaces
{
  
    public interface IHSEApproveLogic : IBusinessOperations<HSEGridModel, JobApplicant, int>
    {
        BusinessOperationResult<HSEGridModel> GetByJobApplicantId(int jobApplicantId);
        BusinessOperationResult<List<HSEGridModel>> GetHSEDataByFilter(string firstName, string lastName, string personnelCode, string nationalCode,bool viewInprogressStatus,FlowType? flowType, ProcessStatus? processStatus, int? start = null, int? length = null);    
        BusinessOperationResult<Guid> AddFile(HSEGridModel newModel, IFormFile file);
    }
}
