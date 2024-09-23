using Teram.Framework.Core.Logic;
using Teram.QC.Module.IncomingGoods.Entities;

namespace Teram.QC.Module.IncomingGoods.Models
{
    public class IncomingGoodsInspectionFileModel:ModelBase<IncomingGoodsInspectionFile,int>
    {
        public int IncomingGoodsInspectionFileId { get; set; }
        public int IncomingGoodsInspectionId {  get; set; }
        public Guid AttachmentId { get; set; }
        public string FileName {  get; set; }
        public string FileExtension {  get; set; }
        public DateTime CreatedOn {  get; set; }
        public DateTime? DeletedOn {  get; set; }
        public string DownloadUrl {  get; set; }
    }
}
