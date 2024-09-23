using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.HR.Module.OC.Models;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.HR.Module.OC.Controllers
{  
    public class PositionController : ControlPanelBaseController<PositionModel, Entities.Position, int>
    {

        public PositionController(ILogger<PositionController> logger
            , IStringLocalizer<PositionController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;


            Model = new ViewInformation<PositionModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["Position"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["Position"],
                OperationColumns = true,
                HomePage = nameof(PositionController).Replace("Controller", "") + "/index",
            };
        }

        [ControlPanelMenu("Position", ParentName = "OrganizationChartManagement", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
    }

}
