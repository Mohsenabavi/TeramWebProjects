using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Configuration;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.Module.AttachmentsManagement.Models;
using Teram.Web.Core.ControlPanel;

namespace Teram.HR.Module.Recruitment.Controllers
{
    public class ViewWorkerJobBackgroundInfoController : BasicControlPanelController
    {
        private readonly IJobApplicantLogic jobApplicantLogic;        
        private readonly IJobApplicantFileLogic jobApplicantFileLogic;
        private readonly IConfiguration configuration;

        public ViewWorkerJobBackgroundInfoController(IJobApplicantLogic jobApplicantLogic,
            IJobApplicantFileLogic jobApplicantFileLogic, IConfiguration configuration, IStringLocalizer<UserAdmissionController> localizer)
        {
            this.jobApplicantLogic=jobApplicantLogic??throw new ArgumentNullException(nameof(jobApplicantLogic));
            this.jobApplicantFileLogic=jobApplicantFileLogic??throw new ArgumentNullException(nameof(jobApplicantFileLogic));
            this.configuration=configuration??throw new ArgumentNullException(nameof(configuration));
            this.localizer = localizer;
        }

        public IActionResult WorkerPrint(int id)
        {
            var downloadurl = configuration.GetSection("Attachment").Get<AttachmentSection>().DownloadUrl;
            var baseInformation = jobApplicantLogic.GetById(id);
            var imageInfo = jobApplicantFileLogic.GetPersonalImage(id);

            if (baseInformation.ResultStatus != OperationResultStatus.Successful || baseInformation.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer["baseInformation Not Found"] });
            }

            if (imageInfo.ResultStatus != OperationResultStatus.Successful || imageInfo.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer["Personnel Image Not Found"] });
            }

            var jobApplicantResult=jobApplicantLogic.GetById(id);

            if (jobApplicantResult.ResultStatus != OperationResultStatus.Successful || jobApplicantResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[jobApplicantResult.AllMessages] });
            }

            var viewModel = new ViewWorkerJobBackgroundInfoModel
            {
                FullName=baseInformation.ResultEntity.FirstName + " " + baseInformation.ResultEntity.LastName,
                Address=baseInformation.ResultEntity.Address,
                ImageSrc=$"{downloadurl}" + imageInfo.ResultEntity.AttachmentId,           
                JobTitle=jobApplicantResult.ResultEntity.JobPositionTitle
            };
            return View(viewModel);
        }

        public IActionResult EmployeePrint(int id)
        {
            var downloadurl = configuration.GetSection("Attachment").Get<AttachmentSection>().DownloadUrl;
            var baseInformation = jobApplicantLogic.GetById(id);
            var imageInfo = jobApplicantFileLogic.GetPersonalImage(id);

            if (baseInformation.ResultStatus != OperationResultStatus.Successful || baseInformation.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer["baseInformation Not Found"] });
            }

            if (imageInfo.ResultStatus != OperationResultStatus.Successful || imageInfo.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer["Personnel Image Not Found"] });
            }

            var jobApplicantResult = jobApplicantLogic.GetById(id);

            if (jobApplicantResult.ResultStatus != OperationResultStatus.Successful || jobApplicantResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[jobApplicantResult.AllMessages] });
            }

            var viewModel = new ViewWorkerJobBackgroundInfoModel
            {
                FullName=baseInformation.ResultEntity.FirstName + " " + baseInformation.ResultEntity.LastName,
                Address=baseInformation.ResultEntity.Address,
                ImageSrc=$"{downloadurl}" + imageInfo.ResultEntity.AttachmentId,               
                JobTitle=jobApplicantResult.ResultEntity.JobPositionTitle
            };
            return View(viewModel);
        }
    }
}


