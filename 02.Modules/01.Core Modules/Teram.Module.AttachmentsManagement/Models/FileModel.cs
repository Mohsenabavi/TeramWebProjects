using System.ComponentModel.DataAnnotations;

namespace Teram.Module.AttachmentsManagement.Models
{
    public class FileModel
    {
        public int EntityId { get; set; }
        public int ApplicationAttachementTypeId { get; set; }
        [MaxLength(100)]
        public string FileName { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [Required]
        public string File { get; set; }

        [MaxLength(100)]
        public string ContentType { get; set; }
    }
}
