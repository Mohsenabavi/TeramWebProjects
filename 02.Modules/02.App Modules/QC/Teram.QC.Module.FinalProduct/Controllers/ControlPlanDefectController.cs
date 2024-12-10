using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Logic;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Teram.Web.Core.Extensions;

namespace Teram.QC.Module.FinalProduct.Controllers
{
    public class ControlPlanDefectController : ControlPanelBaseController<ControlPlanDefectModel, ControlPlanDefect, int>
    {
        private readonly IControlPlanDefectLogic controlPlanDefectLogic;
        private readonly IQCControlPlanLogic controlPlanLogic;
        private readonly IQCDefectLogic defectLogic;

        public ControlPlanDefectController(ILogger<ControlPlanDefectController> logger
            , IStringLocalizer<ControlPlanDefectController> localizer, IControlPlanDefectLogic controlPlanDefectLogic,
            IStringLocalizer<SharedResource> sharedLocalizer, IQCControlPlanLogic controlPlanLogic, IQCDefectLogic defectLogic)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<ControlPlanDefectModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["ControlPlanDefect"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["ControlPlanDefect"],
                OperationColumns = true,
                HomePage = nameof(ControlPlanDefectController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/QC/Module/FinalProduct/Scripts/ControlPlanDefect.js",
            };
            this.controlPlanDefectLogic = controlPlanDefectLogic ?? throw new ArgumentNullException(nameof(controlPlanDefectLogic));
            this.controlPlanLogic=controlPlanLogic??throw new ArgumentNullException(nameof(controlPlanLogic));
            this.defectLogic=defectLogic??throw new ArgumentNullException(nameof(defectLogic));
        }

        [ControlPanelMenu("ControlPlanDefect", ParentName = "BaseInfoManagement", Icon = "fa fa-bug", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            ViewBag.ControlPlans=FillControlPlans();
            ViewBag.Defects=FillDefects();
            return View(Model);
        }

        protected override void ModifyItem(ILogic<ControlPlanDefectModel> service, int id)
        {
            ViewBag.ControlPlans=FillControlPlans();
            ViewBag.Defects=FillDefects();
            base.ModifyItem(service, id);
        }

        private List<SelectListItem> FillControlPlans()
        {
            var result = new List<SelectListItem>();
            var data = controlPlanLogic.GetAll();
            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }
            return data.ResultEntity.ToSelectList(nameof(QCControlPlanModel.Title), nameof(QCControlPlanModel.QCControlPlanId));
        }

        private List<SelectListItem> FillDefects()
        {
            var result = new List<SelectListItem>();
            var data = defectLogic.GetAll();
            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }
            return data.ResultEntity.Select(x => new SelectListItem { Text=x.Code + "-" + x.Title ,Value=x.QCDefectId.ToString() }).ToList();
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult PrintExcel()
        {
            var data = controlPlanDefectLogic.GetAll();

            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[data.AllMessages] });
            }

            var excelData = data.ResultEntity.ExportListExcel("ایرادات طرح های کنترلی");
            if (excelData is null)
            {
                return Json(new { result = "fail", total = 0, rows = new List<FinalProductNoncomplianceModel>(), message = localizer["Unable to create file due to technical problems."] });
            }
            var fileName = "ایرادات طرح های کنترلی-" + DateTime.Now.ToPersianDate();
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + ".xlsx");
        }
    }
}
