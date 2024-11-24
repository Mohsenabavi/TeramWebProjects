using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models.CausationModels;
using Teram.QC.Module.FinalProduct.Models.ReportsModels;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Teram.Web.Core.Extensions;
using Teram.Web.Core.Model;

namespace Teram.QC.Module.FinalProduct.Controllers
{
    public class WronDoersReportController : ControlPanelBaseController<CausationModel, Causation, int>
    {
        private readonly IOperatorLogic operatorLogic;
        private readonly ICausationLogic causationLogic;

        public WronDoersReportController(ILogger<WronDoersReportController> logger, IOperatorLogic operatorLogic
            , IStringLocalizer<WronDoersReportController> localizer, ICausationLogic causationLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<WrongDoersListReportModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["WronDoersReport"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["WronDoersReport"],
                OperationColumns = true,
                HomePage = nameof(WronDoersReportController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/QC/Module/FinalProduct/Scripts/WronDoersReport.js",
                GetDataUrl = "",
                LoadAjaxData = false,
            };
            this.operatorLogic = operatorLogic ?? throw new ArgumentNullException(nameof(operatorLogic));
            this.causationLogic = causationLogic ?? throw new ArgumentNullException(nameof(causationLogic));
        }

        [ControlPanelMenu("WronDoersReport", ParentName = "FinalProductInspection", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]

        public IActionResult Index()
        {
            ViewBag.Operators = GetOperators();
            return View(Model);
        }
        private List<SelectListItem> GetOperators()
        {

            var result = new List<SelectListItem>();

            var data = operatorLogic.GetActives();

            return data.ResultEntity.Select(x => new SelectListItem
            {
                Text = string.Concat(x.FirstName, ' ', x.LastName),
                Value = x.OperatorId.ToString()

            }).ToList();

        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetWrongDoersData(
           DatatablesSentModel model, int? wrongDoerId = null,
           DateTime? statInputDate = null,
           DateTime? endInputDate = null)
        {
            var (Items, TotalCount) = causationLogic.GetWrongDoerReportData(wrongDoerId, statInputDate, endInputDate, model.Start, model.Length).Result;
            var totalCount = TotalCount;
            var result = Items;
            return Json(new { model.Draw, recordsTotal = totalCount, recordsFiltered = totalCount, data = result, error = "", result = "ok" });
        }

        public IActionResult PrintExcel(int? wrongDoerId = null,
           DateTime? statInputDate = null,
           DateTime? endInputDate = null)
        {
            var (Items, TotalCount) = causationLogic.GetWrongDoerReportData(wrongDoerId, statInputDate, endInputDate).Result;            
            var excelData = Items.ExportListExcel("گزارش افراد خاطی");
            if (excelData is null)
            {
                return Json(new { result = "fail", total = 0, rows = new List<WrongDoersListReportModel>(), message = localizer["Unable to create file due to technical problems."] });
            }

            var fileName = "گزارش افراد خاطی-" + DateTime.Now.ToPersianDate();
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + ".xlsx");

        }
    }
}
