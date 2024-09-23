using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.Module.GeographicRegion.Enums;
using Teram.Module.GeographicRegion.Logic.Interfaces;
using Teram.Module.GeographicRegion.Models;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.HR.Module.Recruitment.Controllers
{
    public class JobPositionController : ControlPanelBaseController<JobPositionModel, JobPosition, int>
    {
        private readonly IUserSharedService userSharedService;
        private readonly IJobApplicantLogic jobApplicantLogic;

        public JobPositionController(ILogger<JobPositionController> logger
            , IStringLocalizer<JobPositionController> localizer, IUserSharedService userSharedService, IJobApplicantLogic jobApplicantLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<JobPositionModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["JobPositions"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["JobPositions"],
                OperationColumns = true,
                HomePage = nameof(JobPositionController).Replace("Controller", "") + "/index",
                HasToolbar = true,
                ToolbarName = "_adminToolbar"
            };
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
            this.jobApplicantLogic=jobApplicantLogic??throw new ArgumentNullException(nameof(jobApplicantLogic));
        }

        [ControlPanelMenu("JobPositions", ParentName = "BaseSystemInfo", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            //var counter = 0;

            //var jobApplicants = jobApplicantLogic.GetAll().ResultEntity.Where(x=>x.FlowType==Enums.FlowType.Employed).ToList();

            //var jobApplicantNationIds = jobApplicants.Select(x => x.NationalCode).ToList();

            //var users = userSharedService.GetAllUsers();

            //var userNames = users.Select(x => x.Username).ToList();


            //var relatedUsers = users.Where(x => jobApplicantNationIds.Contains(x.Username)).ToList();


            //var relatedUsers2 = jobApplicants.Where(x => !userNames.Contains(x.NationalCode)).ToList();


            //foreach (var item in relatedUsers)
            //{
            //    var checkFoeRole = await userSharedService.IsInRoleAsync(item, "Employed");

            //    if (!checkFoeRole)
            //    {
            //        counter++;
            //        //var roleResult = await userSharedService.AddToRoleAsync(item, "Employed");
            //    }
            //}


            //var x = counter;

            return View(Model);
        }

        [HttpPost]
        [ParentalAuthorize(nameof(Index))]
        public IActionResult ImportFromExcel([FromServices] IJobPositionLogic jobPositionLogic)
        {
            try
            {
                if (!Request.Form.Files.Any())
                {
                    return Json(new { Result = "fail", message = "هیچ فایلی انتخاب نشده است" });
                }
                var file = Request.Form.Files[0];

                var jobPositionDetails = new List<JobPositionImportModel>();

                using var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                jobPositionDetails = jobPositionDetails.ImportFromExcel(ms).ToList();

                foreach (var item in jobPositionDetails)
                {
                    var jobPosition = new JobPositionModel
                    {
                        Title = item.Title,
                        IsActive=true
                    };
                    jobPositionLogic.AddNew(jobPosition);
                }
                return Json(new { Result = "ok" });
            }
            catch (Exception)
            {
                return Json(new { Result = "fail", message = "درج برخی از ردیف ها با خطا مواجه شده" });
            }
        }
    }

}
