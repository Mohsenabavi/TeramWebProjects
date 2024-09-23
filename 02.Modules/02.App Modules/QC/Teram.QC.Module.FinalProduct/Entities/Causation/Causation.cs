using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.FinalProduct.Entities.Causation
{
    [Table(nameof(Causation) +"s", Schema = "QCFP")]
    public class Causation : EntityBase
    {
        public int CausationId { get; set; }

        private bool? _hasHRCause;
        public bool? HasHRCause
        {
            get { return _hasHRCause; }
            set
            {
                _hasHRCause = value;
                OnPropertyChanged();
            }
        }

        private bool? _hasMethodCause;
        public bool? HasMethodCause
        {
            get { return _hasMethodCause; }
            set
            {
                _hasMethodCause = value;
                OnPropertyChanged();
            }
        }

        private bool? _hasRawMaterialCause;
        public bool? HasRawMaterialCause
        {
            get { return _hasRawMaterialCause; }
            set
            {
                if (_hasRawMaterialCause == value) return;
                _hasRawMaterialCause = value;
                OnPropertyChanged();
            }
        }

        private bool? _hasEssentialCause;
        public bool? HasEssentialCause
        {
            get { return _hasEssentialCause; }
            set
            {
                _hasEssentialCause = value;
                OnPropertyChanged();
            }
        }

        private bool? _hasEquipmentCause;
        public bool? HasEquipmentCause
        {
            get { return _hasEquipmentCause; }
            set
            {
                _hasEquipmentCause = value;
                OnPropertyChanged();
            }
        }
        private int _workStationId;
        public int WorkStationId
        {
            get { return _workStationId; }
            set
            {
                if (_workStationId == value) return;
                _workStationId = value;
                OnPropertyChanged();
            }
        }

        private int _wrongdoerId;
        public int WrongdoerId
        {
            get { return _wrongdoerId; }
            set
            {
                if (_wrongdoerId == value) return;
                _wrongdoerId = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(WorkStationId))]
        public virtual WorkStation? WorkStation { get; set; }

        private int? _machineId;
        public int? MachineId
        {
            get { return _machineId; }
            set
            {
                if (_machineId == value) return;
                _machineId = value;
                OnPropertyChanged();
            }
        }
        [ForeignKey(nameof(MachineId))]
        public virtual Machine? Machine { get; set; }


        private int? _operatorId;
        public int? OperatorId
        {
            get { return _operatorId; }
            set
            {
                if (_operatorId == value) return;
                _operatorId = value;
                OnPropertyChanged();
            }
        }
        [ForeignKey(nameof(OperatorId))]
        public virtual Operator? Operator { get; set; }


        private bool? _isLackOfFit;
        public bool? IsLackOfFit
        {
            get { return _isLackOfFit; }
            set
            {
                _isLackOfFit = value;
                OnPropertyChanged();
            }
        }

        private bool? _isCaseError;
        public bool? IsCaseError
        {
            get { return _isCaseError; }
            set
            {
                _isCaseError = value;
                OnPropertyChanged();
            }
        }

        private bool? _hasLackOfFitWorkerAndJob;
        public bool? HasLackOfFitWorkerAndJob
        {
            get { return _hasLackOfFitWorkerAndJob; }
            set
            {
                _hasLackOfFitWorkerAndJob = value;
                OnPropertyChanged();
            }
        }

        private bool? _hasLackOfEducation;
        public bool? HasLackOfEducation
        {
            get { return _hasLackOfEducation; }
            set
            {
                _hasLackOfEducation = value;
                OnPropertyChanged();
            }
        }

        private bool? _hasFailureOfDefineJob;
        public bool? HasFailureOfDefineJob
        {
            get { return _hasFailureOfDefineJob; }
            set
            {
                _hasFailureOfDefineJob = value;
                OnPropertyChanged();
            }
        }


        private int? _rootCauseId;
        public int? RootCauseId
        {
            get { return _rootCauseId; }
            set
            {
                if (_rootCauseId == value) return;
                _rootCauseId = value;
                OnPropertyChanged();
            }
        }
        [ForeignKey(nameof(RootCauseId))]
        public virtual RootCause? RootCause { get; set; }

        private int? _instructionId;
        public int? InstructionId
        {
            get { return _instructionId; }
            set
            {
                _instructionId = value;
                OnPropertyChanged();
            }
        }
        [ForeignKey(nameof(InstructionId))]
        public virtual Instruction? Instruction { get; set; }


        private int? _unitId;
        public int? UnitId
        {
            get { return _unitId; }
            set
            {
                if (_unitId == value) return;
                _unitId = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(UnitId))]
        public virtual Unit? Unit { get; set; }

        private bool? _canBeIdentifiedAtEntrance;
        public bool? CanBeIdentifiedAtEntrance
        {
            get { return _canBeIdentifiedAtEntrance; }
            set
            {
                _canBeIdentifiedAtEntrance = value;
                OnPropertyChanged();
            }
        }

        private bool? _hasEntitlementLicense;
        public bool? HasEntitlementLicense
        {
            get { return _hasEntitlementLicense; }
            set
            {
                _hasEntitlementLicense = value;
                OnPropertyChanged();
            }
        }

        private bool? _hasNotification;
        public bool? HasNotification
        {
            get { return _hasNotification; }
            set
            {
                _hasNotification = value;
                OnPropertyChanged();
            }
        }

        private string? _notificationNumber;
        public string? NotificationNumber
        {
            get { return _notificationNumber; }
            set
            {
                if (_notificationNumber == value) return;
                _notificationNumber = value;
                OnPropertyChanged();
            }
        }

        private int _finalProductNoncomplianceId;
        public int FinalProductNoncomplianceId
        {
            get { return _finalProductNoncomplianceId; }
            set
            {
                if (_finalProductNoncomplianceId == value) return;
                _finalProductNoncomplianceId = value;
                OnPropertyChanged();
            }
        }

        private string? _instructionDescription;
        public string? InstructionDescription
        {
            get { return _instructionDescription; }
            set
            {
                if (_instructionDescription == value) return;
                _instructionDescription = value;
                OnPropertyChanged();
            }
        }

        private int? _actionerId;
        public int? ActionerId
        {
            get { return _actionerId; }
            set
            {
                if (_actionerId == value) return;
                _actionerId = value;
                OnPropertyChanged();
            }
        }

        public virtual Actioner? Actioner { get; set; }

        public virtual FinalProductNoncompliance? FinalProductNoncompliance { get; set; }
        public virtual ICollection<CorrectiveAction>? CorrectiveActions { get; set; }
    }
}
