using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;

namespace Teram.QC.Module.FinalProduct.Models
{
    public class FinalProductNoncomplianceFileModel : ModelBase<FinalProductNoncomplianceFile, int>
    {
        public int FinalProductNoncomplianceFileId { get; set; }
        public Guid AttachmentId { get; set; }

        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public int FinalProductNoncomplianceId { get; set; }

        public string? ImageSrc {  get; set; }
    }
}
