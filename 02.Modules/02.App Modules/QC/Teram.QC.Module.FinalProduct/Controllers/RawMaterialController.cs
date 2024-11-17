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
  
    public class RawMaterialController : ControlPanelBaseController<RawMaterialModel, RawMaterial, int>
    {

        public RawMaterialController(ILogger<RawMaterialController> logger
            , IStringLocalizer<RawMaterialController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<RawMaterialModel>
            {              
                EditInSamePage = true,
                GridTitle = localizer["RawMaterial"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["RawMaterial"],
                OperationColumns = true,
                HomePage = nameof(RawMaterialController).Replace("Controller", "") + "/index",
            };

        }

        [ControlPanelMenu("RawMaterial", ParentName = "BaseInfoManagement", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
    }

}
