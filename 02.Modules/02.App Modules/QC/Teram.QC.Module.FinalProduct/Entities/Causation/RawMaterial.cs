using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.FinalProduct.Entities.Causation
{
    [Table(nameof(RawMaterial) + "s", Schema = "QCFP")]
    public class RawMaterial:EntityBase
    {
        public int RawMaterialId { get; set; }

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

        public virtual ICollection<Causation>? Causations { get; set; }
    }
}
