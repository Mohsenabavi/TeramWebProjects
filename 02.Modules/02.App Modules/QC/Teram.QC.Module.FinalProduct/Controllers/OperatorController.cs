using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Logic;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.QC.Module.FinalProduct.Models.CausationModels;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.QC.Module.FinalProduct.Controllers
{
    public class OperatorController : ControlPanelBaseController<OperatorModel, Operator, int>
    {
        private readonly IOperatorLogic operatorLogic;

        public OperatorController(ILogger<OperatorController> logger
            , IStringLocalizer<OperatorController> localizer, IOperatorLogic operatorLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<OperatorModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["Operators"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["Operators"],
                OperationColumns = true,
                HomePage = nameof(OperatorController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/QC/Module/FinalProduct/Scripts/Operator.js",
            };
            this.operatorLogic = operatorLogic ?? throw new ArgumentNullException(nameof(operatorLogic));
        }

        [ControlPanelMenu("Operators", ParentName = "BaseInfoManagement", Icon = "fa fa-users-cog", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult PrintExcel()
        {
            var data = operatorLogic.GetAll();

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
