using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.IncomingGoods.Entities
{
    [Table(nameof(IncomingGoodsInspectionFile)+"s", Schema = "QC")]
    public class IncomingGoodsInspectionFile :EntityBase
    {
        public int IncomingGoodsInspectionFileId { get; set; }

        private int _incomingGoodsInspectionId;
        public int IncomingGoodsInspectionId
        {
            get { return _incomingGoodsInspectionId; }
            set
            {
                if (_incomingGoodsInspectionId == value) return;
                _incomingGoodsInspectionId = value;
                OnPropertyChanged();
            }
        }       

        private Guid _attachmentId;
        public Guid AttachmentId
        {
            get { return _attachmentId; }
            set
            {
                if (_attachmentId == value) return;
                _attachmentId = value;
                OnPropertyChanged();
            }
        }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName == value) return;
                _fileName = value;
                OnPropertyChanged();
            }
        }

        private string _fileExtension;

        public string FileExtension
        {
            get { return _fileExtension; }
            set
            {
                if (_fileExtension == value) return;
                _fileExtension = value;
                OnPropertyChanged();
            }
        }

        [Description("تاریخ ایجاد")]
        private DateTime _createdOn;

        public DateTime CreatedOn
        {
            get { return _createdOn; }
            set
            {
                if (_createdOn == value) return;
                _createdOn = value;
                OnPropertyChanged();
            }
        }

        [Description("تاریخ حذف")]
        private DateTime? _deletedOn;

        public DateTime? DeletedOn
        {
            get { return _deletedOn; }
            set
            {
                if (_deletedOn == value) return;
                _deletedOn = value;
                OnPropertyChanged();
            }
        }

      
        [ForeignKey(nameof(IncomingGoodsInspectionId))]
        public virtual IncomingGoodsInspection IncomingGoodsInspection { get; set; }
    }
}
