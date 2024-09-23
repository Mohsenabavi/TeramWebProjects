using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.FileUploader.Models;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.HR.Module.Recruitment.Logic;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.Module.Authentication.Models;
using Teram.Module.SmsSender.Models.AsiaSms;
using Teram.Module.SmsSender.Services.AsiaSms;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Teram.Web.Core.Exceptions;
using Teram.Web.Core.Model;
using Teram.Web.Core.Security;

namespace Teram.HR.Module.Recruitment.Controllers
{
    public class AdditionalDocumentsController : ControlPanelBaseController<JobApplicantModel, JobApplicant, int>
    {
        private readonly IJobApplicantLogic jobApplicantLogic;
        private readonly IJobApplicantFileLogic jobApplicantFileLogic;
        private readonly IUserPrincipal userPrincipal;
        private readonly IBaseInformationLogic baseInformationLogic;

        public AdditionalDocumentsController(ILogger<AdditionalDocumentsController> logger, IJobApplicantLogic jobApplicantLogic, IJobApplicantFileLogic jobApplicantFileLogic
            , IStringLocalizer<AdditionalDocumentsController> localizer, IUserPrincipal userPrincipal, IBaseInformationLogic baseInformationLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<JobApplicantModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["AdditionalDocuments"],
                HasGrid = true,
                HasManagmentForm = true,
                GetDataUrl = "",
                LoadAjaxData = false,
                Title = localizer["AdditionalDocuments"],
                OperationColumns = true,
                HomePage = nameof(AdditionalDocumentsController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/HR/Module/Recruitment/Scripts/AdditionalDocuments.js",
                ModelData=new JobApplicantModel()
            };
            this.jobApplicantLogic=jobApplicantLogic??throw new ArgumentNullException(nameof(jobApplicantLogic));
            this.jobApplicantFileLogic=jobApplicantFileLogic??throw new ArgumentNullException(nameof(jobApplicantFileLogic));
            this.userPrincipal=userPrincipal??throw new ArgumentNullException(nameof(userPrincipal));
            this.baseInformationLogic=baseInformationLogic??throw new ArgumentNullException(nameof(baseInformationLogic));
        }

        [ControlPanelMenu("AdditionalDocuments", ParentName = "ManageJobApplicants", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar , Order =2)]
        public IActionResult Index()
        {
            return View(Model);
        }       

        protected override void ModifyItem(ILogic<JobApplicantModel> service, int id)
        {
            var baseInformation = baseInformationLogic.GetByJobApplicantId(id);
            if (baseInformation.ResultStatus == OperationResultStatus.Successful && baseInformation.ResultEntity is not null)
            {
                Model.ModelData.JobApplicantId=baseInformation.ResultEntity.JobApplicantId;
            }
        }


        public IActionResult UploadAfterFinalApprove() {

            return Content("This Is Not Actual Method");
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult SaveFiles(JobApplicantModel model)
        {

            if (model.JobApplicantId == 0)
            {
                return Json(new { result = "fail", message = localizer["Base Information Not Found"] });
            }

            var relatedJobApplicant = jobApplicantLogic.GetById(model.JobApplicantId);


            if (relatedJobApplicant.ResultStatus != OperationResultStatus.Successful || relatedJobApplicant.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[relatedJobApplicant.AllMessages] });
            }

            var hasPermissionForUploadAfterFinalApprove = userPrincipal.CurrentUser.HasClaim("Permission", ":AdditionalDocuments:UploadAfterFinalApprove");


            if (relatedJobApplicant.ResultEntity.ProcessStatus==ProcessStatus.FinalApproveByHR && !hasPermissionForUploadAfterFinalApprove) {

                return Json(new { result = "fail", message = localizer["Approved Info Is Not Permitted For Upload"] });
            }

            var files = Request.Form.Files;

            var baseInformationResult = baseInformationLogic.GetByJobApplicantId(model.JobApplicantId);

            if (baseInformationResult.ResultStatus != OperationResultStatus.Successful || baseInformationResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer["Base Information Not Found"] });
            }

            var checkForExistingFiles = jobApplicantFileLogic.GetByJobApplicantId(model.JobApplicantId);

            var fileUploadResult = jobApplicantFileLogic.SaveFiles(files, model.JobApplicantId, checkForExistingFiles.ResultEntity);

            if (fileUploadResult.ResultStatus != OperationResultStatus.Successful || fileUploadResult.ResultEntity.Select(x => x.IsSuccess).ToList().Contains(false))
            {
                List<string> messages = fileUploadResult.ResultEntity.Where(x => x.Message!=null && x.Message.Length>1).Select(x => x.Message).ToList();
                string concatenatedMessages = string.Join("\n", messages);
                return Json(new { result = "fail", message = concatenatedMessages });
            }

            var finalUploadedFiles = jobApplicantFileLogic.GetByJobApplicantId(model.JobApplicantId);

            return Json(new { result = "ok", message = localizer["Files Uploaded Successfully"] });
        }
    }

}
