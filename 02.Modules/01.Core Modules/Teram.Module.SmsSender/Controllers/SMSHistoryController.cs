using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.Module.SmsSender.Entities;
using Teram.Module.SmsSender.Models;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.Module.SmsSender.Controllers
{
    [ControlPanelMenu("SMS", Icon = "fa fa-tasks", PanelType = PanelType.User, Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar, Order = 2)]
    public class SMSHistoryController : ControlPanelBaseController<SMSHistoryModel, SMSHistory, int>
    {

        public SMSHistoryController(ILogger<SMSHistoryController> logger
            , IStringLocalizer<SMSHistoryController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<SMSHistoryModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["SMSHistory"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["SMSHistory"],
                OperationColumns = true,
                HomePage = nameof(SMSHistoryController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/Module/SmsSender/Scripts/SMSHistory.js",
            };
        }

        [ControlPanelMenu("SMSHistory", ParentName = "SMS", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
    }

}
