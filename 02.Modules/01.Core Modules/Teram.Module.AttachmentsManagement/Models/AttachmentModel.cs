using Teram.Framework.Core.Logic;
using Teram.Module.AttachmentsManagement.Entities;

namespace Teram.Module.AttachmentsManagement.Models
{
    public class AttachmentModel : ModelBase<Attachmant, int>
    {
        public Guid AttachmantId { get; set; }

        public int EntityRealId { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public long FileSize { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte[] FileData { get; set; }

        public bool IsDeleted { get; set; }
    }
}
