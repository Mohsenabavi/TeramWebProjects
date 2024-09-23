using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.HR.Module.Recruitment.Controllers
{     
    public class BackgroundJobRequestController : ControlPanelBaseController<JobApplicantModel, JobApplicant, int>
    {
        public BackgroundJobRequestController(ILogger<BackgroundJobRequestController> logger
            , IStringLocalizer<BackgroundJobRequestController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<JobApplicantModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["BackgroundJobRequest"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["BackgroundJobRequest"],
                OperationColumns = true,
                GridId="BackgroundJobRequest",
                HomePage = nameof(BackgroundJobRequestController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/HR/Module/Recruitment/Scripts/BackgroundJobRequest.js",
                GetDataUrl = "",
                LoadAjaxData = false,
            };
        }

        [ControlPanelMenu("BackgroundJobRequest", ParentName = "JobBackground", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }

        public IActionResult ViewOnlyNeededToBackGroundCheckProtectionUnitOnly() {

            return Content("This is not actual Action");
        }
    }

}
