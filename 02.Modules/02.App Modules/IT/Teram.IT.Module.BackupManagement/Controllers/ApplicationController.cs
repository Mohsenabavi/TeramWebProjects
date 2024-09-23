using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.IT.Module.BackupManagement.Entities;
using Teram.IT.Module.BackupManagement.Models;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.IT.Module.BackupManagement.Controllers
{
   
    [ControlPanelMenu("BackupManagement", Name = "BackupManagement", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar, Order = 4)]
    public class ApplicationController : ControlPanelBaseController<ApplicationModel, Application, int>
    {

        public ApplicationController(ILogger<ApplicationController> logger
            , IStringLocalizer<ApplicationController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<ApplicationModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["Applications"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["Applications"],
                OperationColumns = true,
                HomePage = nameof(ApplicationController).Replace("Controller", "") + "/index",
            };
        }

        [ControlPanelMenu("Applications", ParentName = "BackupManagement", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
    }

}
