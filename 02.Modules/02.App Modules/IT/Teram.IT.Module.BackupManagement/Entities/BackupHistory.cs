using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.IT.Module.BackupManagement.Entities
{
    [Table("BackupHistory", Schema = "BK")]
    public class BackupHistory :EntityBase
    {
        public int BackupHistoryId { get; set; }

        private DateTime _backupDate;
        public DateTime BackupDate
        {
            get { return _backupDate; }
            set
            {
                if (_backupDate == value) return;
                _backupDate = value;
                OnPropertyChanged();
            }
        }

        private string _applicationTitle;
        public string ApplicationTitle
        {
            get { return _applicationTitle; }
            set
            {
                if (_applicationTitle == value) return;
                _applicationTitle = value;
                OnPropertyChanged();
            }
        }

        private string _sourcePath;

        [StringLength(150)]
        public string SourcePath
        {
            get { return _sourcePath; }
            set
            {
                if (_sourcePath == value) return;
                _sourcePath = value;
                OnPropertyChanged();
            }
        }

        private string _destinationPath;

        [StringLength(150)]
        public string DestinationPath
        {
            get { return _destinationPath; }
            set
            {
                if (_destinationPath == value) return;
                _destinationPath = value;
                OnPropertyChanged();
            }
        }      

        private bool _isSuccess;
        public bool IsSuccess
        {
            get { return _isSuccess; }
            set
            {               
                _isSuccess = value;
                OnPropertyChanged();
            }
        }

        private string? _message;
        public string? Message
        {
            get { return _message; }
            set
            {
                if (_message == value) return;
                _message = value;
                OnPropertyChanged();
            }
        }
    }
}
