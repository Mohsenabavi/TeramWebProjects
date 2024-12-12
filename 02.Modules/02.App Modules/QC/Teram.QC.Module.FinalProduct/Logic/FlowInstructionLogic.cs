using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Transactions;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Entities.WorkFlow;
using Teram.QC.Module.FinalProduct.Enums;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.QC.Module.FinalProduct.Models.WorkFlow;
using Teram.ServiceContracts;
using Teram.Web.Core.Security;

namespace Teram.QC.Module.FinalProduct.Logic
{
    public class FlowInstructionLogic : BusinessOperations<FlowInstructionModel, FlowInstruction, int>, IFlowInstructionLogic
    {

        public FlowInstructionLogic(IPersistenceService<FlowInstruction> service) : base(service)
        {
            BeforeUpdate+=FlowInstructionLogic_BeforeUpdate;
        }

        private void FlowInstructionLogic_BeforeUpdate(TeramEntityEventArgs<FlowInstruction, FlowInstructionModel, int> entity)
        {
            var flowInstructionResult = Service.DeferrQuery().Include(x => x.FlowInstructionConditions)
                  .FirstOrDefault(x => x.FlowInstructionId == entity.NewEntity.FlowInstructionId)??throw new Exception("Not Found");

            if (flowInstructionResult.FlowInstructionConditions!=null && flowInstructionResult.FlowInstructionConditions.Count!=0)
            {
                foreach (var item in flowInstructionResult.FlowInstructionConditions)
                {
                    Service.MarkAsRemovedEntity(item);
                }
            }
            flowInstructionResult.FromStatus = entity.NewEntity.FromStatus;
            flowInstructionResult.ToStatus = entity.NewEntity.ToStatus;
            flowInstructionResult.CurrentCartableRoleId = entity.NewEntity.CurrentCartableRoleId;
            flowInstructionResult.NextCartableRoleId = entity.NewEntity.NextCartableRoleId;

            flowInstructionResult.FlowInstructionConditions = (entity.NewEntity.FlowInstructionConditions!=null && entity.NewEntity.FlowInstructionConditions.Count>0) ? entity.NewEntity.FlowInstructionConditions.Select(x => new FlowInstructionCondition
            {
                FlowInstructionId = x.FlowInstructionId,
                FieldName = x.FieldName,
                FieldValue = x.FieldValue,
            }).ToList() : [];
            var affectedRowsCount = Service.Save();
        }
        public BusinessOperationResult<FlowInstructionModel> GetById(int flowInstructionId)
        {
            return GetFirst<FlowInstructionModel>(x => x.FlowInstructionId == flowInstructionId);
        }

        private Expression<Func<FlowInstruction, bool>> CreateFilterExpression(Guid fromRoleId, FinalProductNoncomplianceModel model)
        {
            Expression<Func<FlowInstruction, bool>> query = x => true;

            query = query.AndAlso(x => x.FromStatus==model.ReferralStatus && x.CurrentCartableRoleId==fromRoleId && x.FormStatus==model.FormStatus);

            var condidateFlows = GetData<FlowInstructionModel>(query, null, null, false, null);

            var flowInstructionConditions = condidateFlows.ResultEntity.SelectMany(data => data.FlowInstructionConditions).ToList();

            if (model.IsApproved.HasValue && flowInstructionConditions.Any(x => x.FieldName=="IsApproved"))
            {
                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "IsApproved" && condition.FieldValue.ToLower() == model.IsApproved.Value.ToString().ToLower()));
            }

            if (model.HasFinalResult.HasValue && flowInstructionConditions.Any(x => x.FieldName=="HasFinalResult"))
            {
                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "HasFinalResult" && condition.FieldValue.ToLower() == model.HasFinalResult.Value.ToString().ToLower()));
            }

            if (model.HasSeperationOrder.HasValue && flowInstructionConditions.Any(x => x.FieldName=="HasSeperationOrder"))
            {
                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "HasSeperationOrder" && condition.FieldValue.ToLower() == model.HasSeperationOrder.Value.ToString().ToLower()));
            }

            if (model.HasWasteOrder.HasValue && flowInstructionConditions.Any(x => x.FieldName=="HasWasteOrder"))
            {
                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "HasWasteOrder" && condition.FieldValue.ToLower() == model.HasWasteOrder.Value.ToString().ToLower()));
            }

            if (model.IsSeperated.HasValue && flowInstructionConditions.Any(x => x.FieldName=="IsSeperated"))
            {
                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "IsSeperated" && condition.FieldValue.ToLower() == model.IsSeperated.Value.ToString().ToLower()));
            }

            if (model.NeedToAdvisoryOpinion.HasValue && flowInstructionConditions.Any(x => x.FieldName=="NeedToAdvisoryOpinion"))
            {
                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "NeedToAdvisoryOpinion" && condition.FieldValue.ToLower() == model.NeedToAdvisoryOpinion.Value.ToString().ToLower()));
            }

            if (model.NeedToCkeckByOther.HasValue && flowInstructionConditions.Any(x => x.FieldName=="NeedToCkeckByOther"))
            {
                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "NeedToCkeckByOther" && condition.FieldValue.ToLower() == model.NeedToCkeckByOther.Value.ToString().ToLower()));
            }

            if (model.NeedToRefferToCEO.HasValue && flowInstructionConditions.Any(x => x.FieldName=="NeedToRefferToCEO"))
            {
                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "NeedToRefferToCEO" && condition.FieldValue.ToLower() == model.NeedToRefferToCEO.Value.ToString().ToLower()));
            }

            if (model.HasLeniency.HasValue && flowInstructionConditions.Any(x => x.FieldName=="HasLeniency"))
            {
                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "HasLeniency" && condition.FieldValue.ToLower() == model.HasLeniency.Value.ToString().ToLower()));
            }

            if (flowInstructionConditions.Any(x => x.FieldName == "HasCausation"))
            {
                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "HasCausation" && condition.FieldValue.ToLower() == model.HasCausation.ToString().ToLower()));
            }

            if (flowInstructionConditions.Any(x => x.FieldName == "NeedToCheckByOperationManager"))
            {
                query = query.AndAlso(x => x.FlowInstructionConditions.Any(condition => condition.FieldName == "NeedToCheckByOperationManager" && condition.FieldValue.ToLower() == model.HasCausation.ToString().ToLower()));
            }
            return query;
        }

        private static Expression<Func<FlowInstruction, bool>> CreateGridFilterExpression(ReferralStatus? fromStatus, ReferralStatus? toStatus, FormStatus? formStatus)
        {
            Expression<Func<FlowInstruction, bool>> query = x => true;

            if (fromStatus.HasValue)
            {
                query = query.AndAlso(x => x.FromStatus==fromStatus);
            }
            if (toStatus.HasValue)
            {
                query = query.AndAlso(x => x.ToStatus==toStatus);
            }

            if (formStatus.HasValue)
            {
                query = query.AndAlso(x => x.FormStatus==formStatus);
            }           
            return query;
        }
        public BusinessOperationResult<FlowInstructionModel> GetByFiler(Guid fromRoleId, FinalProductNoncomplianceModel model)
        {
            var query = CreateFilterExpression(fromRoleId, model);

            var data = GetData<FlowInstructionModel>(query, null, null, false, null);

            return GetFirst<FlowInstructionModel>(query, null);
        }

        public BusinessOperationResult<List<FlowInstructionModel>> GetByCurrentCartableRoleId(Guid roleId)
        {
            return GetData<FlowInstructionModel>(x => x.CurrentCartableRoleId == roleId);
        }       

        public BusinessOperationResult<List<FlowInstructionModel>> GetGridDataByFiler(ReferralStatus? fromStatus, ReferralStatus? toStatus, FormStatus? formStatus, int? start = null, int? length = null)
        {
            var query = CreateGridFilterExpression(fromStatus, toStatus, formStatus);
            return GetData<FlowInstructionModel>(query, row: start.Value, max: length.Value, orderByDescending: true);
        }
    }
}
