using Teram.Module.AttachmentsManagement.Enums;

namespace Teram.Module.AttachmentsManagement.Models
{
    public class FileResultModel
    {
        public int EntityId { get; set; }
        public Guid? AttachmentId { get; set; }
        public string Message { get; set; }
        public ResultStatus Status { get; set; }
    }
}
