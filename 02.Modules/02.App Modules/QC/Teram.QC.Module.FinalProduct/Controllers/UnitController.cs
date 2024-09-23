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
    public class UnitController : ControlPanelBaseController<UnitModel, Unit, int>
    {

        public UnitController(ILogger<UnitController> logger
            , IStringLocalizer<UnitController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<UnitModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["Units"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["Units"],
                OperationColumns = true,
                HomePage = nameof(UnitController).Replace("Controller", "") + "/index",
            };
        }

        [ControlPanelMenu("Units", ParentName = "BaseInfoManagement", Icon = "fa fa-sitemap", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
    }

}
