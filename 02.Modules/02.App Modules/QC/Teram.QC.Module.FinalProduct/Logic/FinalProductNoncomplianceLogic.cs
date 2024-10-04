using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Transactions;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.Module.AttachmentsManagement.Models;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Enums;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.QC.Module.FinalProduct.Models.CausationModels;
using Teram.ServiceContracts;
using Teram.Web.Core.Security;

namespace Teram.QC.Module.FinalProduct.Logic
{
    public class FinalProductNoncomplianceLogic : BusinessOperations<FinalProductNoncomplianceModel, FinalProductNoncompliance, int>, IFinalProductNoncomplianceLogic
    {
        private readonly IUserSharedService userSharedService;
        private readonly IUserPrincipal userPrincipal;
        private readonly IManageCartableLogic manageCartableLogic;
        private readonly IFlowInstructionLogic flowInstructionLogic;
        private readonly IOperatorLogic operatorLogic;
        private readonly IFinalProductNonComplianceCartableItemLogic finalProductNonComplianceCartableItemLogic;
        private readonly IConfiguration configuration;
        private readonly IRoleSharedService roleSharedService;

        public FinalProductNoncomplianceLogic(IPersistenceService<FinalProductNoncompliance> service, IUserSharedService userSharedService,
            IUserPrincipal userPrincipal, IManageCartableLogic manageCartableLogic, IFlowInstructionLogic flowInstructionLogic, IOperatorLogic operatorLogic,
            IFinalProductNonComplianceCartableItemLogic finalProductNonComplianceCartableItemLogic,
            IConfiguration configuration, IRoleSharedService roleSharedService) : base(service)
        {
            AfterAdd += FinalProductNoncomplianceLogic_AfterAdd;
            AfterUpdate += FinalProductNoncomplianceLogic_AfterUpdate;
            this.userSharedService = userSharedService ?? throw new ArgumentNullException(nameof(userSharedService));
            this.userPrincipal = userPrincipal ?? throw new ArgumentNullException(nameof(userPrincipal));
            this.manageCartableLogic = manageCartableLogic;
            this.flowInstructionLogic = flowInstructionLogic ?? throw new ArgumentNullException(nameof(flowInstructionLogic));
            this.operatorLogic = operatorLogic ?? throw new ArgumentNullException(nameof(operatorLogic));
            this.finalProductNonComplianceCartableItemLogic = finalProductNonComplianceCartableItemLogic ?? throw new ArgumentNullException(nameof(finalProductNonComplianceCartableItemLogic));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.roleSharedService = roleSharedService ?? throw new ArgumentNullException(nameof(roleSharedService));
        }

        private void FinalProductNoncomplianceLogic_AfterUpdate(TeramEntityEventArgs<FinalProductNoncompliance, FinalProductNoncomplianceModel, int> entity)
        {
            var nextStep = manageCartableLogic.GetNextStep(entity.Model);

            if (nextStep.ResultStatus == OperationResultStatus.Successful && nextStep.ResultEntity is not null)
            {
                var registeredData = entity.Model;

                var relatedProductionManagers = userSharedService.GetUsersInRole("ProductionManager").Result;
                var productionManagerRole = roleSharedService.GetRoleByName("ProductionManager").Result;

                var productionManagerUserIds = relatedProductionManagers.Select(x => x.UserId).ToList();

                if (productionManagerRole.Id == nextStep.ResultEntity.NextCartableRoleId && entity.NewEntity.DestinationUser == null)
                {
                    var relatedDefectUserId = entity.Model.ControlPlanDefectUserId;
                    manageCartableLogic.AddToCartable(nextStep.ResultEntity, registeredData, null, relatedDefectUserId);
                }
                else
                {
                    if (entity.NewEntity.DestinationUser != null)
                    {
                        manageCartableLogic.AddToCartable(nextStep.ResultEntity, registeredData, null, entity.NewEntity.DestinationUser);
                    }
                    else
                    {
                        manageCartableLogic.AddToCartable(nextStep.ResultEntity, registeredData);
                    }
                }



                if (registeredData.FormStatus != nextStep.ResultEntity.FormStatus || registeredData.ReferralStatus != nextStep.ResultEntity.ToStatus)
                {

                    registeredData.FormStatus = nextStep.ResultEntity.FormStatus;
                    registeredData.ReferralStatus = nextStep.ResultEntity.ToStatus;
                    registeredData.DestinationUser = null;
                    var updateResult = Update(registeredData);
                }
            }
        }

        private void FinalProductNoncomplianceLogic_AfterAdd(TeramEntityEventArgs<FinalProductNoncompliance, FinalProductNoncomplianceModel, int> entity)
        {
            entity.NewEntity.CreatedBy = userPrincipal.CurrentUserId;
            entity.NewEntity.CreateDate = DateTime.Now;
            entity.Model.ReferralStatus = ReferralStatus.InitialRegistration;
            var nextStep = manageCartableLogic.GetNextStep(entity.Model);
            if (nextStep.ResultStatus != OperationResultStatus.Successful || nextStep.ResultEntity is null)
            {
                return;
            }
            var registeredData = GetById(entity.NewEntity.FinalProductNoncomplianceId);
            registeredData.ResultEntity.LastComment = entity.NewEntity.Remarks;
            manageCartableLogic.AddToCartable(nextStep.ResultEntity, registeredData.ResultEntity);
            registeredData.ResultEntity.FormStatus = nextStep.ResultEntity.FormStatus;
            registeredData.ResultEntity.ReferralStatus = nextStep.ResultEntity.ToStatus;
            var updateResult = Update(registeredData.ResultEntity);
        }

        public BusinessOperationResult<List<FinalProductNoncomplianceModel>> GetByFilter(bool isOperator, bool isAdmin, int? orderNo, string? productCode, string finalProductNoncomplianceNumber, FormStatus? formStatus, ReferralStatus? referralStatus, int? start = null, int? length = null)
        {
            var query = CreateFilterExpression(isOperator, isAdmin, orderNo, productCode, finalProductNoncomplianceNumber, formStatus, referralStatus);
            if (start.HasValue && length.HasValue)
            {
                return GetData<FinalProductNoncomplianceModel>(query, row: start.Value, max: length.Value, orderByMember: "CreateDate", orderByDescending: true);
            }
            return GetData<FinalProductNoncomplianceModel>(query, null, null, false, null);
        }


        public BusinessOperationResult<List<FinalProductNoncomplianceModel>> GetAllByOrderNoAndProductCodeAndControlPlanDefectId(int orderNo, string productCode, int controlPlanDefectId)
        {
            return GetData<FinalProductNoncomplianceModel>(x => x.OrderNo == orderNo && x.ProductCode == productCode && x.ControlPlanDefectId == controlPlanDefectId);
        }

        public BusinessOperationResult<List<FinalProductNoncomplianceModel>> GetByControlPlanDefctId(int controlPlanDefectId)
        {
            return GetData<FinalProductNoncomplianceModel>(x => x.ControlPlanDefectId == controlPlanDefectId);
        }

        public BusinessOperationResult<List<FinalProductNoncomplianceModel>> GetByControlPlanDefctIds(List<int> controlPlanDefectIds)
        {
            return GetData<FinalProductNoncomplianceModel>(x => controlPlanDefectIds.Contains(x.ControlPlanDefectId));
        }

        public BusinessOperationResult<List<FinalProductNoncomplianceModel>> GetByControlPlanDefectIdsAndFinalProductInspectionId(List<int> controlPlanDefectIds, int finalProductInspectionId)
        {
            var result = new BusinessOperationResult<List<FinalProductNoncomplianceModel>>();
            var data = GetData<FinalProductNoncomplianceModel>(x => controlPlanDefectIds.Contains(x.ControlPlanDefectId));
            var nonCompliancesDetails = data.ResultEntity.SelectMany(x => x.FinalProductNoncomplianceDetails).ToList();
            var nonCompliancesDetailsNonCompliancesIds = nonCompliancesDetails.Where(x => x.FinalProductInspectionId == finalProductInspectionId).Select(x => x.FinalProductNoncomplianceId).ToList();

            var finalResult = data.ResultEntity.Where(x => nonCompliancesDetailsNonCompliancesIds.Contains(x.FinalProductNoncomplianceId)).ToList();

            result.SetSuccessResult(finalResult);
            return result;
        }

        public BusinessOperationResult<FinalProductNoncomplianceModel> GetById(int finalProductNoncomplianceId)
        {

            var finalresult = new BusinessOperationResult<FinalProductNoncomplianceModel>();

            var result = Service.DeferrQuery()
                .Include(x => x.Causation).ThenInclude(x => x.CorrectiveActions)
                .Include(x => x.ControlPlanDefect)
                .ThenInclude(x => x.QCDefect)
                .Include(x => x.FinalProductNoncomplianceDetails)
                .ThenInclude(x => x.FinalProductNoncomplianceDetailSamples)
                .Include(x => x.FinalProductNonComplianceCartableItems)
                .Include(x => x.FinalProductNoncomplianceFiles).AsSplitQuery().AsNoTracking().FirstOrDefault(x => x.FinalProductNoncomplianceId == finalProductNoncomplianceId);

            var data = result.Adapt<FinalProductNoncomplianceModel>();

            if (data.FinalProductNonComplianceCartableItems != null && data.FinalProductNonComplianceCartableItems.Count > 0)
            {

                var userIds = new List<Guid>();

                var SourceUserIds = data.FinalProductNonComplianceCartableItems.Select(x => x.ReferredBy).ToList();

                var destinationUserIds = data.FinalProductNonComplianceCartableItems.Select(x => x.UserId).ToList();

                userIds.AddRange(SourceUserIds);
                userIds.AddRange(destinationUserIds);

                var usersInfo = userSharedService.GetUserInfos(userIds);

                foreach (var cartableItem in data.FinalProductNonComplianceCartableItems)
                {
                    var sourceUserInfo = usersInfo.Where(x => x.UserId == cartableItem.ReferredBy).FirstOrDefault();
                    var destinationUsers = usersInfo.Where(x => x.UserId == cartableItem.UserId).FirstOrDefault();
                    if (sourceUserInfo != null)
                    {
                        cartableItem.SourceUserName = $"{sourceUserInfo.Name} {sourceUserInfo.Family}";
                    }
                    if (destinationUsers != null)
                    {
                        cartableItem.DestinationUserName = $"{destinationUsers.Name} {destinationUsers.Family}";
                    }
                }
            }
            finalresult.SetSuccessResult(data);
            return finalresult;
        }

        public BusinessOperationResult<List<FinalProductNoncomplianceModel>> GetHasBasisByOrderNoAndProductCodeAndControlPlanDefectId(int orderNo, string productCode, int controlPlanDefectId)
        {
            return GetData<FinalProductNoncomplianceModel>(x => x.OrderNo == orderNo && x.ProductCode == productCode && x.ControlPlanDefectId == controlPlanDefectId && x.FinalProductNoncomplianceType == FinalProductNoncomplianceType.HasBasis);
        }

        public BusinessOperationResult<string> GetLastNonComliance()
        {
            var result = new BusinessOperationResult<string>();
            var lastNumber = Service.DeferrQuery().OrderByDescending(x => x.FinalProductNoncomplianceId).Select(x => x.FinalProductNoncomplianceNumber).FirstOrDefault();
            if (lastNumber != null)
            {
                var lastNumericPart = int.Parse(lastNumber.Substring(1));
                var newNumericPart = lastNumericPart + 1;
                var newNumber = $"F{newNumericPart:D5}";
                result.SetSuccessResult(newNumber);
                return result;
            }
            else
            {
                result.SetSuccessResult("F00001");
                return result;
            }
        }

        public BusinessOperationResult<List<FinalProductNoncomplianceModel>> GetWithouBasisByOrderNoAndProductCodeAndControlPlanDefectId(int orderNo, string productCode, int controlPlanDefectId)
        {
            return GetData<FinalProductNoncomplianceModel>(x => x.OrderNo == orderNo && x.ProductCode == productCode && x.ControlPlanDefectId == controlPlanDefectId && x.FinalProductNoncomplianceType == FinalProductNoncomplianceType.WithoutBasis);

        }

        private Expression<Func<FinalProductNoncompliance, bool>> CreateFilterExpression(bool isOperator, bool isAdmin, int? orderNo, string? productCode, string finalProductNoncomplianceNumber, FormStatus? formStatus, ReferralStatus? referralStatus)
        {
            Expression<Func<FinalProductNoncompliance, bool>> query = x => true;

            var currentUserId = userPrincipal.CurrentUserId;

            if (!isAdmin && !isOperator)
            {
                var relatedCartableItems = finalProductNonComplianceCartableItemLogic.GetByUserId(currentUserId);
                
                if (relatedCartableItems.ResultStatus == OperationResultStatus.Successful && relatedCartableItems.ResultEntity is not null)
                {
                    var relatedNonComplianceIds = relatedCartableItems.ResultEntity.Select(x => x.FinalProductNoncomplianceId).ToList();

                    query = query.AndAlso(x => relatedNonComplianceIds.Contains(x.FinalProductNoncomplianceId));
                }
            }

            if (isOperator)
            {
                var relatedOperator = operatorLogic.GetByUserId(currentUserId);
                query = query.AndAlso(x => x.Causation.OperatorId == relatedOperator.ResultEntity.OperatorId || x.Causation.WrongdoerId == relatedOperator.ResultEntity.OperatorId);
            }

            if (orderNo.HasValue)
            {
                query = query.AndAlso(x => x.OrderNo == orderNo);
            }
            if (!string.IsNullOrEmpty(productCode))
            {
                query = query.AndAlso(x => x.ProductCode == productCode);
            }
            if (!string.IsNullOrEmpty(finalProductNoncomplianceNumber))
            {

                query = query.AndAlso(x => x.FinalProductNoncomplianceNumber == finalProductNoncomplianceNumber);
            }

            if (formStatus.HasValue)
            {

                query = query.AndAlso(x => x.FormStatus == formStatus);
            }
            if (referralStatus.HasValue)
            {
                query = query.AndAlso(x => x.ReferralStatus == referralStatus);
            }
            return query;
        }

        private double CalculateSamplePercentage(double sampleSum, int sampleCount)
        {
            if (sampleCount == 0)
            {
                return 0;
            }

            return Math.Round((sampleSum * 100) / sampleCount, 2);
        }

        private List<ReferralStatus> GetCurrentUserReferralStatus()
        {
            var result = new List<ReferralStatus>();

            var userMainRole = manageCartableLogic.GetUserMainRole();

            if (userMainRole.ResultEntity.Name.ToUpper() == "ACTIONERS" ||
                userMainRole.ResultEntity.Name.ToUpper() == "CEO" ||
                userMainRole.ResultEntity.Name.ToUpper() == "QCMANAGER")
            {
                result.Add(ReferralStatus.ReferredToProductionManager);
            }

            var relatedFlowInstruction = flowInstructionLogic.GetByCurrentCartableRoleId(userMainRole.ResultEntity.Id);

            foreach (var flowInstruction in relatedFlowInstruction.ResultEntity)
            {
                result.Add(flowInstruction.FromStatus);
            }
            return result.Distinct().ToList();
        }

        public NoncomplianceDetailSampleSummaryModel GetSamplesSummaryData(FinalProductNoncomplianceModel relatedNonCompianceResult, List<FinalProductInspectionModel> finalProductInspections)
        {
            var detailsSamples = relatedNonCompianceResult.FinalProductNoncomplianceDetails.SelectMany(x => x.FinalProductNoncomplianceDetailSamples).ToList();

            var relatedNonComplianceDetails = relatedNonCompianceResult.FinalProductNoncomplianceDetails;


            var firstSampleSum = detailsSamples.Where(x => x.SampleType == SampleType.FirstSample).Sum(x => x.Amount);
            var SecondSampleSum = detailsSamples.Where(x => x.SampleType == SampleType.SecondSample).Sum(x => x.Amount);
            var thirdSampleSum = detailsSamples.Where(x => x.SampleType == SampleType.ThirdSample).Sum(x => x.Amount);
            var forthSampleSum = detailsSamples.Where(x => x.SampleType == SampleType.ForthSample).Sum(x => x.Amount);


            var firstSampleCount = 0;
            var secondSampleCount = 0;
            var thirdSampleCount = 0;
            var forthSampleCount = 0;

            long firstSampleTotalCount = 0;
            long secondSampleTotalCount = 0;
            long thirdSampleTotalCount = 0;
            long forthSampleTotalCount = 0;

            var firstSampleDetails = detailsSamples.Where(x => x.SampleType == SampleType.FirstSample).ToList();
            var secondSampleDetails = detailsSamples.Where(x => x.SampleType == SampleType.SecondSample).ToList();
            var thirdSampleDetails = detailsSamples.Where(x => x.SampleType == SampleType.ThirdSample).ToList();
            var forthSampleDetails = detailsSamples.Where(x => x.SampleType == SampleType.ForthSample).ToList();

            foreach (var item in firstSampleDetails.Where(x => x.Amount > 0).GroupBy(x => x.FinalProductNoncomplianceDetailId).ToList())
            {
                var relatedNonCompliance = relatedNonComplianceDetails.Where(x => x.FinalProductNoncomplianceDetailId == item.Key).ToList();
                var finalProductInspectionIds = relatedNonCompliance.Select(x => x.FinalProductInspectionId).ToList();
                var relatedFinalProductInspection = finalProductInspections.Where(x => finalProductInspectionIds.Contains(x.FinalProductInspectionId)).FirstOrDefault();
                firstSampleCount += (relatedFinalProductInspection != null) ? relatedFinalProductInspection.SampleCount : 0;
                firstSampleTotalCount += (relatedFinalProductInspection != null) ? (long)relatedFinalProductInspection.TotalCount : 0L;
            }

            foreach (var item in secondSampleDetails.Where(x => x.Amount > 0).GroupBy(x => x.FinalProductNoncomplianceDetailId).ToList())
            {
                var relatedNonCompliance = relatedNonComplianceDetails.Where(x => x.FinalProductNoncomplianceDetailId == item.Key).ToList();
                var finalProductInspectionIds = relatedNonCompliance.Select(x => x.FinalProductInspectionId).ToList();
                var relatedFinalProductInspection = finalProductInspections.Where(x => finalProductInspectionIds.Contains(x.FinalProductInspectionId)).FirstOrDefault();
                secondSampleCount += (relatedFinalProductInspection != null) ? relatedFinalProductInspection.SampleCount : 0;
                secondSampleTotalCount += (relatedFinalProductInspection != null) ? (long)relatedFinalProductInspection.TotalCount : 0L;
            }

            foreach (var item in thirdSampleDetails.Where(x => x.Amount > 0).GroupBy(x => x.FinalProductNoncomplianceDetailId).ToList())
            {
                var relatedNonCompliance = relatedNonComplianceDetails.Where(x => x.FinalProductNoncomplianceDetailId == item.Key).ToList();
                var finalProductInspectionIds = relatedNonCompliance.Select(x => x.FinalProductInspectionId).ToList();
                var relatedFinalProductInspection = finalProductInspections.Where(x => finalProductInspectionIds.Contains(x.FinalProductInspectionId)).FirstOrDefault();
                thirdSampleCount += (relatedFinalProductInspection != null) ? relatedFinalProductInspection.SampleCount : 0;
                thirdSampleTotalCount += (relatedFinalProductInspection != null) ? (long)relatedFinalProductInspection.TotalCount : 0L;
            }

            foreach (var item in forthSampleDetails.Where(x => x.Amount > 0).GroupBy(x => x.FinalProductNoncomplianceDetailId).ToList())
            {
                var relatedNonCompliance = relatedNonComplianceDetails.Where(x => x.FinalProductNoncomplianceDetailId == item.Key).ToList();
                var finalProductInspectionIds = relatedNonCompliance.Select(x => x.FinalProductInspectionId).ToList();
                var relatedFinalProductInspection = finalProductInspections.Where(x => finalProductInspectionIds.Contains(x.FinalProductInspectionId)).FirstOrDefault();
                forthSampleCount += (relatedFinalProductInspection != null) ? relatedFinalProductInspection.SampleCount : 0;
                forthSampleTotalCount += (relatedFinalProductInspection != null) ? (long)relatedFinalProductInspection.TotalCount : 0L;
            }

            var firstSamplePercent = CalculateSamplePercentage(firstSampleSum, firstSampleCount);
            var secondSamplePercent = CalculateSamplePercentage(SecondSampleSum, secondSampleCount);
            var thirdSamplePercent = CalculateSamplePercentage(thirdSampleSum, thirdSampleCount);
            var forthSamplePercent = CalculateSamplePercentage(forthSampleSum, forthSampleCount);


            var firstSamplePalletCount = detailsSamples.Where(x => x.SampleType == SampleType.FirstSample && x.Amount > 0).GroupBy(x => x.FinalProductNoncomplianceDetailId).ToList().Count;
            var secondSamplePalletCount = detailsSamples.Where(x => x.SampleType == SampleType.SecondSample && x.Amount > 0).GroupBy(x => x.FinalProductNoncomplianceDetailId).ToList().Count;
            var thirdSamplePalletCount = detailsSamples.Where(x => x.SampleType == SampleType.ThirdSample && x.Amount > 0).GroupBy(x => x.FinalProductNoncomplianceDetailId).ToList().Count;
            var forthSamplePalletCount = detailsSamples.Where(x => x.SampleType == SampleType.ForthSample && x.Amount > 0).GroupBy(x => x.FinalProductNoncomplianceDetailId).ToList().Count;

            var firstSampleOpinion = detailsSamples.FirstOrDefault(x => x.SampleType == SampleType.FirstSample)?.OpinionTypeQCManager;
            var SecondSampleOpinion = detailsSamples.FirstOrDefault(x => x.SampleType == SampleType.SecondSample)?.OpinionTypeQCManager;
            var ThirdSampleOpinion = detailsSamples.FirstOrDefault(x => x.SampleType == SampleType.ThirdSample)?.OpinionTypeQCManager;
            var ForthSampleOpinion = detailsSamples.FirstOrDefault(x => x.SampleType == SampleType.ForthSample)?.OpinionTypeQCManager;

            var firstSampleOpinionCEO = detailsSamples.FirstOrDefault(x => x.SampleType == SampleType.FirstSample)?.OpinionTypeCEO;
            var SecondSampleOpinionCEO = detailsSamples.FirstOrDefault(x => x.SampleType == SampleType.SecondSample)?.OpinionTypeCEO;
            var ThirdSampleOpinionCEO = detailsSamples.FirstOrDefault(x => x.SampleType == SampleType.ThirdSample)?.OpinionTypeCEO;
            var ForthSampleOpinionCEO = detailsSamples.FirstOrDefault(x => x.SampleType == SampleType.ForthSample)?.OpinionTypeCEO;


            var firstSampleOpinionCEOFinal = detailsSamples.FirstOrDefault(x => x.SampleType == SampleType.FirstSample)?.OpinionTypeCEOFinal;
            var SecondSampleOpinionCEOFinal = detailsSamples.FirstOrDefault(x => x.SampleType == SampleType.SecondSample)?.OpinionTypeCEOFinal;
            var ThirdSampleOpinionCEOFinal = detailsSamples.FirstOrDefault(x => x.SampleType == SampleType.ThirdSample)?.OpinionTypeCEOFinal;
            var ForthSampleOpinionCEOFinal = detailsSamples.FirstOrDefault(x => x.SampleType == SampleType.ForthSample)?.OpinionTypeCEOFinal;


            var firstSampleSeparetdCount = detailsSamples.Where(x => x.SampleType == SampleType.FirstSample).Sum(x => x.SeparatedCount);
            var secondSampleSeparetdCount = detailsSamples.Where(x => x.SampleType == SampleType.SecondSample).Sum(x => x.SeparatedCount);
            var thirdSampleSeparetdCount = detailsSamples.Where(x => x.SampleType == SampleType.ThirdSample).Sum(x => x.SeparatedCount);
            var forthSampleSeparetdCount = detailsSamples.Where(x => x.SampleType == SampleType.ForthSample).Sum(x => x.SeparatedCount);


            var firstSampleWasteCount = detailsSamples.Where(x => x.SampleType == SampleType.ForthSample).Sum(x => x.WasteCount);
            var secondSampleWasteCount = detailsSamples.Where(x => x.SampleType == SampleType.SecondSample).Sum(x => x.WasteCount);
            var thirdSampleWasteCount = detailsSamples.Where(x => x.SampleType == SampleType.ThirdSample).Sum(x => x.WasteCount);
            var forthSampleWasteCount = detailsSamples.Where(x => x.SampleType == SampleType.ForthSample).Sum(x => x.WasteCount);


            var resultModel = new NoncomplianceDetailSampleSummaryModel
            {
                FinalProductNonComplianceId = relatedNonCompianceResult.FinalProductNoncomplianceId,

                FirstSampleSum = firstSampleSum,
                SecondSampleSum = SecondSampleSum,
                ThirdSampleSum = thirdSampleSum,
                ForthSampleSum = forthSampleSum,



                FirstSamplePalletCount = firstSamplePalletCount,
                SecondSamplePalletCount = secondSamplePalletCount,
                ThirdSamplePalletCount = thirdSamplePalletCount,
                ForthSamplePalletCount = forthSamplePalletCount,

                FirstSampleOpinion = firstSampleOpinion,
                SecondSampleOpinion = SecondSampleOpinion,
                ThirdSampleOpinion = ThirdSampleOpinion,
                ForthSampleOpinion = ForthSampleOpinion,

                FirstSampleOpinionCEO = firstSampleOpinionCEO,
                SecondSampleOpinionCEO = SecondSampleOpinionCEO,
                ThirdSampleOpinionCEO = ThirdSampleOpinionCEO,
                ForthSampleOpinionCEO = ForthSampleOpinionCEO,


                FirstSampleOpinionCEOFinal = firstSampleOpinionCEOFinal,
                SecondSampleOpinionCEOFinal = SecondSampleOpinionCEOFinal,
                ThirdSampleOpinionCEOFinal = ThirdSampleOpinionCEOFinal,
                ForthSampleOpinionCEOFinal = ForthSampleOpinionCEOFinal,

                FirstSampleSeparatedCount = firstSampleSeparetdCount,
                SecondSampleSeparatedCount = secondSampleSeparetdCount,
                ThirdSampleSeparatedCount = thirdSampleSeparetdCount,
                ForthSampleSeparatedCount = forthSampleSeparetdCount,

                FirstSampleWasteCount = firstSampleWasteCount,
                SecondSampleWasteCount = secondSampleWasteCount,
                ThirdSampleWasteCount = thirdSampleWasteCount,
                ForthSampleWasteCount = forthSampleWasteCount,


                FirstSampleTotalCount = firstSampleTotalCount,
                FirstSamplePercent = firstSamplePercent,
                SecondSampleTotalCount = secondSampleTotalCount,
                SecondSamplePercent = secondSamplePercent,
                ThirdSampleTotalCount = thirdSampleTotalCount,
                ThirdSamplePercent = thirdSamplePercent,
                ForthSampleTotalCount = forthSampleTotalCount,
                ForthSamplePercent = forthSamplePercent,

                FormStatus = relatedNonCompianceResult.FormStatus,

                IsSeparated = (relatedNonCompianceResult.IsSeperated.HasValue && relatedNonCompianceResult.IsSeperated.Value) ? true : false,
            };
            return resultModel;
        }

        public FinalProductNoncomplianceModel GetNonComplianceDetails(FinalProductNoncomplianceModel finalProductNoncompliance, List<FinalProductInspectionModel> finalProductInspections)
        {
            var nonComplianceResult = finalProductNoncompliance;

            if (nonComplianceResult.FinalProductNoncomplianceDetails != null && nonComplianceResult.FinalProductNoncomplianceDetails.Any())
            {
                var finalProductInspectionIds = nonComplianceResult.FinalProductNoncomplianceDetails.Select(x => x.FinalProductInspectionId).ToList();
                var relatedFinalInspectionWithNonCompliances = finalProductInspections.Where(x => finalProductInspectionIds.Contains(x.FinalProductInspectionId)).ToList();
                var tracingCodes = string.Join("-", relatedFinalInspectionWithNonCompliances.Select(x => x.TracingCode).Distinct().ToList());
                nonComplianceResult.TracingCodes = tracingCodes;
            }

            nonComplianceResult.NoncomplianceDetailSampleSummary = GetSamplesSummaryData(nonComplianceResult, finalProductInspections);
            nonComplianceResult.SalesUnitCartableItems = finalProductNonComplianceCartableItemLogic.GetSalesUniCartableItems(finalProductNoncompliance.FinalProductNoncomplianceId).ResultEntity;

            var hasPermissionForSave = userPrincipal.CurrentUser.HasClaim("Permission", ":Causation:HasLimitedAccessForCausation");

            if (nonComplianceResult.Causation != null)
            {
                nonComplianceResult.Causation.IsEditMode = true;
                nonComplianceResult.Causation.HasPermissionForSave = hasPermissionForSave;
            }
            else
            {
                nonComplianceResult.Causation = new CausationModel { FinalProductNoncomplianceId = nonComplianceResult.FinalProductNoncomplianceId, IsEditMode = false, HasPermissionForSave = hasPermissionForSave };
            }

            if (nonComplianceResult.FinalProductNoncomplianceDetails != null && nonComplianceResult.FinalProductNoncomplianceDetails.Count > 0)
            {

                var relatedIds = nonComplianceResult.FinalProductNoncomplianceDetails.Where(x => x.FinalProductInspectionId.HasValue).Select(x => x.FinalProductInspectionId.Value).ToList();

                var finalProductInspectionResult = finalProductInspections;

                var returnedPallets = finalProductInspectionResult.Where(x => relatedIds.Contains(x.FinalProductInspectionId)).ToList();
                var totalCount = finalProductInspectionResult.Sum(x => x.TotalCount);
                nonComplianceResult.TotalCount = totalCount;

                var sampleCount = finalProductInspectionResult.Sum(x => x.SampleCount);
                var returnedPalletsSampleCount = returnedPallets.Sum(x => x.SampleCount);


                var OrderFirstSampleSum = finalProductInspectionResult.Sum(x => x.FinalProductInspectionDefects.Sum(x => x.FirstSample));
                var OrderSecondSampleSum = finalProductInspectionResult.Sum(x => x.FinalProductInspectionDefects.Sum(x => x.SecondSample));
                var OrderThirdSampleSum = finalProductInspectionResult.Sum(x => x.FinalProductInspectionDefects.Sum(x => x.ThirdSample));
                var OrderForthSampleSum = finalProductInspectionResult.Sum(x => x.FinalProductInspectionDefects.Sum(x => x.ForthSample));


                var returnedPalletsFirstSampleSum = returnedPallets.Sum(x => x.FinalProductInspectionDefects.Sum(x => x.FirstSample));
                var returnedPalletsSecondSampleSum = returnedPallets.Sum(x => x.FinalProductInspectionDefects.Sum(x => x.SecondSample));
                var returnedPalletsThirdSampleSum = returnedPallets.Sum(x => x.FinalProductInspectionDefects.Sum(x => x.ThirdSample));
                var returnedPalletsForthSampleSum = returnedPallets.Sum(x => x.FinalProductInspectionDefects.Sum(x => x.ForthSample));



                var orderSamplesSum = OrderFirstSampleSum + OrderSecondSampleSum + OrderThirdSampleSum + OrderForthSampleSum ?? 0;
                var returnedSampleSum = returnedPalletsFirstSampleSum ?? 0 + returnedPalletsSecondSampleSum ?? 0 + returnedPalletsThirdSampleSum ?? 0 + returnedPalletsForthSampleSum ?? 0;


                nonComplianceResult.OrderNonCompliancePercent = (sampleCount != 0) ? Math.Round(((decimal)returnedSampleSum * 100) / (decimal)sampleCount, 2) : 0;

                nonComplianceResult.ReturnedNonCompliancePercent = (returnedPalletsSampleCount != 0) ? Math.Round(((decimal)returnedSampleSum * 100) / (decimal)returnedPalletsSampleCount, 2) : 0;

                nonComplianceResult.PalletCount = finalProductInspectionResult.Count();
                nonComplianceResult.ReturnedCount = returnedPallets.Sum(x => x.TotalCount);
                nonComplianceResult.ReturnedPalletsCount = returnedPallets.Count();

                var relatedNonComplainceDetailsResult = nonComplianceResult.FinalProductNoncomplianceDetails;


                foreach (var relatedNonComplainceDetailResult in relatedNonComplainceDetailsResult)
                {
                    var relatedSamples = relatedNonComplainceDetailResult.FinalProductNoncomplianceDetailSamples;

                    var relatedFirstSample = relatedSamples.FirstOrDefault(x => x.SampleType == Enums.SampleType.FirstSample);
                    var relatedSecondSample = relatedSamples.FirstOrDefault(x => x.SampleType == Enums.SampleType.SecondSample);
                    var relatedThirdSample = relatedSamples.FirstOrDefault(x => x.SampleType == Enums.SampleType.ThirdSample);
                    var relatedForthSample = relatedSamples.FirstOrDefault(x => x.SampleType == Enums.SampleType.ForthSample);

                    relatedNonComplainceDetailResult.FirstSample = (relatedFirstSample != null) ? relatedFirstSample.Amount : 0;
                    relatedNonComplainceDetailResult.SecondSample = (relatedSecondSample != null) ? relatedSecondSample.Amount : 0;
                    relatedNonComplainceDetailResult.ThirdSample = (relatedThirdSample != null) ? relatedThirdSample.Amount : 0;
                    relatedNonComplainceDetailResult.ForthSample = (relatedForthSample != null) ? relatedForthSample.Amount : 0;

                }
            }

            var filesInfo = nonComplianceResult.FinalProductNoncomplianceFiles;

            if (filesInfo != null && filesInfo.Count > 0)
            {
                var downloadurl = configuration.GetSection("Attachment").Get<AttachmentSection>().DownloadUrl;

                foreach (var item in filesInfo)
                {
                    item.ImageSrc = $"{downloadurl}" + item.AttachmentId;
                }
            }



            return nonComplianceResult;

        }

        public BusinessOperationResult<List<FinalProductNoncomplianceModel>> GetByIds(List<int> finalProductNoncomplianceIds)
        {
            return GetData<FinalProductNoncomplianceModel>(x => finalProductNoncomplianceIds.Contains(x.FinalProductNoncomplianceId));
        }
    }

}
