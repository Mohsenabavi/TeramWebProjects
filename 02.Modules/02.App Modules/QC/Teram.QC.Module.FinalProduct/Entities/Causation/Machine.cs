using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.FinalProduct.Entities.Causation
{
    [Table(nameof(Machine) +"s", Schema = "QCFP")]
    public class Machine:EntityBase
    {
        public int MachineId { get; set; }

        private string _title;

        [StringLength(300)]
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

        private string? _remarks;
        public string? Remarks
        {
            get { return _remarks; }
            set
            {
                if (_remarks == value) return;
                _remarks = value;
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

        public virtual ICollection<Causation>? Causations { get; set; }
    }
}
