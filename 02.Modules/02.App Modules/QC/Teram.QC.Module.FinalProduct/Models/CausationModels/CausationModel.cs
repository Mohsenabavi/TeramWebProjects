﻿using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;

namespace Teram.QC.Module.FinalProduct.Models.CausationModels
{
    public class CausationModel : ModelBase<Causation, int>
    {
        public int CausationId { get; set; }
        public bool? HasHRCause { get; set; }
        public bool? HasMethodCause { get; set; }
        public bool? HasRawMaterialCause { get; set; }
        public bool? HasEssentialCause { get; set; }
        public bool? HasEquipmentCause { get; set; }
        public int? MachineId { get; set; }
        public int? OperatorId { get; set; }
        public bool? IsLackOfFit { get; set; }
        public bool? IsCaseError { get; set; }
        public bool? HasLackOfFitWorkerAndJob { get; set; }
        public bool? HasLackOfEducation { get; set; }
        public bool? HasFailureOfDefineJob { get; set; }
        public int? RootCauseId { get; set; }
        public int? InstructionId { get; set; }
        public int? UnitId { get; set; }
        public int WorkStationId { get; set; }
        public bool? CanBeIdentifiedAtEntrance { get; set; }
        public bool? HasEntitlementLicense { get; set; }
        public bool? HasNotification { get; set; }
        public string? NotificationNumber { get; set; }
        public int FinalProductNoncomplianceId { get; set; }

        public int? WrongdoerId { get; set; }
        public List<CorrectiveActionModel>? CorrectiveActions { get; set; }
        public bool IsEditMode { get; set; }
        public string? InstructionDescription { get; set; }
        public bool HasPermissionForSave { get; set; }
        public int? ActionerId { get; set; }
    }
}
