using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Linq;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Enums;
using Teram.QC.Module.FinalProduct.Logic;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.QC.Module.FinalProduct.Models.ServiceModels;
using Teram.QC.Module.FinalProduct.Services;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Teram.Web.Core.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Teram.QC.Module.FinalProduct.Controllers
{
    public class FinalProductInspectionController : ControlPanelBaseController<FinalProductInspectionModel, FinalProductInspection, int>
    {
        private readonly IFinalProductNoncomplianceLogic finalProductNoncomplianceLogic;
        private readonly IFinalProductNoncomplianceDetailLogic finalProductNoncomplianceDetailLogic;
        private readonly IFinalProductInspectionLogic finalProductInspectionLogic;
        private readonly IControlPlanDefectLogic controlPlanDefectLogic;
        private readonly IUserSharedService userSharedService;
        private readonly IQueryService queryService;
        private readonly IQCControlPlanLogic qCControlPlanLogic;
        private readonly IAcceptancePeriodLogic acceptancePeriodLogic;

        public FinalProductInspectionController(ILogger<FinalProductInspectionController> logger, IFinalProductNoncomplianceLogic finalProductNoncomplianceLogic,
            IFinalProductNoncomplianceDetailLogic finalProductNoncomplianceDetailLogic, IFinalProductInspectionLogic finalProductInspectionLogic
            , IStringLocalizer<FinalProductInspectionController> localizer, IControlPlanDefectLogic controlPlanDefectLogic, IUserSharedService userSharedService,
            IStringLocalizer<SharedResource> sharedLocalizer, IQueryService queryService, IQCControlPlanLogic qCControlPlanLogic, IAcceptancePeriodLogic acceptancePeriodLogic)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;


            Model = new ViewInformation<FinalProductInspectionModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["FinalProductInspectionForm"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["FinalProductInspectionForm"],
                OperationColumns = true,
                HomePage = nameof(FinalProductInspectionController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/QC/Module/FinalProduct/Scripts/FinalProductInspection.js",
                GetDataUrl = "",
                LoadAjaxData = false,
            };
            this.finalProductNoncomplianceLogic = finalProductNoncomplianceLogic ?? throw new ArgumentNullException(nameof(finalProductNoncomplianceLogic));
            this.finalProductNoncomplianceDetailLogic = finalProductNoncomplianceDetailLogic ?? throw new ArgumentNullException(nameof(finalProductNoncomplianceDetailLogic));
            this.finalProductInspectionLogic = finalProductInspectionLogic ?? throw new ArgumentNullException(nameof(finalProductInspectionLogic));
            this.controlPlanDefectLogic = controlPlanDefectLogic ?? throw new ArgumentNullException(nameof(controlPlanDefectLogic));
            this.userSharedService = userSharedService ?? throw new ArgumentNullException(nameof(userSharedService));
            this.queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
            this.qCControlPlanLogic = qCControlPlanLogic ?? throw new ArgumentNullException(nameof(qCControlPlanLogic));
            this.acceptancePeriodLogic = acceptancePeriodLogic ?? throw new ArgumentNullException(nameof(acceptancePeriodLogic));
        }

        [ControlPanelMenu("FinalProductInspectionForm", ParentName = "FinalProductInspection", Icon = "fa fa-check-circle", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }


        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetFinalProductInspectionGridData(DatatablesSentModel model, int orderNo, int number, string productCode, string tracingCode, string orderTitle, string productName)
        {

            var finalProductInspectionsResult = finalProductInspectionLogic.GetByFilter(orderNo, number, productCode, tracingCode, orderTitle, productName, model.Start, model.Length);

            if (finalProductInspectionsResult.ResultStatus != OperationResultStatus.Successful || finalProductInspectionsResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[finalProductInspectionsResult.AllMessages] });
            }

            var relatedFinalProductInspectionIds = finalProductInspectionsResult.ResultEntity.Select(x => x.FinalProductInspectionId).ToList();
            var finalProductNonComplianceDetails = finalProductNoncomplianceDetailLogic.GetByFinalProductInspectionIds(relatedFinalProductInspectionIds);
            var createdByUserIds = finalProductInspectionsResult.ResultEntity.Select(x => x.CreatedBy).ToList();
            var usersInfo = userSharedService.GetUserInfos(createdByUserIds);

            foreach (var item in finalProductInspectionsResult.ResultEntity)
            {
                var relatedUser = usersInfo.Where(x => x.UserId == item.CreatedBy).FirstOrDefault();
                item.CreatedByText = (relatedUser != null) ? $"{relatedUser.Name} {relatedUser.Family} - {relatedUser.Username} " : " ";
                var relatednoncompianceDetail = finalProductNonComplianceDetails.ResultEntity.Where(x => x.FinalProductInspectionId == item.FinalProductInspectionId).ToList();
                item.HasNonCompliance = (relatednoncompianceDetail.Any()) ? true : false;              
            }
            var totalCount = finalProductInspectionsResult?.Count ?? 0;
            return Json(new { model.Draw, recordsTotal = totalCount, recordsFiltered = totalCount, data = finalProductInspectionsResult?.ResultEntity, error = "", result = "ok" });
        }

        public async Task<IActionResult> PrintExcel(int orderNo, int number, string productCode, string tracingCode, string orderTitle, string productName)
        {
            var finalProductInspectionsResult = finalProductInspectionLogic.GetByFilter(orderNo, number, productCode, tracingCode, orderTitle, productName);

            if (finalProductInspectionsResult.ResultStatus != OperationResultStatus.Successful || finalProductInspectionsResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[finalProductInspectionsResult.AllMessages] });
            }

            var relatedFinalProductInspectionIds = finalProductInspectionsResult.ResultEntity.Select(x => x.FinalProductInspectionId).ToList();
            var finalProductNonComplianceDetails = finalProductNoncomplianceDetailLogic.GetByFinalProductInspectionIds(relatedFinalProductInspectionIds);
            var createdByUserIds = finalProductInspectionsResult.ResultEntity.Select(x => x.CreatedBy).ToList();
            var usersInfo = userSharedService.GetUserInfos(createdByUserIds);

            foreach (var item in finalProductInspectionsResult.ResultEntity)
            {
                var relatedUser = usersInfo.Where(x => x.UserId == item.CreatedBy).FirstOrDefault();
                item.CreatedByText = (relatedUser != null) ? $"{relatedUser.Name} {relatedUser.Family} - {relatedUser.Username} " : " ";
                var relatednoncompianceDetail = finalProductNonComplianceDetails.ResultEntity.Where(x => x.FinalProductInspectionId == item.FinalProductInspectionId).ToList();
                item.HasNonCompliance = (relatednoncompianceDetail.Any()) ? true : false;
            }
           
            var excelData = finalProductInspectionsResult.ResultEntity.ExportListExcel("فرم های بازرسی محصول نهایی");
            if (excelData is null)
            {
                return Json(new { result = "fail", total = 0, rows = new List<FinalProductNoncomplianceModel>(), message = localizer["Unable to create file due to technical problems."] });
            }

            var fileName = "فرم های بازرسی محصول نهایی-" + DateTime.Now.ToPersianDate();
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + ".xlsx");
        }

        public override IActionResult Remove([FromServices] ILogic<FinalProductInspectionModel> service, int key)
        {
            var hasApprovedNonCompiance = false;

            var relatedNonCompliancesResult = finalProductNoncomplianceDetailLogic.GetByFinalProductInspectionId(key);

            var relatedNonComplianceIds = relatedNonCompliancesResult.ResultEntity.Select(x => x.FinalProductNoncomplianceId).ToList();

            var nonComplianceResult = finalProductNoncomplianceLogic.GetByIds(relatedNonComplianceIds);

            if (relatedNonCompliancesResult.ResultStatus == OperationResultStatus.Successful || relatedNonCompliancesResult.ResultEntity is not null)
            {
                foreach (var item in relatedNonCompliancesResult.ResultEntity)
                {

                    var relatedNonCompianceResult = nonComplianceResult.ResultEntity.Where(x => x.FinalProductNoncomplianceId == item.FinalProductNoncomplianceId).FirstOrDefault();

                    hasApprovedNonCompiance = (relatedNonCompianceResult != null && relatedNonCompianceResult.IsApproved.HasValue && relatedNonCompianceResult.IsApproved.Value);

                    if (!hasApprovedNonCompiance)
                    {
                        var deleteResult = finalProductNoncomplianceDetailLogic.Delete(item.Key);
                    }
                }
            }

            if (hasApprovedNonCompiance)
            {
                return Json(new { result = "fail", message = localizer["Delete Is Not Permitted"] });
            }

            return base.Remove(service, key);
        }

        protected override void ModifyItem(ILogic<FinalProductInspectionModel> service, int id)
        {
            if (id > 0)
            {
                Model.ModelData = new FinalProductInspectionModel();

                var finalProductInspectionResult = finalProductInspectionLogic.GetByFinalProductInspectionId(id);
                if (finalProductInspectionResult.ResultStatus == OperationResultStatus.Successful && finalProductInspectionResult.ResultEntity is not null)
                {

                    var registeredDataControlPlanDefectIds = finalProductInspectionResult.ResultEntity.FinalProductInspectionDefects.Select(x => x.ControlPlanDefectId).ToList();
                    var registeredNonCompliances = finalProductNoncomplianceLogic.GetByControlPlanDefectIdsAndFinalProductInspectionId(registeredDataControlPlanDefectIds, finalProductInspectionResult.ResultEntity.FinalProductInspectionId);

                    var registeredNonCompliancesDetails = registeredNonCompliances.ResultEntity.SelectMany(x => x.FinalProductNoncomplianceDetails).ToList();

                    foreach (var item in finalProductInspectionResult.ResultEntity.FinalProductInspectionDefects)
                    {
                        bool isApproved = false;

                        var relatedNonCompliances = registeredNonCompliancesDetails.Where(x => x.FinalProductNoncomplianceControlPlanDefectId == item.ControlPlanDefectId && x.FinalProductInspectionId == item.FinalProductInspectionId).ToList();

                        foreach (var relatedNonCompliance in relatedNonCompliances)
                        {
                            isApproved = relatedNonCompliance.FinalProductNoncomplianceIsApproved;
                        }
                        var noncomplianceNumbers = relatedNonCompliances.Select(x => x.FinalProductNoncomplianceFinalProductNoncomplianceNumber).ToList();
                        item.FinalProductNoncomplianceNumbers = string.Join("-", noncomplianceNumbers);
                        item.IsLocked = isApproved;
                    }
                    ViewData["DefectList"] = GetDefects(finalProductInspectionResult.ResultEntity.ControlPlan);
                    ViewBag.Defects = GetDefects(finalProductInspectionResult.ResultEntity.ControlPlan);
                    Model.ModelData = finalProductInspectionResult.ResultEntity;
                }
            }
            Model.Key = id;
        }

        public IActionResult SaveForm([FromServices] IFinalProductInspectionLogic service, FinalProductInspectionModel model)
        {

            var isExceeding = false;
            var isZeroSampleCount = false;
            var hasNoDefect = false;

            if (model.FinalProductInspectionDefects != null && model.FinalProductInspectionDefects.Count > 0)
            {

                foreach (var item in model.FinalProductInspectionDefects)
                {
                    var sumOfSamples = 0;
                    sumOfSamples = (item.FirstSample ?? 0) + (item.SecondSample ?? 0) + (item.ThirdSample ?? 0) + (item.ForthSample ?? 0);
                    if (sumOfSamples > model.SampleCount)
                    {
                        isExceeding = true;
                    }

                    if (sumOfSamples == 0)
                    {
                        isZeroSampleCount = true;
                    }
                }
                if (isZeroSampleCount)
                {
                    return Json(new { result = "fail", message = localizer["Samples Amount Are Zero"] });
                }
                if (isExceeding)
                {
                    return Json(new { result = "fail", message = localizer["Exceeding the allowed amount"] });
                }

                var duplicates = model.FinalProductInspectionDefects
                            .GroupBy(defect => defect.ControlPlanDefectId)
                            .Where(group => group.Count() > 1)
                            .Select(group => group.Key).ToList();

                if (duplicates.Count != 0)
                {
                    return Json(new { result = "fail", message = localizer["Duplicate Values In ControlPlan Defects"] });
                }
            }
            else
            {
                hasNoDefect = true;
            }

            if (model.FinalProductInspectionId == 0)
            {
                var checkForExistResult = finalProductInspectionLogic.GetByPalletNumber(model.Number);

                if (checkForExistResult.ResultStatus == OperationResultStatus.Successful && checkForExistResult.ResultEntity is not null)
                {
                    return Json(new { result = "fail", message = localizer["Pallet Number Is Duplicate"] });
                }

                var addResult = ((FinalProductInspectionLogic)service).AddNew(model);
                if (addResult.ResultStatus != OperationResultStatus.Successful || addResult.ResultEntity == null)
                {
                    return Json(new { result = "fail", data = addResult.AllMessages, message = addResult.AllMessages });
                }
                return Json(new { result = "ok", message = sharedLocalizer["Your data has been saved"], id = addResult.ResultEntity.FinalProductInspectionId, hasNoDefect = hasNoDefect.ToString() });
            }
            else
            {
                var existingRecord = finalProductInspectionLogic.GetRow(model.FinalProductInspectionId);
                if (existingRecord.ResultEntity.Number != model.Number) {

                    return Json(new { result = "fail", data = existingRecord.AllMessages, message = localizer["Number Cannot be edited"], id = model.FinalProductInspectionId });
                }
                var editResult = finalProductInspectionLogic.Update(model);
                if (editResult.ResultStatus != OperationResultStatus.Successful)
                {
                    return Json(new { result = "fail", data = editResult.AllMessages, message = editResult.AllMessages, id = model.FinalProductInspectionId });
                }

                return Json(new { result = "ok", message = sharedLocalizer["Your data has been saved"], id = model.FinalProductInspectionId, hasNoDefect = hasNoDefect.ToString() });
            }
        }

        public List<SelectListItem> GetDefects(string controlPlan)
        {

            var result = new List<SelectListItem>();

            var relatedControlPlan = qCControlPlanLogic.GetByTitle(controlPlan);

            var relatedControlPlanDefects = controlPlanDefectLogic.GetByControlPlanId(relatedControlPlan.ResultEntity.QCControlPlanId);

            return relatedControlPlanDefects.ResultEntity.Select(x => new SelectListItem
            {
                Text = x.DefectCode + "-" + x.DefectTitle,
                Value = x.ControlPlanDefectId.ToString()
            }).ToList();
        }

        [ParentalAuthorize(nameof(Index))]

        public async Task<IActionResult> FetchPalletInfo(int palletNo)
        {

            Model.ModelData = new FinalProductInspectionModel();

            var result = await queryService.GetPalletInfo(palletNo);

            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity is null)
            {
                return Json(new { result = "fail", content = new PalletInfoModel() });
            }

            var relatedControlPlanResult = qCControlPlanLogic.GetByTitle(result.ResultEntity.ControlPlan);

            if (relatedControlPlanResult.ResultStatus != OperationResultStatus.Successful || relatedControlPlanResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer["Control Plan Information Not Found"] + ":" + result.ResultEntity.ControlPlan });
            }

            var relatedAccetancePeriodResult = acceptancePeriodLogic.GetByContrplPlanIdAndPeriod(relatedControlPlanResult.ResultEntity.QCControlPlanId, result.ResultEntity.Quantity);

            if (relatedAccetancePeriodResult.ResultStatus != OperationResultStatus.Successful || relatedAccetancePeriodResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer["Accetance Period Information Not Found"] + ":" + result.ResultEntity.ControlPlan });
            }

            result.ResultEntity.StartInterval = relatedAccetancePeriodResult.ResultEntity.StartInterval;
            result.ResultEntity.EndInterval = relatedAccetancePeriodResult.ResultEntity.EndInterval;
            result.ResultEntity.SampleCount = relatedAccetancePeriodResult.ResultEntity.SampleCount;

            Model.ModelData.ControlPlan = result.ResultEntity.ControlPlan;

            return Json(new
            {
                result = "ok",
                content = result.ResultEntity
            });
        }
    }

}
