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
    public class BackupHistoryController : ControlPanelBaseController<BackupHistoryModel, BackupHistory, int>
    {

        public BackupHistoryController(ILogger<BackupHistoryController> logger
            , IStringLocalizer<BackupHistoryController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;


            Model = new ViewInformation<BackupHistoryModel>
            {
                EditInSamePage = true,
                GridId="BackupHistory",
                GridTitle = localizer["BackupHistory"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["BackupHistory"],
                OperationColumns = true,
                HomePage = nameof(BackupHistoryController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/IT/Module/BackupManagement/Scripts/BackupHistory.js",
            };
        }

        [ControlPanelMenu("BackupHistory", ParentName = "BackupManagement", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
    }

}
