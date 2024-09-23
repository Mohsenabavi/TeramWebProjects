using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Models.CausationModels;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.QC.Module.FinalProduct.Controllers
{
    public class WorkStationController : ControlPanelBaseController<WorkStationModel, WorkStation, int>
    {

        public WorkStationController(ILogger<WorkStationController> logger
            , IStringLocalizer<WorkStationController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<WorkStationModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["WorkStations"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["WorkStations"],
                OperationColumns = true,
                HomePage = nameof(WorkStationController).Replace("Controller", "") + "/index",
            };
        }

        [ControlPanelMenu("WorkStations", ParentName = "BaseInfoManagement", Icon = "fa fa-network-wired", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
    }

}
