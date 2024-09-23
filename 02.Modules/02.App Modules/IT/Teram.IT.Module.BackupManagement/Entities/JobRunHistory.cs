using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.IT.Module.BackupManagement.Entities
{

    [Table("JobRunHistory", Schema = "BK")]
    public class JobRunHistory :EntityBase
    {
        public int JobRunHistoryId { get; set; }

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

        private DateTime _runDate;
        public DateTime RunDate
        {
            get { return _runDate; }
            set
            {
                if (_runDate == value) return;
                _runDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _runFinishDate;
        public DateTime RunFinishDate
        {
            get { return _runFinishDate; }
            set
            {
                if (_runFinishDate == value) return;
                _runFinishDate = value;
                OnPropertyChanged();
            }
        }

        private bool _isSucess;
        public bool IsSucess
        {
            get { return _isSucess; }
            set
            {               
                _isSucess = value;
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
