using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.HR.Module.Recruitment.Models.JobApplicants;

namespace Teram.HR.Module.Recruitment.Logic.Interfaces
{
    public interface IJobApplicantLogic: IBusinessOperations<JobApplicantModel, JobApplicant, int>
    {
        BusinessOperationResult<JobApplicantModel> GetByUserId(Guid userId);        
        BusinessOperationResult<bool> FinalApprove(JobApplicantModel jobApplicant);
        BusinessOperationResult<bool> FirstApprove(JobApplicantModel jobApplicant);
        BusinessOperationResult<bool> ApproveDocuments(JobApplicantModel jobApplicant);
        BusinessOperationResult<List<JobApplicantModel>> GetWorkersJobApplicants();
        BusinessOperationResult<List<JobApplicantModel>> GetEmployeesJobApplicants();
        BusinessOperationResult<string> GenerateOMFile(string nationalCode,string fullName,string relatedJobPosition, long maxLetterNu,string deadline);
        BusinessOperationResult<string> GenerateDocumentFile(string nationalCode, string promissoryNoteAmount);
        BusinessOperationResult<string> GenerateNoAddictionFile(int jobApplicantId,string nationalCode, string fullName, long maxLetterNu, string deadline);
        BusinessOperationResult<JobApplicantModel> GetById(int  jobApplicantId);
        BusinessOperationResult<List<JobApplicantModel>> GetByIds(List<int> jobApplicantIds);
        BusinessOperationResult<List<JobApplicantModel>> GetByFilter(string? personnelCode, string? nationalCode, string? firstName, string? lastName, bool viewInprogressStatus, FlowType? flowType, ProcessStatus? processStatus, int? start = null, int? length = null);
        BusinessOperationResult<List<JobApplicantModel>> GetByPeriod(DateTime startDate, DateTime endDate);
        BusinessOperationResult<JobApplicantModel> GetByPersonnelCode(string personnelCode);
        BusinessOperationResult<JobApplicantModel> GetByNationalCode(string nationalCode);
    }
}
