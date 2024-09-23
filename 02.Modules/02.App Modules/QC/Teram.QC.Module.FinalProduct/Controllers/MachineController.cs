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
    public class MachineController : ControlPanelBaseController<MachineModel, Machine, int>
    {
        public MachineController(ILogger<MachineController> logger
            , IStringLocalizer<MachineController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;


            Model = new ViewInformation<MachineModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["Machines"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["Machines"],
                OperationColumns = true,
                HomePage = nameof(MachineController).Replace("Controller", "") + "/index",
            };
        }

        [ControlPanelMenu("Machines", ParentName = "BaseInfoManagement", Icon = "fa fa-cogs", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
    }

}
