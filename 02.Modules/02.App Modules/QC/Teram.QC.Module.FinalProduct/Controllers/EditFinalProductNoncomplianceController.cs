using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using MongoDB.Driver;
using System.Transactions;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Module.AttachmentsManagement.Models;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Enums;
using Teram.QC.Module.FinalProduct.Logic;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.QC.Module.FinalProduct.Models.CausationModels;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Teram.Web.Core.Extensions;
using Teram.Web.Core.Model;
using Teram.Web.Core.Security;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Teram.QC.Module.FinalProduct.Controllers
{
    public class EditFinalProductNoncomplianceController : ControlPanelBaseController<FinalProductNoncomplianceModel, FinalProductNoncompliance, int>
    {
        private readonly IFinalProductNoncomplianceLogic finalProductNoncomplianceLogic;
        private readonly IFinalProductNoncomplianceFileLogic finalProductNoncomplianceFileLogic;
        private readonly IQCControlPlanLogic qCControlPlanLogic;
        private readonly IControlPlanDefectLogic controlPlanDefectLogic;
        private readonly IUserPrincipal userPrincipal;
        private readonly IManageCartableLogic manageCartableLogic;
        private readonly IFinalProductNoncomplianceDetailLogic finalProductNoncomplianceDetailLogic;
        private readonly IUserSharedService userSharedService;
        private readonly IFinalProductInspectionDefectLogic finalProductInspectionDefectLogic;
        private readonly IFinalProductInspectionLogic finalProductInspectionLogic;
        private readonly IInstructionLogic instructionLogic;
        private readonly IMachineLogic machineLogic;
        private readonly IOperatorLogic operatorLogic;
        private readonly IActionerLogic actionerLogic;
        private readonly IRootCauseLogic rootCauseLogic;
        private readonly IUnitLogic unitLogic;
        private readonly IWorkStationLogic workStationLogic;
        private readonly IRawMaterialLogic rawMaterialLogic;

        public EditFinalProductNoncomplianceController(ILogger<EditFinalProductNoncomplianceController> logger
            , IStringLocalizer<EditFinalProductNoncomplianceController> localizer, IFinalProductNoncomplianceLogic finalProductNoncomplianceLogic, IFinalProductNoncomplianceFileLogic finalProductNoncomplianceFileLogic,
            IQCControlPlanLogic qCControlPlanLogic, IControlPlanDefectLogic controlPlanDefectLogic, IUserPrincipal userPrincipal, IManageCartableLogic manageCartableLogic,
            IFinalProductNoncomplianceDetailLogic finalProductNoncomplianceDetailLogic, IUserSharedService userSharedService,
            IFinalProductInspectionDefectLogic finalProductInspectionDefectLogic, IFinalProductInspectionLogic finalProductInspectionLogic,
            IInstructionLogic instructionLogic, IMachineLogic machineLogic, IOperatorLogic operatorLogic, IActionerLogic actionerLogic,
            IRootCauseLogic rootCauseLogic, IUnitLogic unitLogic, IWorkStationLogic workStationLogic, IRawMaterialLogic rawMaterialLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<FinalProductNoncomplianceModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["EditFinalProductNoncompliance"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["EditFinalProductNoncompliance"],
                OperationColumns = true,
                HomePage = nameof(EditFinalProductNoncomplianceController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/QC/Module/FinalProduct/Scripts/EditFinalProductNoncompliance.js",
                GridId = "EditFinalProductNoncomplianceGrid",
                GetDataUrl = "",
                LoadAjaxData = false,
            };
            this.finalProductNoncomplianceLogic = finalProductNoncomplianceLogic ?? throw new ArgumentNullException(nameof(finalProductNoncomplianceLogic));
            this.finalProductNoncomplianceFileLogic = finalProductNoncomplianceFileLogic ?? throw new ArgumentNullException(nameof(finalProductNoncomplianceFileLogic));
            this.qCControlPlanLogic = qCControlPlanLogic ?? throw new ArgumentNullException(nameof(qCControlPlanLogic));
            this.controlPlanDefectLogic = controlPlanDefectLogic ?? throw new ArgumentNullException(nameof(controlPlanDefectLogic));
            this.userPrincipal = userPrincipal ?? throw new ArgumentNullException(nameof(userPrincipal));
            this.manageCartableLogic = manageCartableLogic ?? throw new ArgumentNullException(nameof(manageCartableLogic));
            this.finalProductNoncomplianceDetailLogic = finalProductNoncomplianceDetailLogic ?? throw new ArgumentNullException(nameof(finalProductNoncomplianceDetailLogic));
            this.userSharedService = userSharedService ?? throw new ArgumentNullException(nameof(userSharedService));
            this.finalProductInspectionDefectLogic = finalProductInspectionDefectLogic ?? throw new ArgumentNullException(nameof(finalProductInspectionDefectLogic));
            this.finalProductInspectionLogic = finalProductInspectionLogic ?? throw new ArgumentNullException(nameof(finalProductInspectionLogic));
            this.instructionLogic = instructionLogic ?? throw new ArgumentNullException(nameof(instructionLogic));
            this.machineLogic = machineLogic ?? throw new ArgumentNullException(nameof(machineLogic));
            this.operatorLogic = operatorLogic ?? throw new ArgumentNullException(nameof(operatorLogic));
            this.actionerLogic = actionerLogic ?? throw new ArgumentNullException(nameof(actionerLogic));
            this.rootCauseLogic = rootCauseLogic ?? throw new ArgumentNullException(nameof(rootCauseLogic));
            this.unitLogic = unitLogic;
            this.workStationLogic = workStationLogic;
            this.rawMaterialLogic = rawMaterialLogic ?? throw new ArgumentNullException(nameof(rawMaterialLogic));
        }

        [ControlPanelMenu("EditFinalProductNoncompliance", ParentName = "FinalProductInspection", Icon = "fa fa-ban", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }

        public IActionResult ViewAllPermission()
        {
            return Content("This is Not Actual Action");
        }

        public async Task<List<SelectListItem>> GetNeedToAdvisoryOpinionRefferralList()
        {

            var result = new List<SelectListItem>();

            var relatedUsers = await userSharedService.GetUsersInRole("Sales");

            if (relatedUsers != null)
            {
                return relatedUsers.Select(x => new SelectListItem { Text = string.Concat(x.Name, "-", x.Family), Value = x.UserId.ToString() }).ToList();
            }
            return result;
        }


        public async Task<List<SelectListItem>> GetOthersListForAdvisoryOpinion()
        {

            var result = new List<SelectListItem>();

            var relatedUsers = await userSharedService.GetUsersInRole("Reviewer");

            if (relatedUsers != null)
            {
                return relatedUsers.Select(x => new SelectListItem { Text = string.Concat(x.Name, "-", x.Family), Value = x.UserId.ToString() }).ToList();
            }
            return result;
        }

        public IActionResult GetNonComplianceDetails(int finalProductNoncomplianceId)
        {
            var nonComplianceResult = finalProductNoncomplianceLogic.GetById(finalProductNoncomplianceId);


            var finalProductInspectionResult = finalProductInspectionLogic.GetByOrderNoAndProductCode(nonComplianceResult.ResultEntity.OrderNo, nonComplianceResult.ResultEntity.ProductCode);

            ViewBag.RelatedDefects = GetRelatedDefects(nonComplianceResult.ResultEntity.FinalProductNoncomplianceId);
            ViewBag.Instructions = GetInstructions();
            ViewBag.Machines = GetMachines();
            ViewBag.Operators = GetOperators();
            ViewBag.RoorCauses = GetRootCauses();
            ViewBag.Units = GetUnits();
            ViewBag.WorkStations = GetWorkStations();
            ViewBag.Actioners = GetActioners();
            ViewBag.RawMaterials = GetRawMaterials();
            var mainRole = manageCartableLogic.GetUserMainRole();
            if (mainRole != null && mainRole.ResultEntity.Name.ToUpper() == "OPERATOR")
            {
                ViewBag.IsOperator = true;
            }
            else
            {
                ViewBag.IsOperator = false;
            }
            return PartialView("_NonCompliaceDetail", finalProductNoncomplianceLogic.GetNonComplianceDetails(nonComplianceResult.ResultEntity, finalProductInspectionResult.ResultEntity));
        }

        public List<SelectListItem> GetActioners()
        {

            var result = new List<SelectListItem>();
            var data = actionerLogic.GetAll();

            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }
            return data.ResultEntity.Select(x => new SelectListItem
            {
                Text = $"{x.FirstName} {x.LastName} - {x.PostTitle} - {x.PersonnelCode}",
                Value = x.ActionerId.ToString(),
            }).ToList();
        }
        private List<SelectListItem> GetInstructions()
        {

            var result = new List<SelectListItem>();
            var instructionsResult = instructionLogic.GetAll();
            if (instructionsResult.ResultStatus != OperationResultStatus.Successful || instructionsResult.ResultEntity is null)
            {
                return result;
            }
            return instructionsResult.ResultEntity.ToSelectList(nameof(InstructionModel.Title), nameof(InstructionModel.InstructionId));
        }

        private List<SelectListItem> GetMachines()
        {

            var result = new List<SelectListItem>();
            var machinesResult = machineLogic.GetActives();
            if (machinesResult.ResultStatus != OperationResultStatus.Successful || machinesResult.ResultEntity is null)
            {
                return result;
            }
            return machinesResult.ResultEntity.ToSelectList(nameof(MachineModel.Title), nameof(MachineModel.MachineId));
        }
        private List<SelectListItem> GetOperators()
        {

            var result = new List<SelectListItem>();
            var operatorResult = operatorLogic.GetActives();
            if (operatorResult.ResultStatus != OperationResultStatus.Successful || operatorResult.ResultEntity is null)
            {
                return result;
            }
            return operatorResult.ResultEntity.Select(x => new SelectListItem
            {
                Text = $"{x.FirstName} {x.LastName} - {x.PersonnelCode}",
                Value = x.OperatorId.ToString()
            }).ToList();
        }

        private List<SelectListItem> GetRootCauses()
        {

            var result = new List<SelectListItem>();
            var rootCausesResult = rootCauseLogic.GetActives();
            if (rootCausesResult.ResultStatus != OperationResultStatus.Successful || rootCausesResult.ResultEntity is null)
            {
                return result;
            }
            return rootCausesResult.ResultEntity.ToSelectList(nameof(RootCauseModel.Title), nameof(RootCauseModel.RootCauseId));
        }

        private List<SelectListItem> GetUnits()
        {

            var result = new List<SelectListItem>();
            var unitsResult = unitLogic.GetActives();
            if (unitsResult.ResultStatus != OperationResultStatus.Successful || unitsResult.ResultEntity is null)
            {
                return result;
            }
            return unitsResult.ResultEntity.ToSelectList(nameof(UnitModel.Title), nameof(UnitModel.UnitId));
        }

        private List<SelectListItem> GetWorkStations()
        {

            var result = new List<SelectListItem>();
            var workStationsResult = workStationLogic.GetActives();
            if (workStationsResult.ResultStatus != OperationResultStatus.Successful || workStationsResult.ResultEntity is null)
            {
                return result;
            }
            return workStationsResult.ResultEntity.ToSelectList(nameof(WorkStationModel.Title), nameof(WorkStationModel.WorkStationId));
        }

        private List<SelectListItem> GetRawMaterials()
        {

            var result = new List<SelectListItem>();
            var rawMateriasResult = rawMaterialLogic.GetAll();
            if (rawMateriasResult.ResultStatus != OperationResultStatus.Successful || rawMateriasResult.ResultEntity is null)
            {
                return result;
            }
            return rawMateriasResult.ResultEntity.ToSelectList(nameof(RawMaterialModel.Title), nameof(RawMaterialModel.RawMaterialId));
        }

        private List<SelectListItem> GetRelatedDefects(int finalProductNonComplianceId)
        {

            var relatedFinalProductNoncompliance = finalProductNoncomplianceLogic.GetById(finalProductNonComplianceId);

            var relatedControlPlan = qCControlPlanLogic.GetByTitle(relatedFinalProductNoncompliance.ResultEntity.ControlPlan);

            var relatedControlPlanDefects = controlPlanDefectLogic.GetByControlPlanId(relatedControlPlan.ResultEntity.QCControlPlanId);

            return relatedControlPlanDefects.ResultEntity.Select(x => new SelectListItem
            {
                Text = string.Concat(x.DefectCode, "-", x.DefectTitle),
                Value = x.ControlPlanDefectId.ToString(),

            }).ToList();
        }

        protected override void ModifyItem(ILogic<FinalProductNoncomplianceModel> service, int id)
        {
            ViewBag.ControlPlanDefects = GetRelatedDefects(id);
            base.ModifyItem(service, id);
        }

        public override IActionResult Save([FromServices] ILogic<FinalProductNoncomplianceModel> service, FinalProductNoncomplianceModel model)
        {
            var finalProductNoncomplianceResult = finalProductNoncomplianceLogic.GetById(model.FinalProductNoncomplianceId);

            if (finalProductNoncomplianceResult.ResultStatus != OperationResultStatus.Successful || finalProductNoncomplianceResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[finalProductNoncomplianceResult.AllMessages] });
            }

            var nonCompiancesDetails = finalProductNoncomplianceResult.ResultEntity.FinalProductNoncomplianceDetails;
            var FinalProductInspectionIds = nonCompiancesDetails.Where(x => x.FinalProductInspectionId.HasValue).Select(x => x.FinalProductInspectionId.Value).ToList();
            using var transaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            if (nonCompiancesDetails != null && FinalProductInspectionIds.Count > 0)
            {

                var finalProductInspectionDefects = finalProductInspectionDefectLogic.GetByIdsAndControlPlanDefectId(FinalProductInspectionIds, finalProductNoncomplianceResult.ResultEntity.ControlPlanDefectId);
                var registeredFinalProductInspectionDefects = finalProductInspectionDefectLogic.GetByIdsAndControlPlanDefectId(FinalProductInspectionIds, model.ControlPlanDefectId);
                var checkForExist = registeredFinalProductInspectionDefects.ResultEntity.Where(x => x.ControlPlanDefectId == model.ControlPlanDefectId).FirstOrDefault();
                if (checkForExist == null)
                {


                    foreach (var finalProductInspectionDefect in finalProductInspectionDefects.ResultEntity)
                    {
                        finalProductInspectionDefect.ControlPlanDefectId = model.ControlPlanDefectId;
                        finalProductInspectionDefectLogic.Update(finalProductInspectionDefect);
                    }
                }
                else
                {

                    return Json(new { result = "fail", message = localizer["By Changing Control Plan Defect Duplicate Values Will Be Created"] });
                }
            }
            finalProductNoncomplianceResult.ResultEntity.ControlPlanDefectId = model.ControlPlanDefectId;
            finalProductNoncomplianceResult.ResultEntity.FormStatus = FormStatus.ModifiedByQCSupervisor;
            finalProductNoncomplianceResult.ResultEntity.IsApproved = false;
            finalProductNoncomplianceResult.ResultEntity.LastComment = null;
            finalProductNoncomplianceResult.ResultEntity.IsTriggeredByUserAction = true;
            finalProductNoncomplianceLogic.Update(finalProductNoncomplianceResult.ResultEntity);
            transaction.Complete();
            return Json(new { result = "ok", message = localizer["Defect Changed In NonCompliances And Related Inspection Forms"] });
        }

        [ParentalAuthorize(nameof(Index))]
        public async Task<IActionResult> GetNonComplianceData(DatatablesSentModel model, bool viewAll, int? orderNo, string? productCode, string finalProductNoncomplianceNumber, FormStatus? formStatus, ReferralStatus? referralStatus)
        {
            var isOperator = false;
            var userMainRole = manageCartableLogic.GetUserMainRole();

            if (userMainRole.ResultEntity != null && userMainRole.ResultEntity.Name.ToUpper() == "OPERATOR")
            {
                isOperator = true;
            }
            var isAdmin = (userPrincipal.CurrentUser.HasClaim("Permission", ":EditFinalProductNoncompliance:ViewAllPermission") && viewAll);

            var data = finalProductNoncomplianceLogic.GetByFilter(isOperator, isAdmin, orderNo, productCode, finalProductNoncomplianceNumber, formStatus, referralStatus, model.Start, model.Length);

            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[data.AllMessages] });
            }

            var createdbyUserIds = data.ResultEntity.Select(x => x.CreatedBy).ToList();

            var usersInfo = await userSharedService.GetUsers(createdbyUserIds);

            foreach (var item in data.ResultEntity)
            {
                var relatedUserInfo = usersInfo.FirstOrDefault(x => x.UserId == item.CreatedBy);
                item.CreatedByText = (relatedUserInfo != null) ? $"{relatedUserInfo.Name} {relatedUserInfo.Family} - {relatedUserInfo.Username}" : "-";
            }

            var totalCount = data?.Count ?? 0;
            return Json(new { model.Draw, recordsTotal = totalCount, recordsFiltered = totalCount, data = data?.ResultEntity, error = "", result = "ok" });
        }

        public async Task<IActionResult> PrintExcel(bool viewAll, int? orderNo, string? productCode, string finalProductNoncomplianceNumber, FormStatus? formStatus, ReferralStatus? referralStatus)
        {
            var isAdmin = (userPrincipal.CurrentUser.HasClaim("Permission", ":EditFinalProductNoncompliance:ViewAllPermission") && viewAll);

            var data = finalProductNoncomplianceLogic.GetByFilter(false, isAdmin, orderNo, productCode, finalProductNoncomplianceNumber, formStatus, referralStatus);

            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[data.AllMessages] });
            }

            var createdbyUserIds = data.ResultEntity.Select(x => x.CreatedBy).ToList();

            var usersInfo = await userSharedService.GetUsers(createdbyUserIds);

            foreach (var item in data.ResultEntity)
            {
                var relatedUserInfo = usersInfo.FirstOrDefault(x => x.UserId == item.CreatedBy);
                item.CreatedByText = (relatedUserInfo != null) ? $"{relatedUserInfo.Name} {relatedUserInfo.Family} - {relatedUserInfo.Username}" : "-";
            }

            var excelData = data.ResultEntity.ExportListExcel("عدم انطباق محصول نهایی");
            if (excelData is null)
            {
                return Json(new { result = "fail", total = 0, rows = new List<FinalProductNoncomplianceModel>(), message = localizer["Unable to create file due to technical problems."] });
            }

            var fileName = "عدم انطباق محصول نهایی-" + DateTime.Now.ToPersianDate();
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + ".xlsx");

        }
    }

}
