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
    public class RootCauseController : ControlPanelBaseController<RootCauseModel, RootCause, int>
    {
        public RootCauseController(ILogger<RootCauseController> logger
            , IStringLocalizer<RootCauseController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<RootCauseModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["RootCause"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["RootCause"],
                OperationColumns = true,
                HomePage = nameof(RootCauseController).Replace("Controller", "") + "/index",
            };
        }

        [ControlPanelMenu("RootCause", ParentName = "BaseInfoManagement", Icon = "fa fa-search", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
    }
}
