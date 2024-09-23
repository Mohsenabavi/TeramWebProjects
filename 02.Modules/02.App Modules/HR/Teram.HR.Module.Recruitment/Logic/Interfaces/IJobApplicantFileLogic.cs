using Teram.Framework.Core.Logic;
using Teram.HR.Module.FileUploader.Models;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.Module.FileUploader.Models;

namespace Teram.HR.Module.Recruitment.Logic.Interfaces
{
    public interface IJobApplicantFileLogic : IBusinessOperations<JobApplicantFileModel, JobApplicantFile, int>
    {
        BusinessOperationResult<JobApplicantFileModel> SaveToDataBase(IFormFile file, int jobApplicantId,int attachemntTypeId);
        BusinessOperationResult<List<JobApplicantFileModel>> GetByJobApplicantIdAndAttachmentTypeId(int jobApplicantId, int attachmentTypeId);
        BusinessOperationResult<ShowAttachmentModel> GetAttachmentsByJobApplicantIdAndAttachmentTypeId(int jobApplicantId, int attachmentTypeId);
        BusinessOperationResult<List<JobApplicantFileModel>> GetByJobApplicantId(int jobApplicantId);
        Task<BusinessOperationResult<string>> GetFileBytes(Guid attachmentId);
        BusinessOperationResult<List<FileUploadResultModel>> SaveFiles(IFormFileCollection files, int jobApplicantId,List<JobApplicantFileModel> uploadedFiles);
        int RecognizeAttachmentType(string contentDisposition);
        BusinessOperationResult<JobApplicantFileModel> GetPersonalImage(int jobApplicantId);
        BusinessOperationResult<JobApplicantFileModel> GetResumeFile(int jobApplicantId);
        BusinessOperationResult<List<JobApplicantFileModel>> GetJobApplicantDocumentsExceptResume(int jobApplicantId);
    }
}
