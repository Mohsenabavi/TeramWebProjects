using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Models;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.QC.Module.FinalProduct.Controllers
{
    [ControlPanelMenu("FinalProductInspection", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar, Order = 4)]
    public class QCControlPlanController : ControlPanelBaseController<QCControlPlanModel, QCControlPlan, int>
    {

        public QCControlPlanController(ILogger<QCControlPlanController> logger
            , IStringLocalizer<QCControlPlanController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<QCControlPlanModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["FinalProductInspection"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["FinalProductInspection"],
                OperationColumns = true,
                HomePage = nameof(QCControlPlanController).Replace("Controller", "") + "/index",
            };
        }

        [ControlPanelMenu("QCControlPlan", ParentName = "BaseInfoManagement", Icon = "fa fa-file-alt", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
    }

}
