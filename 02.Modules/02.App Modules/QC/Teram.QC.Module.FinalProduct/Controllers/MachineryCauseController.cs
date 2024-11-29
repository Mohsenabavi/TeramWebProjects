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
    public class MachineryCauseController : ControlPanelBaseController<MachineryCauseModel, MachineryCause, int>
    {

        public MachineryCauseController(ILogger<MachineryCauseController> logger
            , IStringLocalizer<MachineryCauseController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;


            Model = new ViewInformation<MachineryCauseModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["MachineryCause"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["MachineryCause"],
                OperationColumns = true,
                HomePage = nameof(MachineryCauseController).Replace("Controller", "") + "/index",
            };
        }

        [ControlPanelMenu("MachineryCause", ParentName = "BaseInfoManagement", Icon = "fa fa-users-cog", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
    }

}
