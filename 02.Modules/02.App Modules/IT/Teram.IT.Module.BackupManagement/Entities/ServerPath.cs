using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.IT.Module.BackupManagement.Entities
{

    [Table("ServerPaths", Schema = "BK")]
    public class ServerPath :EntityBase
    {
        public int ServerPathId { get; set; }       

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

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value) return;
                _description = value;
                OnPropertyChanged();
            }
        }

        private int _applicationId;
        public int ApplicationId
        {
            get { return _applicationId; }
            set
            {
                if (_applicationId == value) return;
                _applicationId = value;
                OnPropertyChanged();
            }
        }

        private string? _fileName;
        public string? FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName == value) return;
                _fileName = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(ApplicationId))]
        public virtual Application Application { get;set; }
    }
}
