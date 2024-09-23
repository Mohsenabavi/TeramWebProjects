using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.FinalProduct.Entities
{

    [Table(nameof(QCControlPlan) +"s", Schema = "QCFP")]
    public class QCControlPlan : EntityBase
    {
        public int QCControlPlanId { get; set; }

        private string _title;

        [StringLength(50)]
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
        public virtual ICollection<AcceptancePeriod>? AcceptancePeriods { get; set; }
        public virtual ICollection<ControlPlanDefect>? ControlPlanDefects {  get; set; }
        
    }
}
