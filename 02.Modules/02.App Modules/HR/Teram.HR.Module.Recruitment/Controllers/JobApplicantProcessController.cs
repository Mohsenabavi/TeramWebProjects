using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.Web.Core;
using Teram.Web.Core.ControlPanel;

namespace Teram.HR.Module.Recruitment.Controllers
{
    public class JobApplicantProcessController : ControlPanelBaseController<JobApplicantModel, JobApplicant, int>
    {
        private readonly IJobApplicantLogic jobApplicantLogic;

        public JobApplicantProcessController(ILogger<JobApplicantProcessController> logger
            , IStringLocalizer<JobApplicantProcessController> localizer, IJobApplicantLogic jobApplicantLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<JobApplicantModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["models"],
                HasManagmentForm = true,                
                Title = localizer["models"],
                OperationColumns = true,
                HomePage = nameof(JobApplicantProcessController).Replace("Controller", "") + "/index",
                Layout = "",
            };
            this.jobApplicantLogic=jobApplicantLogic??throw new ArgumentNullException(nameof(jobApplicantLogic));
        }
        public IActionResult Index(int id)
        {
            var jobApplicantResult = jobApplicantLogic.GetById(id);
            if (jobApplicantResult.ResultStatus != OperationResultStatus.Successful || jobApplicantResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[jobApplicantResult.AllMessages] });
            }
            InitStep(jobApplicantResult.ResultEntity);
            return View(Model);
        }

        private void InitStep(JobApplicantModel model)
        {
            int step = 1;

            if (model.ProcessStatus==Enums.ProcessStatus.NoAction)
            {
                ViewBag.Step=0;
                step=0;
            }
            else if (model.ProcessStatus==Enums.ProcessStatus.BaseInformationAdded)
            {
                ViewBag.Step=1;
                step=1;
            }
            string componentName = new[] {
                $"WorkWithUS",
                $"url:JobApplicant/Index",
            }[step];
            ViewBag.name = componentName;
            ViewBag.step = step;
            return;
        }
    }
}
