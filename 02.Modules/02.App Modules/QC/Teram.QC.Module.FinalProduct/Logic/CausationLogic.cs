using Microsoft.EntityFrameworkCore;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Entities.WorkFlow;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.QC.Module.FinalProduct.Models.CausationModels;
using Teram.Web.Core.Security;

namespace Teram.QC.Module.FinalProduct.Logic
{

    public class CausationLogic : BusinessOperations<CausationModel, Causation, int>, ICausationLogic
    {
        private readonly IUserPrincipal userPrincipal;

        public CausationLogic(IPersistenceService<Causation> service, IUserPrincipal userPrincipal) : base(service)
        {
            BeforeUpdate += CausationLogic_BeforeUpdate;
            this.userPrincipal = userPrincipal ?? throw new ArgumentNullException(nameof(userPrincipal));
        }

        public BusinessOperationResult<CausationModel> GetByFinalProductNonComplianceId(int finalProductNonComplianceId)
        {
            return GetFirst<CausationModel>(x => x.FinalProductNoncomplianceId == finalProductNonComplianceId);
        }

        private void CausationLogic_BeforeUpdate(TeramEntityEventArgs<Causation, CausationModel, int> entity)
        {
            var cauationResult = Service.DeferrQuery().Include(x => x.CorrectiveActions)
                  .FirstOrDefault(x => x.CausationId == entity.NewEntity.CausationId);

            if (cauationResult != null)
            {
                var copyOfResult = cauationResult;

                if (cauationResult != null && cauationResult?.CorrectiveActions != null && cauationResult?.CorrectiveActions.Count != 0)
                {
                    foreach (var item in cauationResult.CorrectiveActions)
                    {
                        Service.MarkAsRemovedEntity(item);
                    }
                }
                var hasLimitedAccessToCausation = userPrincipal.CurrentUser.HasClaim("Permission", ":Causation:HasLimitedAccessForCausation");

                if (!hasLimitedAccessToCausation && cauationResult != null)
                {
                    cauationResult.HasHRCause = entity.NewEntity.HasHRCause;
                    cauationResult.HasMethodCause = entity.NewEntity.HasMethodCause;
                    cauationResult.HasRawMaterialCause = entity.NewEntity.HasRawMaterialCause;
                    cauationResult.HasEssentialCause = entity.NewEntity.HasEssentialCause;
                    cauationResult.HasEquipmentCause = entity.NewEntity.HasEquipmentCause;
                    cauationResult.MachineId = entity.NewEntity.MachineId;
                    cauationResult.OperatorId = entity.NewEntity.OperatorId;
                    cauationResult.IsLackOfFit = entity.NewEntity.IsLackOfFit;
                    cauationResult.IsCaseError = entity.NewEntity.IsCaseError;
                    cauationResult.HasLackOfFitWorkerAndJob = entity.NewEntity.HasLackOfFitWorkerAndJob;
                    cauationResult.HasLackOfEducation = entity.NewEntity.HasLackOfEducation;
                    cauationResult.HasFailureOfDefineJob = entity.NewEntity.HasFailureOfDefineJob;
                    cauationResult.RootCauseId = entity.NewEntity.RootCauseId;
                    cauationResult.InstructionId = entity.NewEntity.InstructionId;
                    cauationResult.UnitId = entity.NewEntity.UnitId;
                    cauationResult.WorkStationId = entity.NewEntity.WorkStationId;
                    cauationResult.CanBeIdentifiedAtEntrance = entity.NewEntity.CanBeIdentifiedAtEntrance;
                    cauationResult.HasEntitlementLicense = entity.NewEntity.HasEntitlementLicense;
                    cauationResult.HasNotification = entity.NewEntity.HasNotification;
                    cauationResult.NotificationNumber = entity.NewEntity.NotificationNumber;
                    cauationResult.FinalProductNoncomplianceId = entity.NewEntity.FinalProductNoncomplianceId;
                }
                else
                {
                    entity.NewEntity.HasHRCause = copyOfResult.HasHRCause;
                    entity.NewEntity.HasMethodCause = copyOfResult.HasMethodCause;
                    entity.NewEntity.HasRawMaterialCause = copyOfResult.HasRawMaterialCause;
                    entity.NewEntity.HasEssentialCause = copyOfResult.HasEssentialCause;
                    entity.NewEntity.HasEquipmentCause = copyOfResult.HasEquipmentCause;
                    entity.NewEntity.MachineId = copyOfResult.MachineId;
                    entity.NewEntity.OperatorId = copyOfResult.OperatorId;
                    entity.NewEntity.IsLackOfFit = copyOfResult.IsLackOfFit;
                    entity.NewEntity.IsCaseError = copyOfResult.IsCaseError;
                    entity.NewEntity.HasLackOfFitWorkerAndJob = copyOfResult.HasLackOfFitWorkerAndJob;
                    entity.NewEntity.HasLackOfEducation = copyOfResult.HasLackOfEducation;
                    entity.NewEntity.HasFailureOfDefineJob = copyOfResult.HasFailureOfDefineJob;
                    entity.NewEntity.RootCauseId = copyOfResult.RootCauseId;
                    entity.NewEntity.InstructionId = copyOfResult.InstructionId;
                    entity.NewEntity.UnitId = copyOfResult.UnitId;
                    entity.NewEntity.WorkStationId = copyOfResult.WorkStationId;
                    entity.NewEntity.CanBeIdentifiedAtEntrance = copyOfResult.CanBeIdentifiedAtEntrance;
                    entity.NewEntity.HasEntitlementLicense = copyOfResult.HasEntitlementLicense;
                    entity.NewEntity.HasNotification = copyOfResult.HasNotification;
                    entity.NewEntity.NotificationNumber = copyOfResult.NotificationNumber;
                    entity.NewEntity.FinalProductNoncomplianceId = copyOfResult.FinalProductNoncomplianceId;
                }
                cauationResult.CorrectiveActions = (entity.NewEntity.CorrectiveActions != null && entity.NewEntity.CorrectiveActions.Count > 0) ? entity.NewEntity.CorrectiveActions.Select(x => new CorrectiveAction
                {
                    ActionerId = x.ActionerId,
                    Descriiption = x.Descriiption,
                    CausationId = x.CausationId,
                    ApproverId = x.ApproverId,
                    ActionDate = x.ActionDate,

                }).ToList() : [];
                var affectedRowsCount = Service.Save();
            }
        }
    }
}
