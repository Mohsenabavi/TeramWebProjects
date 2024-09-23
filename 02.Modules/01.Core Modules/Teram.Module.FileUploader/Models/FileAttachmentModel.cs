namespace Teram.HR.Module.FileUploader.Models
{
    public class FileAttachmentModel
    {
        public Guid AMSId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public Extraobject extra { get; set; }
    }
}
