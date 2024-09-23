namespace Teram.HR.Module.FileUploader.Models
{
    public class ShowAttachmentModel
    {
        public List<InitialPreviewConfig> InitialPreviewConfig { get; set; }
        public List<string> InitialPreview { get; set; }
        public string UploaderName {  get; set; }
    }
}
