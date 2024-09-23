using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.HR.Module.Recruitment.Entities.JobApplicants
{

    [Table(nameof(JobPosition) + "s", Schema = "HR")]
    public class JobPosition :EntityBase
    {
        public int JobPositionId {  get; set; }

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
                _isActive = value;
                OnPropertyChanged();
            }
        }
        public virtual ICollection<JobApplicant>? JobApplicants { get; set; }
    }
}
