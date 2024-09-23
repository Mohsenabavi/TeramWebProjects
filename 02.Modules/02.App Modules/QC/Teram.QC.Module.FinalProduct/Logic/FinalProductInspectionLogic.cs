using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.ServiceContracts;
using Teram.Web.Core.Security;

namespace Teram.QC.Module.FinalProduct.Logic
{
    public class FinalProductInspectionLogic : BusinessOperations<FinalProductInspectionModel, FinalProductInspection, int>, IFinalProductInspectionLogic
    {
        private readonly IFinalProductNoncomplianceLogic finalProductNoncomplianceLogic;
        private readonly IFinalProductNoncomplianceDetailLogic finalProductNoncomplianceDetailLogic;
        private readonly IUserPrincipal userPrincipal;
        private readonly IFinalProductNoncomplianceDetailSampleLogic finalProductNoncomplianceDetailSampleLogic;
        private readonly IControlPlanDefectLogic controlPlanDefectLogic;

        public FinalProductInspectionLogic(IPersistenceService<FinalProductInspection> service, IFinalProductNoncomplianceLogic finalProductNoncomplianceLogic,
            IFinalProductNoncomplianceDetailLogic finalProductNoncomplianceDetailLogic, IUserPrincipal userPrincipal, IFinalProductNoncomplianceDetailSampleLogic finalProductNoncomplianceDetailSampleLogic,
            IControlPlanDefectLogic controlPlanDefectLogic) : base(service)
        {
            BeforeUpdate+=FinalProductInspectionLogic_BeforeUpdate;
            BeforeAdd+=FinalProductInspectionLogic_BeforeAdd;
            this.finalProductNoncomplianceLogic=finalProductNoncomplianceLogic??throw new ArgumentNullException(nameof(finalProductNoncomplianceLogic));
            this.finalProductNoncomplianceDetailLogic=finalProductNoncomplianceDetailLogic??throw new ArgumentNullException(nameof(finalProductNoncomplianceDetailLogic));
            this.userPrincipal=userPrincipal??throw new ArgumentNullException(nameof(userPrincipal));
            this.finalProductNoncomplianceDetailSampleLogic=finalProductNoncomplianceDetailSampleLogic??throw new ArgumentNullException(nameof(finalProductNoncomplianceDetailSampleLogic));
            this.controlPlanDefectLogic=controlPlanDefectLogic??throw new ArgumentNullException(nameof(controlPlanDefectLogic));
        }
        private void FinalProductInspectionLogic_BeforeAdd(TeramEntityEventArgs<FinalProductInspection, FinalProductInspectionModel, int> entity)
        {
            entity.NewEntity.CreatedBy=userPrincipal.CurrentUserId;
            entity.NewEntity.CreateDate=DateTime.Now;
        }

        public BusinessOperationResult<FinalProductInspectionModel> GetByFinalProductInspectionId(int finalProductInspectionId)
        {
            return GetFirst<FinalProductInspectionModel>(x => x.FinalProductInspectionId==finalProductInspectionId);
        }

        public BusinessOperationResult<List<FinalProductInspectionModel>> GetByIds(List<int> finalProductInspectionIds)
        {
            return GetData<FinalProductInspectionModel>(x => finalProductInspectionIds.Contains(x.FinalProductInspectionId));
        }

        public BusinessOperationResult<List<FinalProductInspectionModel>> GetByOrderNoAndProductCode(int orderNo, string productCode)
        {
            return GetData<FinalProductInspectionModel>(x => x.OrderNo==orderNo && x.ProductCode==productCode);
        }

        public BusinessOperationResult<FinalProductInspectionModel> GetByPalletNumber(int palletNumber)
        {
            return GetFirst<FinalProductInspectionModel>(x => x.Number==palletNumber);
        }

        private void FinalProductInspectionLogic_BeforeUpdate(TeramEntityEventArgs<FinalProductInspection, FinalProductInspectionModel, int> entity)
        {

            var finalProductInspectionResult = Service.DeferrQuery().Include(x => x.FinalProductInspectionDefects)
                   .FirstOrDefault(x => x.FinalProductInspectionId == entity.NewEntity.FinalProductInspectionId)??throw new Exception("Not Found");


            var registeredInspectionDefects = (finalProductInspectionResult.FinalProductInspectionDefects!=null && finalProductInspectionResult.FinalProductInspectionDefects.Count>0) ? finalProductInspectionResult.FinalProductInspectionDefects.ToList() : [];
            var newInspectionDefects = (entity.NewEntity.FinalProductInspectionDefects!=null && entity.NewEntity.FinalProductInspectionDefects.Count>0) ? entity.NewEntity.FinalProductInspectionDefects.ToList() : [];
            var newInspectionDefectIds = newInspectionDefects.Select(x => x.ControlPlanDefectId).ToList();

            var deletedReords = registeredInspectionDefects.Where(x => !newInspectionDefectIds.Contains(x.ControlPlanDefectId)).ToList();

            foreach (var deletedRecord in deletedReords)
            {
                var relatedNonComplianceDetail = finalProductNoncomplianceDetailLogic.GetByPalletNumberAndControlPlanDefectId(entity.NewEntity.Number, deletedRecord.ControlPlanDefectId);

                if (relatedNonComplianceDetail.ResultEntity != null)
                {
                    var deleteResult = finalProductNoncomplianceDetailLogic.Delete(relatedNonComplianceDetail.ResultEntity.Key);
                }
            }

            var relatedFinalProductInspectionId = entity.NewEntity.FinalProductInspectionId;

            if (entity.NewEntity.FinalProductInspectionDefects!=null && entity.NewEntity.FinalProductInspectionDefects.Count!=0)
            {
                var relatedControlPlanDefectIds = entity.NewEntity.FinalProductInspectionDefects.Select(x => x.ControlPlanDefectId).ToList();

                var relatedNonCompiancesResult = finalProductNoncomplianceLogic.GetByControlPlanDefectIdsAndFinalProductInspectionId(relatedControlPlanDefectIds, relatedFinalProductInspectionId);

                var relatedNonCompianeDetails = finalProductNoncomplianceDetailLogic.GetByFinalProductNoncomplianceIdsAndFinalProductInspectionId(relatedNonCompiancesResult.ResultEntity.Select(x => x.FinalProductNoncomplianceId).ToList(), relatedFinalProductInspectionId);

                foreach (var item in entity.NewEntity.FinalProductInspectionDefects)
                {
                    var relatedRows = relatedNonCompianeDetails.ResultEntity.Where(x => x.FinalProductNoncomplianceControlPlanDefectId==item.ControlPlanDefectId).ToList();

                    foreach (var relatedRow in relatedRows)
                    {
                        var relatedSamples = relatedRow.FinalProductNoncomplianceDetailSamples;

                        foreach (var sample in relatedSamples)
                        {
                            switch (sample.SampleType)
                            {
                                case Enums.SampleType.FirstSample:
                                    sample.Amount=item.FirstSample;
                                    break;
                                case Enums.SampleType.SecondSample:
                                    sample.Amount=item.SecondSample;
                                    break;
                                case Enums.SampleType.ThirdSample:
                                    sample.Amount=item.ThirdSample;
                                    break;
                                case Enums.SampleType.ForthSample:
                                    sample.Amount=item.ForthSample;
                                    break;
                                default:
                                    break;
                            }
                            var updateResult = finalProductNoncomplianceDetailSampleLogic.Update(sample);
                        }
                    }
                }
            }

            if (finalProductInspectionResult.FinalProductInspectionDefects!=null && finalProductInspectionResult.FinalProductInspectionDefects.Count!=0)
            {
                foreach (var item in finalProductInspectionResult.FinalProductInspectionDefects)
                {
                    Service.MarkAsRemovedEntity(item);
                }
            }

            finalProductInspectionResult.OrderNo = entity.NewEntity.OrderNo;
            finalProductInspectionResult.OrderTitle = entity.NewEntity.OrderTitle;
            finalProductInspectionResult.Number = entity.NewEntity.Number;
            finalProductInspectionResult.Date = entity.NewEntity.Date;
            finalProductInspectionResult.ProductCode = entity.NewEntity.ProductCode;
            finalProductInspectionResult.ProductName = entity.NewEntity.ProductName;
            finalProductInspectionResult.ControlPlan = entity.NewEntity.ControlPlan;
            finalProductInspectionResult.TracingCode = entity.NewEntity.TracingCode;
            finalProductInspectionResult.FinalProductInspectionDefects = (entity.NewEntity.FinalProductInspectionDefects!=null && entity.NewEntity.FinalProductInspectionDefects.Count>0) ? entity.NewEntity.FinalProductInspectionDefects.Select(x => new FinalProductInspectionDefect
            {
                FinalProductInspectionDefectId = x.FinalProductInspectionDefectId,
                FinalProductInspectionId = x.FinalProductInspectionId,
                FirstSample=x.FirstSample,
                SecondSample=x.SecondSample,
                ThirdSample=x.ThirdSample,
                ForthSample=x.ForthSample,
                ControlPlanDefectId=x.ControlPlanDefectId,
            }).ToList() : [];
            var affectedRowsCount = Service.Save();
        }

        public BusinessOperationResult<List<FinalProductInspectionModel>> GetByFilter(int orderNo, int number, string productCode, string TracingCode, string orderTitle, string productName, int? start = null, int? length = null)
        {
            var query = CreateFilterExpression(orderNo, number, productCode, TracingCode, orderTitle, productName);
            if (start.HasValue && length.HasValue)
            {
                return GetData<FinalProductInspectionModel>(query, row: start.Value, max: length.Value, orderByMember: "CreateDate", orderByDescending: true);
            }
            return GetData<FinalProductInspectionModel>(query, null, null, false, null);
        }

        private Expression<Func<FinalProductInspection, bool>> CreateFilterExpression(int orderNo, int number, string productCode, string TracingCode, string orderTitle, string productName)
        {
            Expression<Func<FinalProductInspection, bool>> query = x => true;

            if (orderNo>0)
            {
                query = query.AndAlso(x => x.OrderNo == orderNo);
            }

            if (number>0)
            {
                query = query.AndAlso(x => x.Number == number);
            }
            if (!string.IsNullOrEmpty(productCode))
            {
                query = query.AndAlso(x => x.ProductCode.Contains(productCode));
            }

            if (!string.IsNullOrEmpty(TracingCode))
            {
                query = query.AndAlso(x => x.TracingCode.Contains(TracingCode));
            }

            if (!string.IsNullOrEmpty(orderTitle))
            {
                query = query.AndAlso(x => x.OrderTitle.Contains(orderTitle));
            }

            if (!string.IsNullOrEmpty(productName))
            {
                query = query.AndAlso(x => x.ProductName.Contains(productName));
            }
            return query;
        }
    }
}
