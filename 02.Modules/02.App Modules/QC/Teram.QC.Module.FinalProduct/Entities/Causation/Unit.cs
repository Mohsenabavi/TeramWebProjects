using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.FinalProduct.Entities.Causation
{
    [Table(nameof(Unit) +"s", Schema = "QCFP")]
    public class Unit:EntityBase
    {
        public int UnitId { get; set; }

        private string _title;

        [StringLength(150)]
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
        public virtual ICollection<Causation>? Causations { get; set; }
    }
}
