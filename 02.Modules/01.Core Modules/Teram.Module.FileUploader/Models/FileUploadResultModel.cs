namespace Teram.Module.FileUploader.Models
{
    public class FileUploadResultModel
    {
        public IFormFile File { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Guid AttachmentId { get; set; }
    }
}
