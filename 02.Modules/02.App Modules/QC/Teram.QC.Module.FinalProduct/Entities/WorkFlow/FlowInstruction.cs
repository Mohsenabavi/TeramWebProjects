using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.QC.Module.FinalProduct.Enums;

namespace Teram.QC.Module.FinalProduct.Entities.WorkFlow
{

    [Table(nameof(FlowInstruction) +"s", Schema = "QCFP")]
    public class FlowInstruction : EntityBase
    {

        public int FlowInstructionId { get; set; }

        private ReferralStatus _fromStatus;
        public ReferralStatus FromStatus
        {
            get { return _fromStatus; }
            set
            {
                if (_fromStatus == value) return;
                _fromStatus = value;
                OnPropertyChanged();
            }
        }

        private ReferralStatus _toStatus;
        public ReferralStatus ToStatus
        {
            get { return _toStatus; }
            set
            {
                if (_toStatus == value) return;
                _toStatus = value;
                OnPropertyChanged();
            }
        }

        private Guid _currentCartableRoleId;
        public Guid CurrentCartableRoleId
        {
            get { return _currentCartableRoleId; }
            set
            {
                if (_currentCartableRoleId == value) return;
                _currentCartableRoleId = value;
                OnPropertyChanged();
            }
        }

        private Guid _nextCartableRoleId;
        public Guid NextCartableRoleId
        {
            get { return _nextCartableRoleId; }
            set
            {
                if (_nextCartableRoleId == value) return;
                _nextCartableRoleId = value;
                OnPropertyChanged();
            }
        }

        private FormStatus _formStatus;
        public FormStatus FormStatus
        {
            get { return _formStatus; }
            set
            {
                if (_formStatus == value) return;
                _formStatus = value;
                OnPropertyChanged();
            }
        }
        public virtual ICollection<FlowInstructionCondition> FlowInstructionConditions { get; set; }

    }
}
