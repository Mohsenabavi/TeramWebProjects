using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Transactions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.FileUploader.Models;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.HR.Module.Recruitment.ExtensionMethods;
using Teram.HR.Module.Recruitment.Logic;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.HR.Module.Recruitment.Models.WorkWithUs;
using Teram.HR.Module.Recruitment.Services;
using Teram.Module.SmsSender.Models.AsiaSms;
using Teram.Module.SmsSender.Services;
using Teram.Module.SmsSender.Services.AsiaSms;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Teram.Web.Core.Model;
using Teram.Web.Core.Security;

namespace Teram.HR.Module.Recruitment.Controllers
{

    [ControlPanelMenu("JobBackground", Icon = "fa fa-tasks", PanelType = PanelType.User, Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar)]
    public class WorkerJobBackgroundController : ControlPanelBaseController<WorkerJobBackgroundModel, WorkerJobBackground, int>
    {
        private readonly IJobApplicantLogic jobApplicantLogic;
        private readonly IBaseInformationLogic baseInformationLogic;
        private readonly ISendAsiaSMSService sendAsiaSMSService;
        private readonly IJobApplicantFileLogic jobApplicantFileLogic;
        private readonly IUserPrincipal userPrincipal;
        private readonly IWorkerJobBackgroundLogic workerJobBackgroundLogic;
        private readonly IUserSharedService userSharedService;

        public WorkerJobBackgroundController(ILogger<WorkerJobBackgroundController> logger
            , IStringLocalizer<WorkerJobBackgroundController> localizer, IJobApplicantLogic jobApplicantLogic, IBaseInformationLogic baseInformationLogic, ISendAsiaSMSService sendAsiaSMSService, IJobApplicantFileLogic jobApplicantFileLogic,
            IStringLocalizer<SharedResource> sharedLocalizer, IUserPrincipal userPrincipal, IWorkerJobBackgroundLogic workerJobBackgroundLogic,
            IUserSharedService userSharedService)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<WorkerJobBackgroundModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["WorkerJobBackground"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["WorkerJobBackground"],
                OperationColumns = true,
                HomePage = nameof(WorkerJobBackgroundController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/HR/Module/Recruitment/Scripts/WorkerJobBackground.js",
                GetDataUrl = nameof(GetGridData),
                ModelData=new WorkerJobBackgroundModel()
            };
            this.jobApplicantLogic=jobApplicantLogic??throw new ArgumentNullException(nameof(jobApplicantLogic));
            this.baseInformationLogic=baseInformationLogic??throw new ArgumentNullException(nameof(baseInformationLogic));
            this.sendAsiaSMSService=sendAsiaSMSService??throw new ArgumentNullException(nameof(sendAsiaSMSService));
            this.jobApplicantFileLogic=jobApplicantFileLogic??throw new ArgumentNullException(nameof(jobApplicantFileLogic));
            this.userPrincipal=userPrincipal??throw new ArgumentNullException(nameof(userPrincipal));
            this.workerJobBackgroundLogic=workerJobBackgroundLogic??throw new ArgumentNullException(nameof(workerJobBackgroundLogic));
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
        }

        [ControlPanelMenu("WorkerJobBackground", ParentName = "JobBackground", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index(int id)
        {
            Model.GetDataUrl = "GetData?id=" + id;
            ViewBag.BaseInformationList=GetJobApplicantsList();
            Model.ModelData.JobApplicantId=id;
            return View(Model);
        }

        public override IActionResult GetData([FromServices] ILogic<WorkerJobBackgroundModel> service, DatatablesSentModel model)
        {
            return base.GetData(service, model);
        }
        protected override void ModifyItem(ILogic<WorkerJobBackgroundModel> service, int id)
        {
            ViewBag.BaseInformationList=GetJobApplicantsList();
            base.ModifyItem(service, id);
        }

        public override IActionResult Save([FromServices] ILogic<WorkerJobBackgroundModel> service, WorkerJobBackgroundModel model)
        {
            var relatedJobApplicant = jobApplicantLogic.GetById(model.JobApplicantId);
            if (relatedJobApplicant.ResultStatus != OperationResultStatus.Successful || relatedJobApplicant.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[relatedJobApplicant.AllMessages] });
            }

            if (relatedJobApplicant.ResultEntity.BaseInformationApproveStatus==ApproveStatus.FisrtApproved || relatedJobApplicant.ResultEntity.BaseInformationApproveStatus==ApproveStatus.FinalApprove)
            {
                return Json(new { result = "fail", message = localizer["Approved Data Has no Permission for Save"] });
            }

            if ((!model.ResumeIsMatch || !model.AddressIMatch) && model.ApproveStatus == BackgroundJobApproveStatus.Approved)
            {

                return Json(new { result = "fail", message = localizer["Approve is Not Permitted"] });
            }

            var files = Request.Form.Files;

            var jobApplicantFiles = jobApplicantFileLogic.GetByJobApplicantId(model.JobApplicantId);

            if (files.Count>0)
            {

                foreach (var file in files)
                {
                    var contentDisposition = file.ContentDisposition;

                    if (contentDisposition.Contains("BackgroundAttchament1"))
                    {
                        var checkForExistReferral = jobApplicantFiles.ResultEntity.Where(x => x.AttachmentTypeId==(int)DocumentType.BackgroundAttchament1).FirstOrDefault();
                        if (checkForExistReferral != null)
                            return Json(new { result = "fail", message = localizer["BackgroundAttchament1 has Already Uploaded"] });
                    }

                    if (contentDisposition.Contains("BackgroundAttchament2"))
                    {
                        var checkForExistReferral = jobApplicantFiles.ResultEntity.Where(x => x.AttachmentTypeId==(int)DocumentType.BackgroundAttchament2).FirstOrDefault();
                        if (checkForExistReferral != null)
                            return Json(new { result = "fail", message = localizer["BackgroundAttchament2 has Already Uploaded"] });
                    }
                }

                var fileUploadResult = jobApplicantFileLogic.SaveFiles(files, model.JobApplicantId, jobApplicantFiles.ResultEntity);

                if (fileUploadResult.ResultStatus != OperationResultStatus.Successful || fileUploadResult.ResultEntity is null)
                {
                    return Json(new { result = "fail", message = localizer[fileUploadResult.AllMessages] });
                }

                var backgroundAttchament1UploadResult = fileUploadResult.ResultEntity.Where(x => x.File.ContentDisposition.Contains("BackgroundAttchament1")).FirstOrDefault();
                if (backgroundAttchament1UploadResult != null)
                    model.BackgroundAttchamentId1=backgroundAttchament1UploadResult.AttachmentId;

                var backgroundAttchament2UploadResult = fileUploadResult.ResultEntity.Where(x => x.File.ContentDisposition.Contains("BackgroundAttchament2")).FirstOrDefault();
                if (backgroundAttchament2UploadResult != null)
                    model.BackgroundAttchamentId2=backgroundAttchament2UploadResult.AttachmentId;
            }

            if (model.WorkerJobBackgroundId==0)
            {

                var existData = workerJobBackgroundLogic.GetByJobApplicantId(model.JobApplicantId);

                if (existData.ResultStatus == OperationResultStatus.Successful && existData.ResultEntity is not null)
                {
                    return Json(new { result = "fail", message = localizer["Data already Exists"] });
                }
            }

            else
            {
                if (relatedJobApplicant.ResultEntity.ProcessStatus>ProcessStatus.ApproveJobBackground)
                {
                    return Json(new { result = "fail", message = localizer["Due To Form Status Edit is Not Permitted"] });
                }
            }

            if (model.ApproveStatus == BackgroundJobApproveStatus.Approved)
            {
                relatedJobApplicant.ResultEntity.ProcessStatus=ProcessStatus.ApproveJobBackground;
                relatedJobApplicant.ResultEntity.Address=(!string.IsNullOrEmpty(relatedJobApplicant.ResultEntity.Address) ? relatedJobApplicant.ResultEntity.Address : " ");
                relatedJobApplicant.ResultEntity.PromissoryNoteAmount=(!string.IsNullOrEmpty(relatedJobApplicant.ResultEntity.PromissoryNoteAmount) ? relatedJobApplicant.ResultEntity.PromissoryNoteAmount : " ");
                jobApplicantLogic.Update(relatedJobApplicant.ResultEntity);
            }
            model.ApprovedBy=userPrincipal.CurrentUserId;
            model.ApproveDate=DateTime.Now;
            return base.Save(service, model);
        }
        protected override List<WorkerJobBackgroundModel> ModifyGridData(List<WorkerJobBackgroundModel> data)
        {
            var jobApplicantIds = data.Select(x => x.JobApplicantId).ToList();
            var jobApplicantsData = jobApplicantLogic.GetByIds(jobApplicantIds);
            var userIds = data.Where(x => x.ApprovedBy!=Guid.Empty).Select(x => x.ApprovedBy).ToList();
            var userInfo = userSharedService.GetUserInfos(userIds);
            foreach (var item in data)
            {
                var user = userInfo.FirstOrDefault(x => x.UserId==item.ApprovedBy);
                item.ApprovedByUserName=(user!=null) ? user.Fullname : "-";
                var relatedJobApplicants = jobApplicantsData.ResultEntity.FirstOrDefault(x => x.JobApplicantId==item.JobApplicantId);
                item.FullName=(relatedJobApplicants!=null) ? relatedJobApplicants.FirstName + " " + relatedJobApplicants.LastName : "-";
                item.NationalCode=(relatedJobApplicants!=null) ? relatedJobApplicants.NationalCode : "-";
            }
            return base.ModifyGridData(data);
        }

        private List<SelectListItem> GetJobApplicantsList()
        {
            var result = new List<SelectListItem>();
            var data = jobApplicantLogic.GetWorkersJobApplicants();
            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }
            return data.ResultEntity.Select(x => new SelectListItem
            {
                Text = string.Concat("کد ملی : ", x.NationalCode, " - ", "شماره تلفن :", x.MobileNumber),
                Value = x.JobApplicantId.ToString()
            }).ToList();
        }

        public override IActionResult Remove([FromServices] ILogic<WorkerJobBackgroundModel> service, int key)
        {
            var relatedRecordResult = service.GetRow(key);

            if (relatedRecordResult.ResultStatus != OperationResultStatus.Successful || relatedRecordResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[relatedRecordResult.AllMessages] });
            }

            var relatedJobApplicant = jobApplicantLogic.GetById(relatedRecordResult.ResultEntity.JobApplicantId);

            if (relatedJobApplicant.ResultStatus != OperationResultStatus.Successful || relatedJobApplicant.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[relatedJobApplicant.AllMessages] });
            }

            if (relatedJobApplicant.ResultEntity.ProcessStatus<=ProcessStatus.ApproveJobBackground)
            {
                return base.Remove(service, key);
            }

            return Json(new { result = "fail", message = localizer["Due To Form Status Delete is Not Permitted"] });
        }

        [ParentalAuthorize(nameof(Edit))]
        public IActionResult GetWorkerJobBackgroundAttachmentTypes(int id)
        {
            var attachmentTypes = new List<int>();

            var workerJobBackgroundResult = workerJobBackgroundLogic.GetRow(id);

            if (workerJobBackgroundResult.ResultStatus != OperationResultStatus.Successful || workerJobBackgroundResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[workerJobBackgroundResult.AllMessages] });
            }
            var jobApplicantFilesResult = jobApplicantFileLogic.GetByJobApplicantId(workerJobBackgroundResult.ResultEntity.JobApplicantId);
            if (jobApplicantFilesResult.ResultStatus != OperationResultStatus.Successful || jobApplicantFilesResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[jobApplicantFilesResult.AllMessages] });
            }
            attachmentTypes = jobApplicantFilesResult.ResultEntity.Select(x => x.AttachmentTypeId).ToList();
            return Json(new { result = "ok", message = localizer[jobApplicantFilesResult.AllMessages], data = attachmentTypes ,jobApplicantId= workerJobBackgroundResult.ResultEntity.JobApplicantId });
        }
    }

}
