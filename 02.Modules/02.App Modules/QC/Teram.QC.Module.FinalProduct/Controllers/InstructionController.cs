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
    [ControlPanelMenu("BaseInfoManagement", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar, Order = 4)]
    public class InstructionController : ControlPanelBaseController<InstructionModel, Instruction, int>
    {
        public InstructionController(ILogger<InstructionController> logger
            , IStringLocalizer<InstructionController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<InstructionModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["Instructions"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["Instructions"],
                OperationColumns = true,
                HomePage = nameof(InstructionController).Replace("Controller", "") + "/index",
            };
        }

        [ControlPanelMenu("Instructions", ParentName = "BaseInfoManagement", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
    }

}
