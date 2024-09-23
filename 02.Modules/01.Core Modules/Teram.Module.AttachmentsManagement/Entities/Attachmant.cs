using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.Module.AttachmentsManagement.Entities
{
    [Table(nameof(Attachmant) + "s", Schema = "Files")]
    public class Attachmant : EntityBase
    {
        private Guid __attachmantId;
        public Guid AttachmantId
        {
            get { return __attachmantId; }
            set
            {
                if (__attachmantId == value) return;
                __attachmantId = value;
                OnPropertyChanged();
            }
        }

        private int _entityRealId;
        public int EntityRealId
        {
            get { return _entityRealId; }
            set
            {
                if (_entityRealId == value) return;
                _entityRealId = value;
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

        private string _contentType;
        public string ContentType
        {
            get { return _contentType; }
            set
            {
                if (_contentType == value) return;
                _contentType = value;
                OnPropertyChanged();
            }
        }

        private long _fileSize;
        public long FileSize
        {
            get { return _fileSize; }
            set
            {
                if (_fileSize == value) return;
                _fileSize = value;
                OnPropertyChanged();
            }
        }


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

        private byte[] _fileData;
        public byte[] FileData
        {
            get { return _fileData; }
            set
            {
                if (_fileData == value) return;
                _fileData = value;
                OnPropertyChanged();
            }
        }

        private bool _isDeleted;
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set
            {
                if (_isDeleted == value) return;
                _isDeleted = value;
                OnPropertyChanged();
            }
        }

    }
}
