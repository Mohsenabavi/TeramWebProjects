using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Logic;
using Teram.IT.Module.BackupManagement.Entities;
using Teram.IT.Module.BackupManagement.Logic.Interfaces;
using Teram.IT.Module.BackupManagement.Models;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Teram.Web.Core.Extensions;

namespace Teram.IT.Module.BackupManagement.Controllers
{

    public class ServerPathController : ControlPanelBaseController<ServerPathModel, ServerPath, int>
    {
        private readonly IApplicationLogic applicationLogic;

        public ServerPathController(ILogger<ServerPathController> logger
            , IStringLocalizer<ServerPathController> localizer, IApplicationLogic applicationLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<ServerPathModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["ServerPaths"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["ServerPaths"],
                OperationColumns = true,
                HomePage = nameof(ServerPathController).Replace("Controller", "") + "/index",
            };
            this.applicationLogic=applicationLogic??throw new ArgumentNullException(nameof(applicationLogic));
        }

        [ControlPanelMenu("ServerPaths", ParentName = "BackupManagement", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            ViewBag.Applications=GetApplications();
            return View(Model);
        }

        protected override void ModifyItem(ILogic<ServerPathModel> service, int id)
        {
            ViewBag.Applications=GetApplications();
            base.ModifyItem(service, id);
        }

        private List<SelectListItem> GetApplications()
        {
            var result = new List<SelectListItem>();
            var data = applicationLogic.GetAll();
            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }
            return data.ResultEntity.ToSelectList(nameof(ApplicationModel.Title), nameof(ApplicationModel.ApplicationId));
        }
    }

}
