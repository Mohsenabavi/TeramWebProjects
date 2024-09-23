using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.FinalProduct.Entities
{
    [Table(nameof(FinalProductNoncomplianceFile) +"s", Schema = "QCFP")]
    public class FinalProductNoncomplianceFile:EntityBase
    {
        public int FinalProductNoncomplianceFileId { get; set; }

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

        [StringLength(150)]
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

        [StringLength(10)]
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

        private int _finalProductNoncomplianceId;
        public int FinalProductNoncomplianceId
        {
            get { return _finalProductNoncomplianceId; }
            set
            {
                if (_finalProductNoncomplianceId == value) return;
                _finalProductNoncomplianceId = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(FinalProductNoncomplianceId))]
        public virtual FinalProductNoncompliance FinalProductNoncompliance { get; set; }
    }
}
