using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.FinalProduct.Entities.Causation
{

    [Table(nameof(Instruction) +"s", Schema = "QCFP")]
    public class Instruction:EntityBase
    {
        public int InstructionId { get; set; }

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

        [StringLength(50)]
        private string? _number;
        public string? Number
        {
            get { return _number; }
            set
            {
                if (_number == value) return;
                _number = value;
                OnPropertyChanged();
            }
        }
        public virtual ICollection<Causation>? Causations { get; set; }
    }
}
