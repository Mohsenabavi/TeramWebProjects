using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.FinalProduct.Entities.Causation
{
    [Table(nameof(WorkStation) +"s", Schema = "QCFP")]
    public class WorkStation:EntityBase
    {
        public int WorkStationId { get; set; }

        private string _title;

        [StringLength(400)]
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
