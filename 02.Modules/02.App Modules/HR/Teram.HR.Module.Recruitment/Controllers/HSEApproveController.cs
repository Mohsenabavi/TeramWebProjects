using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Configuration;
using System.Net.Mime;
using System.Text.Json.Nodes;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.FileUploader.Models;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.HR.Module.Recruitment.Logic;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.Module.AttachmentsManagement.Models;
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
    public class HSEApproveController : ControlPanelBaseController<HSEGridModel, JobApplicant, int>
    {
        private readonly IHSEApproveLogic approveLogic;
        private readonly IConfiguration configuration;
        private readonly IJobApplicantFileLogic jobApplicantFileLogic;
        private readonly IUserSharedService userSharedService;
        private readonly IUserPrincipal userPrincipal;
        private readonly IBaseInformationLogic baseInformationLogic;
        private readonly ISendAsiaSMSService sendAsiaSMSService;

        public HSEApproveController(ILogger<HSEApproveController> logger
            , IStringLocalizer<HSEApproveController> localizer, IHSEApproveLogic approveLogic, IConfiguration configuration, IJobApplicantFileLogic jobApplicantFileLogic,
            IUserSharedService userSharedService, IUserPrincipal userPrincipal, IBaseInformationLogic baseInformationLogic, ISendAsiaSMSService sendAsiaSMSService,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<JobApplicantModel>
            {
                ModelData=new HSEGridModel(),
                ModelType=typeof(HSEGridModel),
                Title = localizer["HSEApprove"],
                EditInSamePage = true,
                HasGrid = true,
                HasManagmentForm = true,
                ExtraScripts = "/ExternalModule/HR/Module/Recruitment/Scripts/HSEApprove.js",
                OperationColumns = true,
                LoadAjaxData = false,
                GridId = "HSEGrid",
                GetDataUrl = "",
                HomePage = nameof(HSEApproveController).Replace("Controller", "") + "/index",
            };
            this.approveLogic=approveLogic??throw new ArgumentNullException(nameof(approveLogic));
            this.configuration=configuration??throw new ArgumentNullException(nameof(configuration));
            this.jobApplicantFileLogic=jobApplicantFileLogic??throw new ArgumentNullException(nameof(jobApplicantFileLogic));
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
            this.userPrincipal=userPrincipal??throw new ArgumentNullException(nameof(userPrincipal));
            this.baseInformationLogic=baseInformationLogic??throw new ArgumentNullException(nameof(baseInformationLogic));
            this.sendAsiaSMSService=sendAsiaSMSService??throw new ArgumentNullException(nameof(sendAsiaSMSService));
        }

        [ControlPanelMenu("HSEApprove", ParentName = "menu", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
        public override IActionResult Save([FromServices] ILogic<HSEGridModel> service, HSEGridModel model)
        {
            if (model.JobApplicantId==0)
            {
                return Json(new { result = "fail", message = localizer["you must edit a row"] });
            }
            else
            {

                var jobApplicantFiles = jobApplicantFileLogic.GetByJobApplicantId(model.JobApplicantId);

                if (jobApplicantFiles.ResultStatus != OperationResultStatus.Successful)
                {
                    return Json(new { result = "fail", message = localizer[jobApplicantFiles.AllMessages] });
                }                

                var files = Request.Form.Files;

                if (files.Count>0)
                {

                    foreach (var file in files)
                    {
                        var contentDisposition = file.ContentDisposition;

                        if (contentDisposition.Contains("Referral"))
                        {
                            var checkForExistReferral = jobApplicantFiles.ResultEntity.Where(x => x.AttachmentTypeId==(int)DocumentType.Referral).FirstOrDefault();
                            if (checkForExistReferral != null)
                                return Json(new { result = "fail", message = localizer["Referral has Already Uploaded"] });
                        }

                        if (contentDisposition.Contains("FileSummary"))
                        {
                            var checkForExistReferral = jobApplicantFiles.ResultEntity.Where(x => x.AttachmentTypeId==(int)DocumentType.FileSummary).FirstOrDefault();
                            if (checkForExistReferral != null)
                                return Json(new { result = "fail", message = localizer["FileSummary has Already Uploaded"] });
                        }
                    }                    

                    var fileUploadResult = jobApplicantFileLogic.SaveFiles(files, model.JobApplicantId, jobApplicantFiles.ResultEntity);

                    var referralUploadResult = fileUploadResult.ResultEntity.Where(x => x.File.ContentDisposition.Contains("Referral")).FirstOrDefault();
                    if (referralUploadResult != null)
                        model.ReferralAtachmentId=referralUploadResult.AttachmentId;

                    var fileSummarylUploadResult = fileUploadResult.ResultEntity.Where(x => x.File.ContentDisposition.Contains("FileSummary")).FirstOrDefault();
                    if (fileSummarylUploadResult != null)
                        model.FileSummaryAttchmanetId=fileSummarylUploadResult.AttachmentId;
                }


                if (model.OccupationalMedicineApproveStatus==Enums.OccupationalMedicineApproveStatus.NoAction)
                {

                    return Json(new { result = "fail", message = localizer["Select Approve Status"] });

                }

                if (model.OccupationalMedicineApproveStatus==Enums.OccupationalMedicineApproveStatus.DisApprove && string.IsNullOrEmpty(model.OccupationalMedicineRemarks))
                {

                    return Json(new { result = "fail", message = localizer["Due To Reject You Must Insert Reason"] });
                }

                if (model.OccupationalMedicineApproveStatus==Enums.OccupationalMedicineApproveStatus.Referral && string.IsNullOrEmpty(model.OccupationalMedicineRemarks))
                {

                    return Json(new { result = "fail", message = localizer["Due To Referral You Must Insert Reason"] });
                }

                if (model.OccupationalMedicineApproveStatus==Enums.OccupationalMedicineApproveStatus.Conditionally  && string.IsNullOrEmpty(model.OccupationalMedicineRemarks))
                {
                    return Json(new { result = "fail", message = localizer["Due To Conditionally You Must Insert Reason"] });
                }

                var relatedJobApplicant = approveLogic.GetByJobApplicantId(model.JobApplicantId);
                if (relatedJobApplicant.ResultStatus != OperationResultStatus.Successful || relatedJobApplicant.ResultEntity is null)
                {
                    return Json(new { result = "fail", message = localizer[relatedJobApplicant.AllMessages] });
                }

                if (relatedJobApplicant.ResultEntity.BaseInformationApproveStatus==Enums.ApproveStatus.FinalApprove)
                {
                    return Json(new { result = "fail", message = localizer["Approved Data is not oermitted to Edit"] });
                }

                model.ProcessStatus=Enums.ProcessStatus.ApproveHSE;
                model.OccupationalMedicineApprovedBy=userPrincipal.CurrentUserId;

                sendAsiaSMSService.SendMessage(new Teram.Module.SmsSender.Models.AsiaSms.SendSmsModel
                {
                    Receivers="09014988824,09135748102,09135653195",
                    //Receivers="09135653195",
                    SmsText=$"وضعیت تاییدیه طب کار {relatedJobApplicant.ResultEntity.FirstName} {relatedJobApplicant.ResultEntity.LastName} در این لحظه ثبت گردید"
                });

                return base.Save(service, model);
            }
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetJobAllicants(DatatablesSentModel model, string firstName, string lastName, string personnelCode, string nationalCode, bool viewInprogressStatus , FlowType? flowType , ProcessStatus? processStatus )
        {
            flowType=FlowType.JobApplicant;

            var jobApllicantsResults = approveLogic.GetHSEDataByFilter(firstName, lastName, personnelCode, nationalCode, viewInprogressStatus, flowType, processStatus, model.Start, model.Length);
            if (jobApllicantsResults.ResultStatus != OperationResultStatus.Successful || jobApllicantsResults.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[jobApllicantsResults.AllMessages] });
            }
            var totalCount = jobApllicantsResults?.Count ?? 0;

            if (jobApllicantsResults.ResultEntity!=null && jobApllicantsResults.ResultEntity.Any())
            {

                var approverUserName = jobApllicantsResults.ResultEntity.Where(x => x.OccupationalMedicineApprovedBy.HasValue).Select(x => x.OccupationalMedicineApprovedBy.Value).ToList();

                var usersInfo = userSharedService.GetUserInfos(approverUserName);

                var jobApplicantIds = jobApllicantsResults.ResultEntity.Select(x => x.JobApplicantId).ToList();

                var baseInformationData = baseInformationLogic.GetByJobApplicantIds(jobApplicantIds);


                foreach (var item in jobApllicantsResults.ResultEntity)
                {
                    var approverInfo = (item.OccupationalMedicineApprovedBy.HasValue) ? usersInfo.FirstOrDefault(x => x.UserId==item.OccupationalMedicineApprovedBy.Value) : null;

                    var baseInformationInfo = baseInformationData.ResultEntity.FirstOrDefault(x => x.JobApplicantId==item.JobApplicantId);

                    if (approverInfo!=null)
                    {
                        item.OccupationalMedicineApprovedByName=approverInfo.Fullname;
                    }

                    if (baseInformationInfo!=null)
                    {

                        item.FullName=$"{baseInformationInfo.Name} {baseInformationInfo.Lastname}";
                    }
                }
            }

            return Json(new { model.Draw, recordsTotal = totalCount, recordsFiltered = totalCount, data = jobApllicantsResults?.ResultEntity, error = "", result = "ok" });
        }
    }
}
