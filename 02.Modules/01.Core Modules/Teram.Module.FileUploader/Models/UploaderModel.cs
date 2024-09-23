using System.Xml.Linq;

namespace Teram.HR.Module.FileUploader.Models
{
    public class UploaderModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string FileAccept { get; set; }
        public DocumentType DocumentType { get; set; }
        public string RemoveUploadedFilesUrl { get; set; }
        public int? Index {  get; set; }
    }
}
