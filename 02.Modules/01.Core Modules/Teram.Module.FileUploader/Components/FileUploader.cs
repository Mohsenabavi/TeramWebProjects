using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Teram.HR.Module.FileUploader.Models;

namespace Teram.HR.Module.FileUploader.Components
{
    public class FileUploader : ViewComponent
    {
        public FileUploader()
        {
        }
        public IViewComponentResult Invoke(DocumentType documentType, string? entityId, string? FileAccept, int? Index)
        {
            var model = new UploaderModel { Index=Index, Name = (Index!=null && Index>0) ? string.Concat(documentType.ToString(), "_", Index) : documentType.ToString(), Id = entityId, FileAccept = !string.IsNullOrEmpty(FileAccept) ? FileAccept : ".jpg, .png, .jpeg|image/*", DocumentType = documentType };
            return View("Default", model);
        }
    }
}
