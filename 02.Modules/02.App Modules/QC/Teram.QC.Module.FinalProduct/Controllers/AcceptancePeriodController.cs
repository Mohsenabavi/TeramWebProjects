using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Logic;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.QC.Module.FinalProduct.Models.ImportModels;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Teram.Web.Core.Extensions;
using Teram.Web.Core.Model;

namespace Teram.QC.Module.FinalProduct.Controllers
{
    public class AcceptancePeriodController : ControlPanelBaseController<AcceptancePeriodModel, AcceptancePeriod, int>
    {
        private readonly IAcceptancePeriodLogic acceptancePeriodLogic;
        private readonly IQCControlPlanLogic qCControlPlanLogic;
        private readonly IQCControlPlanLogic controlPlanLogic;

        public AcceptancePeriodController(ILogger<AcceptancePeriodController> logger
            , IStringLocalizer<AcceptancePeriodController> localizer, IAcceptancePeriodLogic acceptancePeriodLogic, IQCControlPlanLogic qCControlPlanLogic,
            IStringLocalizer<SharedResource> sharedLocalizer, IQCControlPlanLogic controlPlanLogic)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<AcceptancePeriodModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["AcceptancePeriods"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["AcceptancePeriods"],
                OperationColumns = true,
                HomePage = nameof(AcceptancePeriodController).Replace("Controller", "") + "/index",
                HasToolbar = true,
                ToolbarName="_adminToolbar"
            };
            this.acceptancePeriodLogic=acceptancePeriodLogic??throw new ArgumentNullException(nameof(acceptancePeriodLogic));
            this.qCControlPlanLogic=qCControlPlanLogic??throw new ArgumentNullException(nameof(qCControlPlanLogic));
            this.controlPlanLogic=controlPlanLogic??throw new ArgumentNullException(nameof(controlPlanLogic));
        }

        [ControlPanelMenu("AcceptancePeriods", ParentName = "BaseInfoManagement", Icon = "fa fa-calendar-plus", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            ViewBag.ControlPlans=FillQCControlPlans();
            return View(Model);
        }

        [HttpPost]
        [ParentalAuthorize(nameof(Index))]
        public async Task<IActionResult> ImportFromExcel()
        {
            try
            {
                if (!Request.Form.Files.Any())
                {
                    return Json(new { Result = "fail", message = "هیچ فایلی انتخاب نشده است" });
                }
                var file = Request.Form.Files[0];
                var acceptancePeriodList = new List<AcceptancePeriodImportModel>();
                using var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                acceptancePeriodList = acceptancePeriodList.ImportFromExcel(ms).ToList();
                var controlPlans = qCControlPlanLogic.GetAll();
                var insertList = new List<AcceptancePeriodModel>();
                foreach (var acceptancePeriod in acceptancePeriodList)
                {
                    var relatedControlPlan = controlPlans.ResultEntity.FirstOrDefault(x => x.Title==acceptancePeriod.ControlPlanTitle.Trim());
                    if (relatedControlPlan!=null)
                    {
                        insertList.Add(new AcceptancePeriodModel
                        {
                            A=acceptancePeriod.A,
                            QCControlPlanId=relatedControlPlan.QCControlPlanId,
                            StartInterval=acceptancePeriod.StartInterval,
                            EndInterval=acceptancePeriod.EndInterval,
                            SampleCount=acceptancePeriod.SampleCount,
                            Total = acceptancePeriod.Total
                        });
                    }
                }
                var result = await acceptancePeriodLogic.BulkInsertAsync(insertList);
                return Json(new { Result = "ok" });
            }
            catch (Exception)
            {
                return Json(new { Result = "fail", message = "درج برخی از ردیف ها با خطا مواجه شده" });
            }
        }

        public override IActionResult GetData([FromServices] ILogic<AcceptancePeriodModel> service, DatatablesSentModel model)
        {
            var data = base.GetData(service, model);
            return data;
        }

        protected override void ModifyItem(ILogic<AcceptancePeriodModel> service, int id)
        {
            ViewBag.ControlPlans=FillQCControlPlans();
            base.ModifyItem(service, id);
        }

        private List<SelectListItem> FillQCControlPlans()
        {

            var result = new List<SelectListItem>();
            var data = controlPlanLogic.GetAll();

            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }
            return data.ResultEntity.ToSelectList(nameof(QCControlPlanModel.Title), nameof(QCControlPlanModel.QCControlPlanId));
        }
    }

}
