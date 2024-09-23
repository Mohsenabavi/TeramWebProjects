using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.FinalProduct.Entities.WorkFlow
{

    [Table(nameof(FlowInstructionCondition) +"s", Schema = "QCFP")]
    public class FlowInstructionCondition : EntityBase
    {
        public int FlowInstructionConditionId { get; set; }      

        private string _fieldName;
        public string FieldName
        {
            get { return _fieldName; }
            set
            {
                if (_fieldName == value) return;
                _fieldName = value;
                OnPropertyChanged();
            }
        }

        private string _fieldValue;
        public string FieldValue
        {
            get { return _fieldValue; }
            set
            {
                if (_fieldValue == value) return;
                _fieldValue = value;
                OnPropertyChanged();
            }
        }

        private int _flowInstructionId;
        public int FlowInstructionId
        {
            get { return _flowInstructionId; }
            set
            {
                if (_flowInstructionId == value) return;
                _flowInstructionId = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(FlowInstructionId))]
        public virtual FlowInstruction FlowInstruction { get; set; }

    }
}
