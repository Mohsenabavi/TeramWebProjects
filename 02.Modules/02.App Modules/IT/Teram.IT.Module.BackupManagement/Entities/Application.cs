using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.IT.Module.BackupManagement.Entities
{

    [Table("Applications", Schema = "BK")]
    public class Application :EntityBase
    {
        public int ApplicationId { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged();
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive == value) return;
                _isActive = value;
                OnPropertyChanged();
            }
        }

        private string _destinationPath;
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

        private string _sourcePath;
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

        public virtual ICollection<ServerPath>? ServerPaths { get; set; }
    }
}
