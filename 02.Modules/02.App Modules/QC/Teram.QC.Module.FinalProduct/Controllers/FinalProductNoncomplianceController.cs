using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Transactions;
using System.Xml.Linq;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Module.AttachmentsManagement.Models;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Enums;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.QC.Module.FinalProduct.Services;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Extensions;

namespace Teram.QC.Module.FinalProduct.Controllers
{
    public class FinalProductNoncomplianceController : ControlPanelBaseController<FinalProductNoncomplianceModel, FinalProductNoncompliance, int>
    {
        private readonly IConfiguration configuration;
        private readonly ICausationLogic causation;
        private readonly IFinalProductNoncomplianceLogic finalProductNoncomplianceLogic;
        private readonly IFinalProductInspectionDefectLogic finalProductInspectionDefectLogic;
        private readonly IFinalProductInspectionLogic finalProductInspectionLogic;
        private readonly IFinalProductNoncomplianceDetailSampleLogic finalProductNoncomplianceDetailSampleLogic;
        private readonly IFinalProductNoncomplianceDetailLogic finalProductNoncomplianceDetailLogic;
        private readonly IQueryService queryService;
        private readonly IControlPlanDefectLogic controlPlanDefectLogic;
        private readonly IFinalProductNoncomplianceFileLogic finalProductNoncomplianceFileLogic;

        public FinalProductNoncomplianceController(ILogger<FinalProductNoncomplianceController> logger,
            IConfiguration configuration, ICausationLogic causation
            , IStringLocalizer<FinalProductNoncomplianceController> localizer, IFinalProductNoncomplianceLogic finalProductNoncomplianceLogic,
            IFinalProductInspectionDefectLogic finalProductInspectionDefectLogic, IFinalProductInspectionLogic finalProductInspectionLogic, IFinalProductNoncomplianceDetailSampleLogic finalProductNoncomplianceDetailSampleLogic,
            IFinalProductNoncomplianceDetailLogic finalProductNoncomplianceDetailLogic, IQueryService queryService, IControlPlanDefectLogic controlPlanDefectLogic, IFinalProductNoncomplianceFileLogic finalProductNoncomplianceFileLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;


            Model = new ViewInformation<FinalProductNoncomplianceModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["models"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["models"],
                OperationColumns = true,
                HomePage = nameof(FinalProductNoncomplianceController).Replace("Controller", "") + "/index",
            };
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.causation = causation ?? throw new ArgumentNullException(nameof(causation));
            this.finalProductNoncomplianceLogic = finalProductNoncomplianceLogic ?? throw new ArgumentNullException(nameof(finalProductNoncomplianceLogic));
            this.finalProductInspectionDefectLogic = finalProductInspectionDefectLogic ?? throw new ArgumentNullException(nameof(finalProductInspectionDefectLogic));
            this.finalProductInspectionLogic = finalProductInspectionLogic ?? throw new ArgumentNullException(nameof(finalProductInspectionLogic));
            this.finalProductNoncomplianceDetailSampleLogic = finalProductNoncomplianceDetailSampleLogic ?? throw new ArgumentNullException(nameof(finalProductNoncomplianceDetailSampleLogic));
            this.finalProductNoncomplianceDetailLogic = finalProductNoncomplianceDetailLogic ?? throw new ArgumentNullException(nameof(finalProductNoncomplianceDetailLogic));
            this.queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
            this.controlPlanDefectLogic = controlPlanDefectLogic ?? throw new ArgumentNullException(nameof(controlPlanDefectLogic));
            this.finalProductNoncomplianceFileLogic = finalProductNoncomplianceFileLogic ?? throw new ArgumentNullException(nameof(finalProductNoncomplianceFileLogic));
        }
        public IActionResult Index()
        {
            return View(Model);
        }

        public IActionResult EmailReciverPermission()
        {
            return Content("This Is Not Actual Action");
        }

        public IActionResult SMSReciverPermission()
        {
            return Content("This Is Not Actual Action");
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetWrongdoerReport(int wrongDoerId)
        {

            var data = causation.GetWrongDoerReport(wrongDoerId).Result;

            return PartialView("_WrongDoerReport", data);
        }
        public IActionResult CloseFinalProductNonCompliance(int finalProductNonComplianceId)
        {
            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            relatedNonCompianceResult.ResultEntity.FinalApproveByQA = true;
            relatedNonCompianceResult.ResultEntity.FinalApproveByQADate = DateTime.Now;
            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.ProcessCompleted;
            relatedNonCompianceResult.ResultEntity.ReferralStatus = ReferralStatus.ProcessCompleted;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Successfully Approved"] });
        }

        public IActionResult TriggerQCManagerModifyOrder(int finalProductNonComplianceId, string comment, ApproveStatus? approveStatus = null)
        {
            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            relatedNonCompianceResult.ResultEntity.LastComment = comment;
            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.QCManagerModifyOrder;
            relatedNonCompianceResult.ResultEntity.IsApproved = false;
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }
        public IActionResult TriggerQcManagerVoided(int finalProductNonComplianceId, string comment, ApproveStatus? approveStatus = null)
        {
            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            relatedNonCompianceResult.ResultEntity.LastComment = comment;
            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.QcManagerVoided;
            relatedNonCompianceResult.ResultEntity.IsApproved = false;
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }

        public IActionResult TriggerQCManagerSendToSalesUnit(Guid destinationUser, int finalProductNonComplianceId, string comments)
        {
            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.QCManagerApproved;
            relatedNonCompianceResult.ResultEntity.IsApproved = true;
            relatedNonCompianceResult.ResultEntity.NeedToAdvisoryOpinion = true;
            relatedNonCompianceResult.ResultEntity.LastComment = comments;
            relatedNonCompianceResult.ResultEntity.DestinationUser = destinationUser;
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }

        public IActionResult TriggerRequestForReviewByCEO(int finalProductNonComplianceId, string comment)
        {

            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            relatedNonCompianceResult.ResultEntity.LastComment = comment;
            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.RequestForReviewByQCManager;
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }

        public IActionResult TriggerSaveOpinionByQCManager(int finalProductNonComplianceId, OpinionType firstSampleOpinion, OpinionType secondSampleOpinion, OpinionType thirdSampleOpinion, OpinionType forthSampleOpinion, string comments)
        {

            var opinionList = new List<OpinionType>();
            if ((int)firstSampleOpinion > 0)
                opinionList.Add(firstSampleOpinion);
            if ((int)secondSampleOpinion > 0)
                opinionList.Add(secondSampleOpinion);
            if ((int)thirdSampleOpinion > 0)
                opinionList.Add(thirdSampleOpinion);
            if ((int)forthSampleOpinion > 0)
                opinionList.Add(forthSampleOpinion);

            var hasError =
                    (opinionList.Any(x => x == OpinionType.CEOOpinion) && opinionList.Any(x => x == OpinionType.Seperation)) ||
                    (opinionList.Any(x => x == OpinionType.CEOOpinion) && opinionList.Any(x => x == OpinionType.Leniency));

            if (hasError)
            {
                var distinctResults = opinionList.Distinct().ToList();
                var errorMessage = $"{localizer["Combination Of"]} {string.Join(" / ", distinctResults.Select(x => x.GetDescription()))} {localizer["Not Permitted"]}";

                return Json(new { result = "fail", message = errorMessage });
            }

            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            var relatedDetailSamples = relatedNonCompianceResult.ResultEntity.FinalProductNoncomplianceDetails.SelectMany(x => x.FinalProductNoncomplianceDetailSamples).ToList();
            foreach (var item in relatedDetailSamples)
            {
                switch (item.SampleType)
                {
                    case SampleType.FirstSample:

                        item.OpinionTypeQCManager = firstSampleOpinion;
                        finalProductNoncomplianceDetailSampleLogic.Update(item);
                        break;

                    case SampleType.SecondSample:
                        item.OpinionTypeQCManager = secondSampleOpinion;
                        finalProductNoncomplianceDetailSampleLogic.Update(item);
                        break;
                    case SampleType.ThirdSample:
                        item.OpinionTypeQCManager = thirdSampleOpinion;
                        finalProductNoncomplianceDetailSampleLogic.Update(item);
                        break;
                    case SampleType.ForthSample:
                        item.OpinionTypeQCManager = forthSampleOpinion;
                        finalProductNoncomplianceDetailSampleLogic.Update(item);
                        break;
                }
            }
            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.QCManagerApproved;
            relatedNonCompianceResult.ResultEntity.IsApproved = true;
            relatedNonCompianceResult.ResultEntity.NeedToAdvisoryOpinion = false;
            relatedNonCompianceResult.ResultEntity.HasSeperationOrder = (firstSampleOpinion == OpinionType.Seperation || secondSampleOpinion == OpinionType.Seperation || thirdSampleOpinion == OpinionType.Seperation || forthSampleOpinion == OpinionType.Seperation);
            relatedNonCompianceResult.ResultEntity.NeedToRefferToCEO = (firstSampleOpinion == OpinionType.CEOOpinion || secondSampleOpinion == OpinionType.CEOOpinion || thirdSampleOpinion == OpinionType.CEOOpinion || forthSampleOpinion == OpinionType.CEOOpinion);
            relatedNonCompianceResult.ResultEntity.HasWasteOrder = (firstSampleOpinion == OpinionType.Waste || secondSampleOpinion == OpinionType.Waste || thirdSampleOpinion == OpinionType.Waste || forthSampleOpinion == OpinionType.Waste);
            relatedNonCompianceResult.ResultEntity.HasLeniency = (firstSampleOpinion == OpinionType.Leniency || secondSampleOpinion == OpinionType.Leniency || thirdSampleOpinion == OpinionType.Leniency || forthSampleOpinion == OpinionType.Leniency);
            relatedNonCompianceResult.ResultEntity.LastComment = comments;
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }

        public IActionResult TriggerSaveSeparationInfo([FromServices] IFinalProductNoncomplianceDetailSampleLogic finalProductNoncomplianceDetailSampleLogic, int finalProductNonComplianceId, int firstSampleSeparatedCount, int secondSampleSeparatedCount, int thirdSampleSeparatedCount, int forthSampleSeparatedCount, string comment)
        {
            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            var relatedDetailSamples = relatedNonCompianceResult.ResultEntity.FinalProductNoncomplianceDetails.SelectMany(x => x.FinalProductNoncomplianceDetailSamples).ToList();

            var firstSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.FirstSample).FirstOrDefault();
            if (firstSampleRow != null)
            {
                firstSampleRow.SeparatedCount = firstSampleSeparatedCount;
                finalProductNoncomplianceDetailSampleLogic.Update(firstSampleRow);
            }
            var secondSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.SecondSample).FirstOrDefault();
            if (secondSampleRow != null)
            {
                secondSampleRow.SeparatedCount = secondSampleSeparatedCount;
                finalProductNoncomplianceDetailSampleLogic.Update(secondSampleRow);
            }
            var thirdSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.ThirdSample).FirstOrDefault();
            if (thirdSampleRow != null)
            {
                thirdSampleRow.SeparatedCount = thirdSampleSeparatedCount;
                finalProductNoncomplianceDetailSampleLogic.Update(thirdSampleRow);
            }
            var forthSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.ForthSample).FirstOrDefault();
            if (forthSampleRow != null)
            {
                forthSampleRow.SeparatedCount = forthSampleSeparatedCount;
                finalProductNoncomplianceDetailSampleLogic.Update(forthSampleRow);
            }

            relatedNonCompianceResult.ResultEntity.IsSeperated = true;
            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.Seperation;
            relatedNonCompianceResult.ResultEntity.LastComment = comment;
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }

        public IActionResult TriggerSaveSeparationInfoAfterCEOOpinion([FromServices] IFinalProductNoncomplianceDetailSampleLogic finalProductNoncomplianceDetailSampleLogic, int finalProductNonComplianceId, int firstSampleSeparatedCount, int secondSampleSeparatedCount, int thirdSampleSeparatedCount, int forthSampleSeparatedCount, string comments)
        {

            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            var relatedDetailSamples = relatedNonCompianceResult.ResultEntity.FinalProductNoncomplianceDetails.SelectMany(x => x.FinalProductNoncomplianceDetailSamples).ToList();

            var firstSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.FirstSample).FirstOrDefault();
            if (firstSampleRow != null)
            {
                firstSampleRow.SeparatedCount = firstSampleSeparatedCount;
                finalProductNoncomplianceDetailSampleLogic.Update(firstSampleRow);
            }
            var secondSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.SecondSample).FirstOrDefault();
            if (secondSampleRow != null)
            {
                secondSampleRow.SeparatedCount = secondSampleSeparatedCount;
                finalProductNoncomplianceDetailSampleLogic.Update(secondSampleRow);
            }
            var thirdSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.ThirdSample).FirstOrDefault();
            if (thirdSampleRow != null)
            {
                thirdSampleRow.SeparatedCount = thirdSampleSeparatedCount;
                finalProductNoncomplianceDetailSampleLogic.Update(thirdSampleRow);
            }
            var forthSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.ForthSample).FirstOrDefault();
            if (forthSampleRow != null)
            {
                forthSampleRow.SeparatedCount = forthSampleSeparatedCount;
                finalProductNoncomplianceDetailSampleLogic.Update(forthSampleRow);
            }
            relatedNonCompianceResult.ResultEntity.IsSeperated = true;
            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.Seperation;
            relatedNonCompianceResult.ResultEntity.LastComment = comments;
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }

        public IActionResult TriggerSaveCEOInfo([FromServices] IFinalProductNoncomplianceDetailSampleLogic finalProductNoncomplianceDetailSampleLogic, int finalProductNonComplianceId,
            OpinionType firstSampleOpinion, OpinionType secondSampleOpinion, OpinionType thirdSampleOpinion, OpinionType forthSampleOpinion, string comments)
        {

            var opinionList = new List<OpinionType>();
            if ((int)firstSampleOpinion > 0)
                opinionList.Add(firstSampleOpinion);
            if ((int)secondSampleOpinion > 0)
                opinionList.Add(secondSampleOpinion);
            if ((int)thirdSampleOpinion > 0)
                opinionList.Add(thirdSampleOpinion);
            if ((int)forthSampleOpinion > 0)
                opinionList.Add(forthSampleOpinion);

            var hasError =
                    (opinionList.Any(x => x == OpinionType.Waste) && opinionList.Any(x => x == OpinionType.Seperation)) ||
                    (opinionList.Any(x => x == OpinionType.Waste) && opinionList.Any(x => x == OpinionType.Leniency));

            if (hasError)
            {
                var distinctResults = opinionList.Distinct().ToList();
                var errorMessage = $"{localizer["Combination Of"]} {string.Join(" / ", distinctResults.Select(x => x.GetDescription()))} {localizer["Not Permitted"]}";

                return Json(new { result = "fail", message = errorMessage });
            }

            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            var relatedDetailSamples = relatedNonCompianceResult.ResultEntity.FinalProductNoncomplianceDetails.SelectMany(x => x.FinalProductNoncomplianceDetailSamples).ToList();

            var firstSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.FirstSample).FirstOrDefault();
            if (firstSampleRow != null)
            {
                firstSampleRow.OpinionTypeCEO = firstSampleOpinion;
                finalProductNoncomplianceDetailSampleLogic.Update(firstSampleRow);
            }
            var secondSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.SecondSample).FirstOrDefault();
            if (secondSampleRow != null)
            {
                secondSampleRow.OpinionTypeCEO = secondSampleOpinion;
                finalProductNoncomplianceDetailSampleLogic.Update(secondSampleRow);
            }
            var thirdSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.ThirdSample).FirstOrDefault();
            if (thirdSampleRow != null)
            {
                thirdSampleRow.OpinionTypeCEO = thirdSampleOpinion;
                finalProductNoncomplianceDetailSampleLogic.Update(thirdSampleRow);
            }
            var forthSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.ForthSample).FirstOrDefault();
            if (forthSampleRow != null)
            {
                forthSampleRow.OpinionTypeCEO = forthSampleOpinion;
                finalProductNoncomplianceDetailSampleLogic.Update(forthSampleRow);
            }

            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.CEOFirstOpinion;
            relatedNonCompianceResult.ResultEntity.HasSeperationOrder = (firstSampleOpinion == OpinionType.Seperation || secondSampleOpinion == OpinionType.Seperation || thirdSampleOpinion == OpinionType.Seperation || forthSampleOpinion == OpinionType.Seperation);
            relatedNonCompianceResult.ResultEntity.HasWasteOrder = (firstSampleOpinion == OpinionType.Waste || secondSampleOpinion == OpinionType.Waste || thirdSampleOpinion == OpinionType.Waste || forthSampleOpinion == OpinionType.Waste);
            relatedNonCompianceResult.ResultEntity.HasLeniency = (firstSampleOpinion == OpinionType.Leniency || secondSampleOpinion == OpinionType.Leniency || thirdSampleOpinion == OpinionType.Leniency || forthSampleOpinion == OpinionType.Leniency);
            relatedNonCompianceResult.ResultEntity.LastComment = comments;
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }


        public IActionResult TriggerSaveCEOInfoAfterSeparation([FromServices] IFinalProductNoncomplianceDetailSampleLogic finalProductNoncomplianceDetailSampleLogic, int finalProductNonComplianceId,
           OpinionType firstSampleOpinion, OpinionType secondSampleOpinion, OpinionType thirdSampleOpinion, OpinionType forthSampleOpinion, string comments)
        {
            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            var relatedDetailSamples = relatedNonCompianceResult.ResultEntity.FinalProductNoncomplianceDetails.SelectMany(x => x.FinalProductNoncomplianceDetailSamples).ToList();

            var firstSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.FirstSample).FirstOrDefault();
            if (firstSampleRow != null)
            {
                firstSampleRow.OpinionTypeCEOFinal = firstSampleOpinion;
                finalProductNoncomplianceDetailSampleLogic.Update(firstSampleRow);
            }
            var secondSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.SecondSample).FirstOrDefault();
            if (secondSampleRow != null)
            {
                secondSampleRow.OpinionTypeCEOFinal = secondSampleOpinion;
                finalProductNoncomplianceDetailSampleLogic.Update(secondSampleRow);
            }
            var thirdSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.ThirdSample).FirstOrDefault();
            if (thirdSampleRow != null)
            {
                thirdSampleRow.OpinionTypeCEOFinal = thirdSampleOpinion;
                finalProductNoncomplianceDetailSampleLogic.Update(thirdSampleRow);
            }
            var forthSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.ForthSample).FirstOrDefault();
            if (forthSampleRow != null)
            {
                forthSampleRow.OpinionTypeCEOFinal = forthSampleOpinion;
                finalProductNoncomplianceDetailSampleLogic.Update(forthSampleRow);
            }
            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.CEOLastOpinion;
            relatedNonCompianceResult.ResultEntity.HasFinalResult = true;
            relatedNonCompianceResult.ResultEntity.LastComment = comments;
            relatedNonCompianceResult.ResultEntity.HasWasteOrder = (firstSampleOpinion == OpinionType.Waste || secondSampleOpinion == OpinionType.Waste || thirdSampleOpinion == OpinionType.Waste || forthSampleOpinion == OpinionType.Waste);
            relatedNonCompianceResult.ResultEntity.HasSeperationOrder = (firstSampleOpinion == OpinionType.Seperation || secondSampleOpinion == OpinionType.Seperation || thirdSampleOpinion == OpinionType.Seperation || forthSampleOpinion == OpinionType.Seperation);
            relatedNonCompianceResult.ResultEntity.HasLeniency = (firstSampleOpinion == OpinionType.Leniency || secondSampleOpinion == OpinionType.Leniency || thirdSampleOpinion == OpinionType.Leniency || forthSampleOpinion == OpinionType.Leniency);
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }


        public IActionResult TriggerSaveCEOInfoAfterFirstRevise([FromServices] IFinalProductNoncomplianceDetailSampleLogic finalProductNoncomplianceDetailSampleLogic, int finalProductNonComplianceId,
           OpinionType firstSampleOpinion, OpinionType secondSampleOpinion, OpinionType thirdSampleOpinion, OpinionType forthSampleOpinion, string comments)
        {

            var opinionList = new List<OpinionType>();
            if ((int)firstSampleOpinion > 0)
                opinionList.Add(firstSampleOpinion);
            if ((int)secondSampleOpinion > 0)
                opinionList.Add(secondSampleOpinion);
            if ((int)thirdSampleOpinion > 0)
                opinionList.Add(thirdSampleOpinion);
            if ((int)forthSampleOpinion > 0)
                opinionList.Add(forthSampleOpinion);

            var hasError =
                    (opinionList.Any(x => x == OpinionType.Waste) && opinionList.Any(x => x == OpinionType.Seperation)) ||
                    (opinionList.Any(x => x == OpinionType.Waste) && opinionList.Any(x => x == OpinionType.Leniency));

            if (hasError)
            {
                var distinctResults = opinionList.Distinct().ToList();
                var errorMessage = $"{localizer["Combination Of"]} {string.Join(" / ", distinctResults.Select(x => x.GetDescription()))} {localizer["Not Permitted"]}";

                return Json(new { result = "fail", message = errorMessage });
            }

            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            var relatedDetailSamples = relatedNonCompianceResult.ResultEntity.FinalProductNoncomplianceDetails.SelectMany(x => x.FinalProductNoncomplianceDetailSamples).ToList();

            var firstSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.FirstSample).FirstOrDefault();
            if (firstSampleRow != null)
            {
                firstSampleRow.OpinionTypeCEO = firstSampleOpinion;
                finalProductNoncomplianceDetailSampleLogic.Update(firstSampleRow);
            }
            var secondSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.SecondSample).FirstOrDefault();
            if (secondSampleRow != null)
            {
                secondSampleRow.OpinionTypeCEO = secondSampleOpinion;
                finalProductNoncomplianceDetailSampleLogic.Update(secondSampleRow);
            }
            var thirdSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.ThirdSample).FirstOrDefault();
            if (thirdSampleRow != null)
            {
                thirdSampleRow.OpinionTypeCEO = thirdSampleOpinion;
                finalProductNoncomplianceDetailSampleLogic.Update(thirdSampleRow);
            }
            var forthSampleRow = relatedDetailSamples.Where(x => x.SampleType == SampleType.ForthSample).FirstOrDefault();
            if (forthSampleRow != null)
            {
                forthSampleRow.OpinionTypeCEO = forthSampleOpinion;
                finalProductNoncomplianceDetailSampleLogic.Update(forthSampleRow);
            }
            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.CEOFirstOpinion;
            relatedNonCompianceResult.ResultEntity.HasFinalResult = false;
            relatedNonCompianceResult.ResultEntity.LastComment = comments;
            relatedNonCompianceResult.ResultEntity.HasWasteOrder = (firstSampleOpinion == OpinionType.Waste || secondSampleOpinion == OpinionType.Waste || thirdSampleOpinion == OpinionType.Waste || forthSampleOpinion == OpinionType.Waste);
            relatedNonCompianceResult.ResultEntity.HasSeperationOrder = (firstSampleOpinion == OpinionType.Seperation || secondSampleOpinion == OpinionType.Seperation || thirdSampleOpinion == OpinionType.Seperation || forthSampleOpinion == OpinionType.Seperation);
            relatedNonCompianceResult.ResultEntity.HasLeniency = (firstSampleOpinion == OpinionType.Leniency || secondSampleOpinion == OpinionType.Leniency || thirdSampleOpinion == OpinionType.Leniency || forthSampleOpinion == OpinionType.Leniency);
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }

        public IActionResult TriggerSaveWasteDocument([FromServices] IFinalProductNoncomplianceDetailSampleLogic finalProductNoncomplianceDetailSampleLogic, int finalProductNonComplianceId, string wasteDocumentNumber)
        {

            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);

            if (relatedNonCompianceResult.ResultStatus != OperationResultStatus.Successful || relatedNonCompianceResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[relatedNonCompianceResult.AllMessages] });
            }

            var relatedDetailSamples = relatedNonCompianceResult.ResultEntity.FinalProductNoncomplianceDetails.
                SelectMany(x => x.FinalProductNoncomplianceDetailSamples).Where(x => x.OpinionTypeCEO == OpinionType.Waste).ToList();

            foreach (var item in relatedDetailSamples)
            {
                item.WasteDocumentNumber = wasteDocumentNumber;
                finalProductNoncomplianceDetailSampleLogic.Update(item);
            }
            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.WasteOperation;
            relatedNonCompianceResult.ResultEntity.LastComment = "--";
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }


        public IActionResult TriggerFinalApprove([FromServices] IFinalProductNoncomplianceDetailSampleLogic finalProductNoncomplianceDetailSampleLogic, int finalProductNonComplianceId)
        {
            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);

            if (relatedNonCompianceResult.ResultStatus != OperationResultStatus.Successful || relatedNonCompianceResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[relatedNonCompianceResult.AllMessages] });
            }
            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.RefferedToQA;
            relatedNonCompianceResult.ResultEntity.LastComment = "--";
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }

        public IActionResult TriggerFinalApproveByOperationManager([FromServices] IFinalProductNoncomplianceDetailSampleLogic finalProductNoncomplianceDetailSampleLogic, int finalProductNonComplianceId)
        {
            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);

            if (relatedNonCompianceResult.ResultStatus != OperationResultStatus.Successful || relatedNonCompianceResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[relatedNonCompianceResult.AllMessages] });
            }
            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.RefferedToQA;
            relatedNonCompianceResult.ResultEntity.NeedToCheckByOperationManager = true;
            relatedNonCompianceResult.ResultEntity.ForceRole = "OPERATIONMANAGER";
            relatedNonCompianceResult.ResultEntity.LastComment = "--";
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }
        public IActionResult TriggerRefferFromProuctionManager(int finalProductNonComplianceId, string comment, Guid destinationUser, YesNoStatus needToAdvisoryOpinion)
        {
            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            relatedNonCompianceResult.ResultEntity.LastComment = comment;
            relatedNonCompianceResult.ResultEntity.DestinationUser = destinationUser;

            if (needToAdvisoryOpinion == YesNoStatus.No)
            {
                relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.RequestForReviewByOthers;
                relatedNonCompianceResult.ResultEntity.ReferralStatus = ReferralStatus.ReferredToOthersForCausation;
            }
            if (needToAdvisoryOpinion == YesNoStatus.Yes)
            {
                relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.RequestForReviewByOthers;
                relatedNonCompianceResult.ResultEntity.NeedToAdvisoryOpinion = true;
            }
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }
        public IActionResult TriggerRefferFromProuctionManagerToTherActioner([FromServices] IActionerLogic actionerLogic, int finalProductNonComplianceId, int destinationUser, string comment)
        {
            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            relatedNonCompianceResult.ResultEntity.LastComment = comment;
            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.RequestForCausationbyOtherManagers;
            var relatedActioner = actionerLogic.GetRow(destinationUser);
            relatedNonCompianceResult.ResultEntity.DestinationUser = relatedActioner.ResultEntity.UserId;
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }
        public IActionResult TriggerSendBackToProductionManager(int finalProductNonComplianceId, string comment)
        {
            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            var lastReferral = relatedNonCompianceResult.ResultEntity.FinalProductNonComplianceCartableItems.OrderByDescending(x => x.InputDatePersian).FirstOrDefault();
            relatedNonCompianceResult.ResultEntity.DestinationUser = lastReferral.ReferredBy;
            relatedNonCompianceResult.ResultEntity.LastComment = comment;
            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.OthersOpinion;
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }

        public IActionResult TriggerSendBackToProductionManagerFromProuctionManager(int finalProductNonComplianceId, string comment)
        {
            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            relatedNonCompianceResult.ResultEntity.LastComment = comment;
            if (relatedNonCompianceResult?.ResultEntity?.FinalProductNonComplianceCartableItems != null)
            {
                var lastRefferal = relatedNonCompianceResult?.ResultEntity?.FinalProductNonComplianceCartableItems.
                                   OrderByDescending(x => x.FinalProductNonComplianceCartableItemId).FirstOrDefault();
                relatedNonCompianceResult.ResultEntity.DestinationUser = lastRefferal?.ReferredBy;
            }

            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.RequestForDeterminingReason;
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }

        public IActionResult TriggerSendBackToCauseFinderByOperationManager([FromServices] IManageCartableLogic manageCartableLogic, int finalProductNonComplianceId, string comment)
        {
            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            relatedNonCompianceResult.ResultEntity.LastComment = comment;
            if (relatedNonCompianceResult?.ResultEntity?.FinalProductNonComplianceCartableItems != null)
            {
                var lastCauseFinderResult = manageCartableLogic.GetLastCauseFinder(relatedNonCompianceResult.ResultEntity.FinalProductNonComplianceCartableItems);
                relatedNonCompianceResult.ResultEntity.DestinationUser = lastCauseFinderResult.ResultEntity?.ReferredBy;
            }
            relatedNonCompianceResult.ResultEntity.NeedToCheckByOperationManager = true;
            relatedNonCompianceResult.ResultEntity.ForceRole = "OPERATIONMANAGER";
            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.RequestForDeterminingReason;
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }

        public IActionResult TriggerSendToOperationManager(int finalProductNonComplianceId, string comment)
        {
            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            relatedNonCompianceResult.ResultEntity.LastComment = comment;
            relatedNonCompianceResult.ResultEntity.NeedToCheckByOperationManager = true;
            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.RefferedToOperationManager;
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }

        public IActionResult TriggerSendBackToQCManager(int finalProductNonComplianceId, string comment)
        {
            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            relatedNonCompianceResult.ResultEntity.LastComment = comment;
            relatedNonCompianceResult.ResultEntity.FormStatus = FormStatus.SalesUnitOpinion;
            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }

        public IActionResult FinalApproveByQCManager(int finalProductNonComplianceId, string comment)
        {

            var relatedNonCompianceResult = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);
            var finalProductInspectionResult = finalProductInspectionLogic.GetByOrderNoAndProductCode(relatedNonCompianceResult.ResultEntity.OrderNo, relatedNonCompianceResult.ResultEntity.ProductCode);
            relatedNonCompianceResult.ResultEntity.LastComment = comment;
            relatedNonCompianceResult.ResultEntity.NoncomplianceDetailSampleSummary = finalProductNoncomplianceLogic.GetSamplesSummaryData(relatedNonCompianceResult.ResultEntity, finalProductInspectionResult.ResultEntity);

            if (relatedNonCompianceResult.ResultEntity.HasFinalResult.HasValue && relatedNonCompianceResult.ResultEntity.HasFinalResult.Value)
            {
                var firstSampleCEOOpinionFinal = relatedNonCompianceResult.ResultEntity.NoncomplianceDetailSampleSummary.FirstSampleOpinionCEOFinal;
                var secondSampleCEOOpinionFinal = relatedNonCompianceResult.ResultEntity.NoncomplianceDetailSampleSummary.SecondSampleOpinionCEOFinal;
                var thirdSampleCEOOpinionFinal = relatedNonCompianceResult.ResultEntity.NoncomplianceDetailSampleSummary.ThirdSampleOpinionCEOFinal;
                var forthSampleCEOOpinionFinal = relatedNonCompianceResult.ResultEntity.NoncomplianceDetailSampleSummary.ForthSampleOpinionCEOFinal;
                relatedNonCompianceResult.ResultEntity.HasWasteOrder = (firstSampleCEOOpinionFinal == OpinionType.Waste || secondSampleCEOOpinionFinal == OpinionType.Waste || thirdSampleCEOOpinionFinal == OpinionType.Waste || forthSampleCEOOpinionFinal == OpinionType.Waste);
                relatedNonCompianceResult.ResultEntity.HasSeperationOrder = (firstSampleCEOOpinionFinal == OpinionType.Seperation || secondSampleCEOOpinionFinal == OpinionType.Seperation || thirdSampleCEOOpinionFinal == OpinionType.Seperation || forthSampleCEOOpinionFinal == OpinionType.Seperation);
                relatedNonCompianceResult.ResultEntity.HasLeniency = (firstSampleCEOOpinionFinal == OpinionType.Leniency || secondSampleCEOOpinionFinal == OpinionType.Leniency || thirdSampleCEOOpinionFinal == OpinionType.Leniency || forthSampleCEOOpinionFinal == OpinionType.Leniency);
            }

            else
            {

                var firstSampleCEOOpinion = relatedNonCompianceResult.ResultEntity.NoncomplianceDetailSampleSummary.FirstSampleOpinionCEO;
                var secondSampleCEOOpinion = relatedNonCompianceResult.ResultEntity.NoncomplianceDetailSampleSummary.SecondSampleOpinionCEO;
                var thirdSampleCEOOpinion = relatedNonCompianceResult.ResultEntity.NoncomplianceDetailSampleSummary.ThirdSampleOpinionCEO;
                var forthSampleCEOOpinion = relatedNonCompianceResult.ResultEntity.NoncomplianceDetailSampleSummary.ForthSampleOpinionCEO;
                relatedNonCompianceResult.ResultEntity.HasWasteOrder = (firstSampleCEOOpinion == OpinionType.Waste || secondSampleCEOOpinion == OpinionType.Waste || thirdSampleCEOOpinion == OpinionType.Waste || forthSampleCEOOpinion == OpinionType.Waste);
                relatedNonCompianceResult.ResultEntity.HasSeperationOrder = (firstSampleCEOOpinion == OpinionType.Seperation || secondSampleCEOOpinion == OpinionType.Seperation || thirdSampleCEOOpinion == OpinionType.Seperation || forthSampleCEOOpinion == OpinionType.Seperation);
                relatedNonCompianceResult.ResultEntity.HasLeniency = (firstSampleCEOOpinion == OpinionType.Leniency || secondSampleCEOOpinion == OpinionType.Leniency || thirdSampleCEOOpinion == OpinionType.Leniency || forthSampleCEOOpinion == OpinionType.Leniency);
            }

            relatedNonCompianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(relatedNonCompianceResult.ResultEntity);
            return Json(new { result = "ok", message = localizer["Referral Done Successfully"] });
        }

        public override IActionResult Save([FromServices] ILogic<FinalProductNoncomplianceModel> service, FinalProductNoncomplianceModel model)
        {
            var addResult = new BusinessOperationResult<FinalProductNoncomplianceModel>();
            model.FinalProductNoncomplianceType = FinalProductNoncomplianceType.HasBasis;
            var files = Request.Form.Files;

            var checkForExist = finalProductNoncomplianceDetailLogic.GetByPalletNumberAndControlPlanDefectId(model.Number, model.ControlPlanDefectId);

            if (checkForExist.ResultStatus == OperationResultStatus.Successful && checkForExist.ResultEntity is not null)
            {
                return Json(new { result = "fail", id = model.FinalProductInspectionId, timeout = 8000, message = localizer["Duplicate NonCompliance"] });
            }

            if (model.FinalProductNoncomplianceId == 0)
            {
                model.FinalProductNoncomplianceDetails = new List<FinalProductNoncomplianceDetailModel>();

                model.FinalProductNoncomplianceNumber = finalProductNoncomplianceLogic.GetLastNonComliance().ResultEntity;
                var insertModel = new FinalProductNoncomplianceDetailModel
                {
                    Number = model.Number,
                    FinalProductInspectionId = model.FinalProductInspectionId,
                    FinalProductNoncomplianceDetailSamples = [
                    new() { SampleType=SampleType.FirstSample, Amount=model.NewFirstSample },
                        new() { SampleType=SampleType.SecondSample, Amount=model.NewSecondSample },
                        new() { SampleType=SampleType.ThirdSample, Amount=model.NewThirdSample },
                        new() { SampleType=SampleType.ForthSample, Amount=model.NewForthSample },
                    ]
                };
                model.FinalProductNoncomplianceDetails.Add(insertModel);
                addResult = service.AddNew(model);
                foreach (var file in files)
                {
                    finalProductNoncomplianceFileLogic.SaveToDataBase(file, addResult.ResultEntity.FinalProductNoncomplianceId);
                }
            }
            else
            {
                var relatedNonCompianceDetails = finalProductNoncomplianceDetailLogic.GetByFinalProductNoncomplianceId(model.FinalProductNoncomplianceId);
                var relatedOldSamples = relatedNonCompianceDetails.ResultEntity.SelectMany(x => x.FinalProductNoncomplianceDetailSamples).Where(x => x.Amount > 0).ToList();
                var insertModel = new FinalProductNoncomplianceDetailModel
                {
                    Number = model.Number,
                    FinalProductInspectionId = model.FinalProductInspectionId,
                    FinalProductNoncomplianceId = (model.FinalProductNoncomplianceId == 0) ? addResult.ResultEntity.FinalProductNoncomplianceId : model.FinalProductNoncomplianceId,
                    FinalProductNoncomplianceDetailSamples = [
                    new() { SampleType=SampleType.FirstSample, Amount=model.NewFirstSample },
                        new() { SampleType=SampleType.SecondSample, Amount=model.NewSecondSample },
                        new() { SampleType=SampleType.ThirdSample, Amount=model.NewThirdSample },
                        new() { SampleType=SampleType.ForthSample, Amount=model.NewForthSample },
                    ]
                };
                var relatedNewSamples = insertModel.FinalProductNoncomplianceDetailSamples.Where(x => x.Amount > 0).ToList();
                var interSectionOfOldAndNewSamples = relatedOldSamples.IntersectBy(relatedNewSamples.Select(e => e.SampleType), x => x.SampleType);
                var newSamples = relatedNewSamples.ExceptBy(relatedOldSamples.Select(e => e.SampleType), x => x.SampleType).ToList();

                foreach (var item in relatedNewSamples)
                {
                    var oldEquivalent = relatedOldSamples.Where(x => x.SampleType == item.SampleType).FirstOrDefault();
                    item.OpinionTypeQCManager = (oldEquivalent != null) ? oldEquivalent.OpinionTypeQCManager : 0;
                    item.OpinionTypeCEO = (oldEquivalent != null) ? oldEquivalent.OpinionTypeCEO : 0;
                    item.OpinionTypeCEOFinal = (oldEquivalent != null) ? oldEquivalent.OpinionTypeCEOFinal : 0;
                }

                var relatedNonComplianceResult = finalProductNoncomplianceLogic.GetById(model.FinalProductNoncomplianceId);

                if (newSamples.Count != 0 && relatedNonComplianceResult.ResultEntity.FormStatus != FormStatus.InitialRegistration)
                {
                    relatedNonComplianceResult.ResultEntity.FormStatus = FormStatus.InitialRegistration;
                    relatedNonComplianceResult.ResultEntity.ReferralStatus = ReferralStatus.InitialRegistration;
                    relatedNonComplianceResult.ResultEntity.IsApproved = false;
                    relatedNonComplianceResult.ResultEntity.HasCausation = false;
                    relatedNonComplianceResult.ResultEntity.HasFinalResult = null;
                    relatedNonComplianceResult.ResultEntity.HasSeperationOrder = null;
                    relatedNonComplianceResult.ResultEntity.HasWasteOrder = null;
                    relatedNonComplianceResult.ResultEntity.DestinationUser = null;
                    relatedNonComplianceResult.ResultEntity.FinalApproveByQA = false;
                    relatedNonComplianceResult.ResultEntity.FinalApproveByQADate = DateTime.MinValue;
                    relatedNonComplianceResult.ResultEntity.IsTriggeredByUserAction = true;
                    relatedNonComplianceResult.ResultEntity.LastComment = "توضیحات سسیتمی : برخی نمونه ها قبلا تعیین تکلیف شده است";
                }

                finalProductNoncomplianceDetailLogic.AddNew(insertModel);
                var result = finalProductNoncomplianceLogic.Update(relatedNonComplianceResult.ResultEntity);
            }
            return Json(new { result = "ok", id = model.FinalProductInspectionId, timeout = 8000, message = string.Concat(localizer["FinalProductNoncompliance"], localizer["Number"], model.FinalProductNoncomplianceNumber, localizer["Saved Successfully"]) });
        }

        public IActionResult GetDefectNoComplianceData(int finalProductNoncomplianceId, int palletNumber, int controlPlanDefectId, int finalProductInspectionId)
        {
            if (finalProductNoncomplianceId == -1)
            {

                return ShowNewnonComplianceForm(controlPlanDefectId, palletNumber, finalProductInspectionId);
            }

            return ShowNonComplianceForm(finalProductNoncomplianceId, palletNumber, controlPlanDefectId, finalProductInspectionId);
        }

        private IActionResult ShowNonComplianceForm(int finalProductNoncomplianceId, int palletNumber, int controlPlanDefectId, int finalProductInspectionId)
        {
            var data = finalProductNoncomplianceLogic.GetById(finalProductNoncomplianceId);

            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[data.AllMessages] });
            }

            var finalProductNoncomplianceDetail = finalProductNoncomplianceDetailLogic.GetByPalletNumberAndControlPlanDefectId(palletNumber, data.ResultEntity.ControlPlanDefectId);

            if (finalProductNoncomplianceDetail.ResultStatus == OperationResultStatus.Successful && finalProductNoncomplianceDetail.ResultEntity is not null)
            {
                var relatedSamples = finalProductNoncomplianceDetail.ResultEntity.FinalProductNoncomplianceDetailSamples;

                if (relatedSamples.Count > 0)
                {
                    var firsSample = relatedSamples.Where(x => x.SampleType == SampleType.FirstSample).FirstOrDefault();
                    data.ResultEntity.NewFirstSample = (firsSample != null) ? firsSample.Amount : 0;

                    var secondSample = relatedSamples.Where(x => x.SampleType == SampleType.SecondSample).FirstOrDefault();
                    data.ResultEntity.NewSecondSample = (secondSample != null) ? secondSample.Amount : 0;

                    var thirdSample = relatedSamples.Where(x => x.SampleType == SampleType.ThirdSample).FirstOrDefault();
                    data.ResultEntity.NewThirdSample = (thirdSample != null) ? thirdSample.Amount : 0;

                    var forthSample = relatedSamples.Where(x => x.SampleType == SampleType.ForthSample).FirstOrDefault();
                    data.ResultEntity.NewForthSample = (forthSample != null) ? forthSample.Amount : 0;

                }
            }

            var finalProductInspectionDefectResult = finalProductInspectionDefectLogic.GetByFianalProductInspectionIdAndControlPlandefectId(finalProductInspectionId, controlPlanDefectId);
            data.ResultEntity.Number = palletNumber;
            data.ResultEntity.NewFirstSample = finalProductInspectionDefectResult.ResultEntity.FirstSample ?? 0;
            data.ResultEntity.NewSecondSample = finalProductInspectionDefectResult.ResultEntity.SecondSample ?? 0;
            data.ResultEntity.NewThirdSample = finalProductInspectionDefectResult.ResultEntity.ThirdSample ?? 0;
            data.ResultEntity.NewForthSample = finalProductInspectionDefectResult.ResultEntity.ForthSample ?? 0;

            var downloadurl = configuration.GetSection("Attachment").Get<AttachmentSection>().DownloadUrl;

            foreach (var item in data.ResultEntity.FinalProductNoncomplianceFiles)
            {
                item.ImageSrc = $"{downloadurl}" + item.AttachmentId;
            }

            return PartialView("_AddNonCompliance", data.ResultEntity);
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetNonCompliances(int orderNo, string productCode, int controlPlandefectId, int number, int finalProductInspectionId)
        {

            #region Load _ChooseNonCompliance When There is Related NonCompliance Based On orderNo,productCode,controlPlandefectId

            var viewModel = new ChooseNonComplianceModel();

            var data = finalProductNoncomplianceLogic.GetHasBasisByOrderNoAndProductCodeAndControlPlanDefectId(orderNo, productCode, controlPlandefectId);

            if (data.ResultStatus == OperationResultStatus.Successful && data.ResultEntity is not null && data.ResultEntity.Count > 0)
            {
                var createNewSelectListOption = new SelectListItem { Text = localizer["Save New NonCompliance"], Value = "-1" };
                var nonCompliances = data.ResultEntity.ToSelectList(nameof(FinalProductNoncomplianceModel.FinalProductNoncomplianceNumber), nameof(FinalProductNoncomplianceModel.FinalProductNoncomplianceId));
                var selectListOptions = new List<SelectListItem>
                {
                    createNewSelectListOption
                };
                selectListOptions.AddRange(nonCompliances);
                viewModel.NonCompliancesList = selectListOptions;
                viewModel.ControlPlanDefectId = controlPlandefectId;
                viewModel.Number = number;
                viewModel.FinalProductInspectionId = finalProductInspectionId;
                return PartialView("_ChooseNonCompliance", viewModel);
            }

            #endregion

            #region Load New _AddNonCompliance Partial For First Issue Of NonCompliance

            return ShowNewnonComplianceForm(controlPlandefectId, number, finalProductInspectionId);

            #endregion
        }

        private IActionResult ShowNewnonComplianceForm(int controlPlandefectId, int number, int finalProductInspectionId)
        {
            var finalProductNoncomplianceModel = new FinalProductNoncomplianceModel();

            var serviceInfoResult = finalProductInspectionLogic.GetByFinalProductInspectionId(finalProductInspectionId);

            if (serviceInfoResult.ResultStatus != OperationResultStatus.Successful || serviceInfoResult.ResultEntity is null)
            {
                finalProductNoncomplianceModel.IsEmpty = true;
                finalProductNoncomplianceModel.ErrorMessage = localizer["Pallet Info Not Found"];
                return PartialView("_AddNonCompliance", finalProductNoncomplianceModel);
            }

            var controlPlanDefectResult = controlPlanDefectLogic.GetByControlPlanDefectId(controlPlandefectId);

            if (controlPlanDefectResult.ResultStatus != OperationResultStatus.Successful || controlPlanDefectResult.ResultEntity is null)
            {
                finalProductNoncomplianceModel.IsEmpty = true;
                finalProductNoncomplianceModel.ErrorMessage = localizer["Control Plan Defct Not Found"];
                return PartialView("_AddNonCompliance", finalProductNoncomplianceModel);
            }

            var finalProductInspectionDefectResult = finalProductInspectionDefectLogic.GetByFianalProductInspectionIdAndControlPlandefectId(finalProductInspectionId, controlPlandefectId);

            if (finalProductInspectionDefectResult.ResultStatus != OperationResultStatus.Successful || finalProductInspectionDefectResult.ResultEntity is null)
            {
                finalProductNoncomplianceModel.IsEmpty = true;
                finalProductNoncomplianceModel.ErrorMessage = localizer["Inspection Details Not Found"];
                return PartialView("_AddNonCompliance", finalProductNoncomplianceModel);
            }

            finalProductNoncomplianceModel.IsEmpty = false;
            finalProductNoncomplianceModel.IsNew = true;

            finalProductNoncomplianceModel.OrderNo = serviceInfoResult.ResultEntity.OrderNo;
            finalProductNoncomplianceModel.ProductCode = serviceInfoResult.ResultEntity.ProductCode;
            finalProductNoncomplianceModel.ProductName = serviceInfoResult.ResultEntity.ProductName;
            finalProductNoncomplianceModel.ControlPlanDefectId = controlPlandefectId;
            finalProductNoncomplianceModel.ControlPlanDefectCode = controlPlanDefectResult.ResultEntity.DefectCode;
            finalProductNoncomplianceModel.ControlPlanDefectTitle = controlPlanDefectResult.ResultEntity.DefectTitle;
            finalProductNoncomplianceModel.ControlPlanDefectValue = controlPlanDefectResult.ResultEntity.ControlPlanDefectVal;
            finalProductNoncomplianceModel.Number = number;
            finalProductNoncomplianceModel.NewFirstSample = finalProductInspectionDefectResult.ResultEntity.FirstSample ?? 0;
            finalProductNoncomplianceModel.NewSecondSample = finalProductInspectionDefectResult.ResultEntity.SecondSample ?? 0;
            finalProductNoncomplianceModel.NewThirdSample = finalProductInspectionDefectResult.ResultEntity.ThirdSample ?? 0;
            finalProductNoncomplianceModel.NewForthSample = finalProductInspectionDefectResult.ResultEntity.ForthSample ?? 0;
            finalProductNoncomplianceModel.FinalProductInspectionId = finalProductInspectionId;
            finalProductNoncomplianceModel.ControlPlan = controlPlanDefectResult.ResultEntity.ControlPlanTitle;
            finalProductNoncomplianceModel.OrderTitle = serviceInfoResult.ResultEntity.OrderTitle;

            return PartialView("_AddNonCompliance", finalProductNoncomplianceModel);
        }
    }

}
