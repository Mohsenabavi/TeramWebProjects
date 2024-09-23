using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using ModuleSharedContracts.Hubs;
using Teram.HR.Module.Recruitment.Entities.BaseInfo;
using Teram.HR.Module.Recruitment.Jobs;
using Teram.HR.Module.Recruitment.Models.BaseInfo;
using Teram.Module.SmsSender.Services;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Xceed.Words.NET;

namespace Teram.HR.Module.Recruitment.Controllers
{

    [ControlPanelMenu("BaseSystemInfo", Name = "BaseSystemInfo", Icon = "fa fa-tasks", PanelType = PanelType.User, Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar)]
    public class MajorController : ControlPanelBaseController<MajorModel, Major, int>
    {
        private readonly ISendAsiaSMSService sendAsiaSMSService;
        private readonly IHubContext<ProgressHub> _hubContext;
        public MajorController(ILogger<MajorController> logger
            , IStringLocalizer<MajorController> localizer, ISendAsiaSMSService sendAsiaSMSService,
            IStringLocalizer<SharedResource> sharedLocalizer, IHubContext<ProgressHub> hubContext)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;
            this.sendAsiaSMSService=sendAsiaSMSService??throw new ArgumentNullException(nameof(sendAsiaSMSService));
            this._hubContext = hubContext;


            Model = new ViewInformation<MajorModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["Majors"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["Majors"],
                OperationColumns = true,
                HomePage = nameof(MajorController).Replace("Controller", "") + "/index",
                HasToolbar = true,
                ToolbarName = "_adminToolbar"
            };
        }

        [ControlPanelMenu("Majors", ParentName = "BaseSystemInfo", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult UploadFile(IFormFile file)
        {
            BackgroundJob.Enqueue(() => ProcessExcelFile("D:\\Cities.xlsx"));
            return RedirectToAction("Index");
        }

        [ParentalAuthorize(nameof(Index))]
        public void ProcessExcelFile(string filePath)
        {
            var progress = new Progress<int>(percent =>
            {
                _hubContext.Clients.All.SendAsync("ReceiveProgress", percent);
            });

            var importJob = new ExcelImportJob();
            importJob.ImportData(filePath, progress);
        }
    }

}
