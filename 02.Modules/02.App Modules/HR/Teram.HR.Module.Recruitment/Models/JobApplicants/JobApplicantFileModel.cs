using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.Module.AttachmentsManagement.Models;

namespace Teram.HR.Module.Recruitment.Models.JobApplicants
{
    public class JobApplicantFileModel : ModelBase<JobApplicantFile, int>
    {

        public int JobApplicantFileId { get; set; }
        public int JobApplicantId { get; set; }
        public int AttachmentTypeId { get; set; }
        public Guid AttachmentId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string ImageSrc { get; set; }
        public string AttachmentTypeName { get; set; }
        public DateTime? ApproveDateTime { get; set; }
        public Guid? ApprovedBy { get; set; }
        public ApproveStatus ApproveStatus {  get; set; }
        public string ApproveStatusText=> ApproveStatus.GetDescription();
    }
}
