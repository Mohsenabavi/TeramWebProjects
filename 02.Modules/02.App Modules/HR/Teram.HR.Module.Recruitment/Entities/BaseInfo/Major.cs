using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;

namespace Teram.HR.Module.Recruitment.Entities.BaseInfo
{
    [Table(nameof(Major) + "s", Schema = "HR")]
    public class Major : EntityBase
    {
        [Key]
        public int MajorId { get; set; }

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
        public virtual ICollection<JobApplicant> JobApplicants { get; set; }
    }
}
