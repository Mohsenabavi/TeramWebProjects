using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Transactions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.FileUploader.Models;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.HR.Module.Recruitment.Logic;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.Module.Authentication.Models;
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
    public class EmployeeJobBackgroundController : ControlPanelBaseController<EmployeeJobBackgroundModel, EmployeeJobBackground, int>
    {
        private readonly IBaseInformationLogic baseInformationLogic;
        private readonly ISendAsiaSMSService sendAsiaSMSService;
        private readonly IJobApplicantLogic jobApplicantLogic;
        private readonly IUserSharedService userSharedService;
        private readonly IJobApplicantFileLogic jobApplicantFileLogic;
        private readonly IUserPrincipal userPrincipal;
        private readonly IEmployeeJobBackgroundLogic employeeJobBackgroundLogic;        

        public EmployeeJobBackgroundController(ILogger<EmployeeJobBackgroundController> logger, IBaseInformationLogic baseInformationLogic, ISendAsiaSMSService sendAsiaSMSService
            , IStringLocalizer<EmployeeJobBackgroundController> localizer, IJobApplicantLogic jobApplicantLogic, IUserSharedService userSharedService, IJobApplicantFileLogic jobApplicantFileLogic,
            IUserPrincipal userPrincipal, IEmployeeJobBackgroundLogic employeeJobBackgroundLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<EmployeeJobBackgroundModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["EmployeeJobBackground"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["EmployeeJobBackground"],
                OperationColumns = true,
                HomePage = nameof(EmployeeJobBackgroundController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/HR/Module/Recruitment/Scripts/EmployeeJobBackground.js",
                GetDataUrl = nameof(GetGridData),
                ModelData=new EmployeeJobBackgroundModel()
            };
            this.baseInformationLogic=baseInformationLogic??throw new ArgumentNullException(nameof(baseInformationLogic));
            this.sendAsiaSMSService=sendAsiaSMSService??throw new ArgumentNullException(nameof(sendAsiaSMSService));
            this.jobApplicantLogic=jobApplicantLogic??throw new ArgumentNullException(nameof(jobApplicantLogic));
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
            this.jobApplicantFileLogic=jobApplicantFileLogic??throw new ArgumentNullException(nameof(jobApplicantFileLogic));
            this.userPrincipal=userPrincipal??throw new ArgumentNullException(nameof(userPrincipal));
            this.employeeJobBackgroundLogic=employeeJobBackgroundLogic??throw new ArgumentNullException(nameof(employeeJobBackgroundLogic));
        }

        [ControlPanelMenu("EmployeeJobBackground", ParentName = "JobBackground", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index(int id)
        {
            Model.GetDataUrl = "GetData?id=" + id;
            ViewBag.BaseInformationList=GetJobApplicantsList();
            Model.ModelData.JobApplicantId=id;
            return View(Model);
        }
        protected override void ModifyItem(ILogic<EmployeeJobBackgroundModel> service, int id)
        {
            ViewBag.BaseInformationList=GetJobApplicantsList();
            base.ModifyItem(service, id);
        }

        public override IActionResult GetData([FromServices] ILogic<EmployeeJobBackgroundModel> service, DatatablesSentModel model)
        {
            return base.GetData(service, model);
        }

        public override IActionResult Remove([FromServices] ILogic<EmployeeJobBackgroundModel> service, int key)
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

        protected override List<EmployeeJobBackgroundModel> ModifyGridData(List<EmployeeJobBackgroundModel> data)
        {
            var jobApplicantIds = data.Select(x => x.JobApplicantId).ToList();
            var jobApplicantsData = jobApplicantLogic.GetByIds(jobApplicantIds);
            var userIds = data.Where(x => x.ApprovedBy!=Guid.Empty).Select(x => x.ApprovedBy).ToList();
            var userInfo = userSharedService.GetUserInfos(userIds);
            foreach (var item in data)
            {
                var relatedJobApplicantsData = jobApplicantsData.ResultEntity.FirstOrDefault(x => x.JobApplicantId==item.JobApplicantId);
                item.FullName=(relatedJobApplicantsData!=null) ? relatedJobApplicantsData.FirstName + " " + relatedJobApplicantsData.LastName : "-";
                item.NationalCode=(relatedJobApplicantsData!=null) ? relatedJobApplicantsData.NationalCode : "-";
                var user = userInfo.FirstOrDefault(x => x.UserId==item.ApprovedBy);
                item.ApprovedByUserName=(user!=null) ? user.Fullname : "-";
            }
            return base.ModifyGridData(data);
        }

        public override IActionResult Save([FromServices] ILogic<EmployeeJobBackgroundModel> service, EmployeeJobBackgroundModel model)
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

            if ((!model.ResumeIsMatch || !model.PerformanceIsApproved || !model.DisciplineIsApproved) && model.ApproveStatus == BackgroundJobApproveStatus.Approved)
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


            if (model.EmployeeJobBackgroundId==0)
            {
                var checkForExistData = employeeJobBackgroundLogic.GetByJobApplicantId(model.JobApplicantId);

                if (checkForExistData.ResultStatus == OperationResultStatus.Successful && checkForExistData.ResultEntity is not null)
                {
                    return Json(new { result = "fail", message = localizer["data allready Exist"] });
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
        private List<SelectListItem> GetJobApplicantsList()
        {
            var result = new List<SelectListItem>();
            var data = jobApplicantLogic.GetEmployeesJobApplicants();
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

        [ParentalAuthorize(nameof(Edit))]
        public IActionResult GetWorkerJobBackgroundAttachmentTypes(int id)
        {
            var attachmentTypes = new List<int>();

            var employeeJobBackgroundResult = employeeJobBackgroundLogic.GetRow(id);

            if (employeeJobBackgroundResult.ResultStatus != OperationResultStatus.Successful || employeeJobBackgroundResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[employeeJobBackgroundResult.AllMessages] });
            }
            var jobApplicantFilesResult = jobApplicantFileLogic.GetByJobApplicantId(employeeJobBackgroundResult.ResultEntity.JobApplicantId);
            if (jobApplicantFilesResult.ResultStatus != OperationResultStatus.Successful || jobApplicantFilesResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[jobApplicantFilesResult.AllMessages] });
            }
            attachmentTypes = jobApplicantFilesResult.ResultEntity.Select(x => x.AttachmentTypeId).ToList();
            return Json(new { result = "ok", message = localizer[jobApplicantFilesResult.AllMessages], data = attachmentTypes, jobApplicantId = employeeJobBackgroundResult.ResultEntity.JobApplicantId });
        }

        //public IActionResult GetRelatedEmployeeJobBackGround(int jobApplicantId) {

        //    var relatedEmployeeJobBackground = employeeJobBackgroundLogic.GetByJobApplicantId(jobApplicantId);


        
        //}
    }
}
