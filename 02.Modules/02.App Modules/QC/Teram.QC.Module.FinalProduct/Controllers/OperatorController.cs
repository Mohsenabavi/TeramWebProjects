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
    public class OperatorController : ControlPanelBaseController<OperatorModel, Operator, int>
    {
        public OperatorController(ILogger<OperatorController> logger
            , IStringLocalizer<OperatorController> localizer,
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
            };
        }

        [ControlPanelMenu("Operators", ParentName = "BaseInfoManagement", Icon = "fa fa-users-cog", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
    }

}
