using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.FileUploader.Models;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.Module.AttachmentsManagement.Models;
using Teram.Module.Authentication.Models;
using Teram.Module.SmsSender.Models.AsiaSms;
using Teram.Module.SmsSender.Services;
using Teram.Module.SmsSender.Services.AsiaSms;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Exceptions;
using Teram.Web.Core.Model;
using Teram.Web.Core.Security;

namespace Teram.HR.Module.Recruitment.Controllers
{
    public class JobApplicantController : ControlPanelBaseController<JobApplicantModel, JobApplicant, int>
    {
        private readonly IMajorLogic majorLogic;
        private readonly IBaseInformationLogic baseInformationLogic;
        private readonly IUserPrincipal userPrincipal;
        private readonly IUserSharedService userSharedService;
        private readonly IJobApplicantFileLogic jobApplicantFileLogic;
        private readonly ISendAsiaSMSService sendAsiaSMSService;
        private readonly IJobApplicantLogic jobApplicantLogic;
        private readonly IConfiguration configuration;

        public JobApplicantController(ILogger<JobApplicantController> logger
            , IStringLocalizer<JobApplicantController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer, IMajorLogic majorLogic, IBaseInformationLogic baseInformationLogic,
            IUserPrincipal userPrincipal, IUserSharedService userSharedService, IJobApplicantFileLogic jobApplicantFileLogic, ISendAsiaSMSService sendAsiaSMSService,
            IJobApplicantLogic jobApplicantLogic, IConfiguration configuration)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<JobApplicantModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["JobApplicant"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["JobApplicant"],
                OperationColumns = true,
                HomePage = nameof(JobApplicantController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/HR/Module/Recruitment/Scripts/JobApplicant.js",
                ModelData = new JobApplicantModel(),
                GridId = "JobApplicantGrid",
                GetDataUrl = "",
                LoadAjaxData = false,
            };

            var isAdmin = userPrincipal.CurrentUser.HasClaim("Permission", ":JobApplicant:AdminPermission");
            Model.ModelData.IsAdmin=isAdmin;
            Model.HasGrid=isAdmin;

            var userJobApplicantResult = jobApplicantLogic.GetByUserId(userPrincipal.CurrentUserId);

            if (userJobApplicantResult.ResultStatus == OperationResultStatus.Successful && userJobApplicantResult.ResultEntity is not null)
            {
                if (userJobApplicantResult.ResultEntity.ProcessStatus<ProcessStatus.BaseInformationAdded)
                {
                    Model.ModelData.IsShow=false;
                }
                else
                {
                    Model.ModelData.IsShow=true;
                }

                var baseInformation = baseInformationLogic.GetByJobApplicantId(userJobApplicantResult.ResultEntity.JobApplicantId);
                if (baseInformation.ResultStatus == OperationResultStatus.Successful && baseInformation.ResultEntity is not null)
                {
                    Model.ModelData.Gender = baseInformation.ResultEntity.Gender;
                    Model.ModelData.NationalCode=baseInformation.ResultEntity.NationalCode;
                    Model.ModelData.JobApplicantId=baseInformation.ResultEntity.JobApplicantId;
                    Model.ModelData.ChildCount=baseInformation.ResultEntity.ChildCount;
                    Model.ModelData.MarriageStatus=baseInformation.ResultEntity.MarriageStatus;
                }
                Model.ModelData.JobApplicantId = userJobApplicantResult.ResultEntity.JobApplicantId;
            }

            this.majorLogic = majorLogic ?? throw new ArgumentNullException(nameof(majorLogic));
            this.baseInformationLogic = baseInformationLogic ?? throw new ArgumentNullException(nameof(baseInformationLogic));
            this.userPrincipal = userPrincipal ?? throw new ArgumentNullException(nameof(userPrincipal));
            this.userSharedService = userSharedService ?? throw new ArgumentNullException(nameof(userSharedService));
            this.jobApplicantFileLogic = jobApplicantFileLogic ?? throw new ArgumentNullException(nameof(jobApplicantFileLogic));
            this.sendAsiaSMSService=sendAsiaSMSService??throw new ArgumentNullException(nameof(sendAsiaSMSService));
            this.jobApplicantLogic = jobApplicantLogic ?? throw new ArgumentNullException(nameof(jobApplicantLogic));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [ControlPanelMenu("JobApplicant", ParentName = "ManageJobApplicants", Icon = "fa fa-file-image", PanelType = PanelType.User, Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar, Order = 2)]
        public IActionResult Index()
        {
            return View(Model);
        }

        public IActionResult GetJobApplicantById(int jobApplicantId)
        {

            var data = jobApplicantLogic.GetById(jobApplicantId);

            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[data.AllMessages] });
            }
            return Json(new { result = "ok", data = data.ResultEntity, message = localizer[data.AllMessages] });
        }

        protected override void ModifyItem(ILogic<JobApplicantModel> service, int id)
        {
            var isAdmin = userPrincipal.CurrentUser.HasClaim("Permission", ":JobApplicant:AdminPermission");
            Model.ModelData.IsAdmin=isAdmin;

            var jobApplicantResult = jobApplicantLogic.GetRow(id);
            if (jobApplicantResult.ResultEntity.ProcessStatus<ProcessStatus.BaseInformationAdded)
            {
                Model.ModelData.IsShow=false;
                Model.HasGrid=false;
            }
            else
            {
                Model.ModelData.IsShow=true;
                Model.HasGrid=true;
            }
            var baseInformation = baseInformationLogic.GetByJobApplicantId(id);
            if (baseInformation.ResultStatus == OperationResultStatus.Successful && baseInformation.ResultEntity is not null)
            {
                Model.ModelData.Gender = baseInformation.ResultEntity.Gender;
                Model.ModelData.NationalCode=baseInformation.ResultEntity.NationalCode;
                Model.ModelData.JobApplicantId=baseInformation.ResultEntity.JobApplicantId;
                Model.ModelData.ChildCount=baseInformation.ResultEntity.ChildCount;
                Model.ModelData.MarriageStatus=baseInformation.ResultEntity.MarriageStatus;
            }
        }


        public IActionResult CanApproveFileAfterFinalApprove()
        {

            return Content("This is Not Actual Method");

        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetGridData(DatatablesSentModel model, string? personnelCode, string? nationalCode, string? firstName, string? lastName, bool viewInprogressStatus, FlowType? flowType, ProcessStatus? processStatus)
        {
            BusinessOperationResult<List<JobApplicantModel>>? gridData;
            gridData = jobApplicantLogic.GetByFilter(personnelCode, nationalCode, firstName, lastName, viewInprogressStatus, flowType, processStatus, model.Start, model.Length);

            if (gridData.ResultStatus != OperationResultStatus.Successful)
            {
                return Json(new { result = "fail", message = localizer[gridData.AllMessages] });
            }
            var totalCount = gridData?.Count ?? 0;
            return Json(new { model.Draw, recordsTotal = totalCount, recordsFiltered = totalCount, data = gridData?.ResultEntity, error = "", result = "ok" });
        }
        public IActionResult ApproveFile(int jobApplicantFileId)
        {
            var jobApplicantFile = jobApplicantFileLogic.GetRow(jobApplicantFileId);

            if (jobApplicantFile.ResultStatus != OperationResultStatus.Successful || jobApplicantFile.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[jobApplicantFile.AllMessages] });
            }

            var relatedJobApplicant = jobApplicantLogic.GetById(jobApplicantFile.ResultEntity.JobApplicantId);

            if (relatedJobApplicant.ResultStatus != OperationResultStatus.Successful || relatedJobApplicant.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[relatedJobApplicant.AllMessages] });
            }


            var hasPermissionForApproveFileAfterFinalApprove = userPrincipal.CurrentUser.HasClaim("Permission", ":JobApplicant:CanApproveFileAfterFinalApprove");

            if (relatedJobApplicant.ResultEntity.BaseInformationApproveStatus==ApproveStatus.FinalApprove && !hasPermissionForApproveFileAfterFinalApprove)
            {

                return Json(new { result = "fail", message = localizer["Approve And DisApprove Approved Information is Not Permitted"] });
            }

            jobApplicantFile.ResultEntity.ApproveDateTime = DateTime.Now;
            jobApplicantFile.ResultEntity.ApprovedBy = userPrincipal.CurrentUserId;

            if (jobApplicantFile.ResultEntity.ApproveStatus == Enums.ApproveStatus.Disapproved || jobApplicantFile.ResultEntity.ApproveStatus == Enums.ApproveStatus.NoAction)
            {
                jobApplicantFile.ResultEntity.ApproveStatus = Enums.ApproveStatus.FisrtApproved;
            }
            else
            {
                jobApplicantFile.ResultEntity.ApproveStatus = Enums.ApproveStatus.Disapproved;
            }

            var updateResult = jobApplicantFileLogic.Update(jobApplicantFile.ResultEntity);

            return Json(new { result = "ok", data = updateResult.ResultEntity });
        }

        [Display(Description = "AdminPermission")]
        public IActionResult AdminPermission()
        {
            return Content("This is not an actual method");
        }
        public async Task<IActionResult> ShowImagesInfo(int id)
        {
            var downloadurl = configuration.GetSection("Attachment").Get<AttachmentSection>().DownloadUrl;
            var data = jobApplicantFileLogic.GetByJobApplicantId(id);

            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[data.AllMessages] });
            }
            int jobApplicantId = 0;
            foreach (var item in data.ResultEntity)
            {
                item.ImageSrc = $"{downloadurl}" + item.AttachmentId;
                jobApplicantId = item.JobApplicantId;
                DocumentType documentTypeEnum = (DocumentType)item.AttachmentTypeId;
                item.AttachmentTypeName = documentTypeEnum.GetDescription();
            }
            if (data.ResultEntity is not null && data.ResultEntity.Count > 0)
            {
                data.ResultEntity.FirstOrDefault().JobApplicantId = jobApplicantId;
            }
            return PartialView("_ShowImagesInfo", data.ResultEntity);
        }

        public async Task<IActionResult> ShowResumeInfo(int id)
        {
            var result = new List<JobApplicantFileModel>();
            var downloadurl = configuration.GetSection("Attachment").Get<AttachmentSection>().DownloadUrl;
            var data = jobApplicantFileLogic.GetResumeFile(id);

            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[data.AllMessages] });
            }
            int jobApplicantId = 0;
            data.ResultEntity.ImageSrc = $"{downloadurl}" + data.ResultEntity.AttachmentId;
            jobApplicantId = data.ResultEntity.JobApplicantId;
            DocumentType documentTypeEnum = (DocumentType)data.ResultEntity.AttachmentTypeId;
            data.ResultEntity.AttachmentTypeName = documentTypeEnum.GetDescription();
            result.Add(data.ResultEntity);
            return PartialView("_ShowImagesInfo", result);
        }

        [Display(Description = "DeleteFile")]
        [ParentalAuthorize(nameof(Edit))]
        public IActionResult RemoveUploadedFiles([FromServices] IJobApplicantFileLogic jobApplicantFileLogic, int id)
        {
            try
            {
                var data = jobApplicantFileLogic.GetRow(id);

                if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
                {
                    return Json(new { result = "fail", message = data.AllMessages });
                }

                var relatedJobApplicant = jobApplicantLogic.GetById(data.ResultEntity.JobApplicantId);

                if (relatedJobApplicant.ResultStatus != OperationResultStatus.Successful || relatedJobApplicant.ResultEntity is null)
                {
                    return Json(new { result = "fail", message = localizer["Job Applicant Not Found"] });
                }

                if ((relatedJobApplicant.ResultEntity.BaseInformationApproveStatus==ApproveStatus.FisrtApproved) ||
                     data.ResultEntity.ApproveStatus==ApproveStatus.FisrtApproved)
                {

                    return Json(new { result = "fail", message = localizer["Approved Information Is Not Permitted For Delete"] });
                }
                data.ResultEntity.DeletedOn = DateTime.Now;
                jobApplicantFileLogic.Update(data.ResultEntity);

                return Json(new { result = "ok", message = localizer["Success Remove"] });
            }
            catch (UIException ex)
            {
                return Json(new { result = "fail", message = ex.Message });
            }
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult DownLoadAll([FromRoute] int id)
        {


            var filesData = jobApplicantFileLogic.GetByJobApplicantId(id);
            if (filesData.ResultStatus != OperationResultStatus.Successful || filesData.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[filesData.AllMessages] });
            }

            var jobApplicantResult = jobApplicantLogic.GetRow(id);

            if (jobApplicantResult.ResultStatus != OperationResultStatus.Successful || jobApplicantResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[jobApplicantResult.AllMessages] });
            }

            var zipName = $"{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";
            using (MemoryStream ms = new MemoryStream())
            {
                using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
                {

                    filesData.ResultEntity.ToList().ForEach(file =>
                    {
                        DocumentType documentTypeEnum = (DocumentType)file.AttachmentTypeId;
                        var enumDisplayStatus = (DocumentType)documentTypeEnum;
                        var fileBase64String = jobApplicantFileLogic.GetFileBytes(file.AttachmentId).Result.ResultEntity;
                        var entry = zip.CreateEntry(enumDisplayStatus.GetDescription() + "." + file.FileExtension);
                        using var fileStream = new MemoryStream(Convert.FromBase64String(fileBase64String));
                        using var entryStream = entry.Open();
                        fileStream.CopyTo(entryStream);
                    });
                }
                return File(ms.ToArray(), "application/zip", zipName);
            }
        }

        public IActionResult GetJobApplicantAttachmentTypes(int id)
        {
            var attachmentTypes = new List<int>();
            var jobApplicantFilesResult = jobApplicantFileLogic.GetByJobApplicantId(id);
            if (jobApplicantFilesResult.ResultStatus != OperationResultStatus.Successful || jobApplicantFilesResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[jobApplicantFilesResult.AllMessages] });
            }
            attachmentTypes = jobApplicantFilesResult.ResultEntity.Select(x => x.AttachmentTypeId).ToList();
            return Json(new { result = "ok", message = localizer[jobApplicantFilesResult.AllMessages], data = attachmentTypes });
        }

        [Display(Description = "ShowUploadedFile")]
        public IActionResult ShowUploadedFile(int jobApplicantId, int attachmentTypeId)
        {
            try
            {
                DocumentType documentTypeEnum = (DocumentType)attachmentTypeId;
                var enumDisplayStatus = (DocumentType)documentTypeEnum;
                string uploaderClass = $"fileuploader_{enumDisplayStatus.ToString()}";
                var showAttachmentResult = jobApplicantFileLogic.GetAttachmentsByJobApplicantIdAndAttachmentTypeId(jobApplicantId, attachmentTypeId);
                if (showAttachmentResult.ResultStatus == OperationResultStatus.Successful)
                {
                    showAttachmentResult.ResultEntity.UploaderName = uploaderClass;
                    return Json(showAttachmentResult.ResultEntity);
                }
                else
                {
                    return Json(new { result = "fail", message = localizer["Show uploaded file error"] });
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "fail", message = ex.Message });
            }
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult SaveJobApplicant(JobApplicantModel model)
        {

            var requiredAttachments = new List<DocumentType>();

            if (model.JobApplicantId == 0)
            {
                return Json(new { result = "fail", message = localizer["Base Information Not Found"] });
            }

            var relatedJobApplicant = jobApplicantLogic.GetById(model.JobApplicantId);           


            if (relatedJobApplicant.ResultStatus != OperationResultStatus.Successful || relatedJobApplicant.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[relatedJobApplicant.AllMessages] });
            }

            if (relatedJobApplicant.ResultEntity.ProcessStatus>ProcessStatus.DoumentsUploaded) {

                return Json(new { result = "fail", message = localizer["Due To Workflow Progress Upload Is Not Permitted"] });

            }

            var filesCount = 0;
            var files = Request.Form.Files;
            var hasLargFile = false;

            foreach (var file in files)
            {
                long fileSizeInBytes = file.Length;
                double fileSizeInKilobytes = fileSizeInBytes / 1024.0;
                double fileSizeInMegabytes = fileSizeInKilobytes / 1024.0;

                if (fileSizeInMegabytes > 5)
                {
                    hasLargFile=true;
                }
            }

            if (hasLargFile)
            {
                return Json(new { result = "fail", message = localizer["File Size Must Be Smaller Than 5MB"] });
            }


            var baseInformationResult = baseInformationLogic.GetByJobApplicantId(model.JobApplicantId);

            if (baseInformationResult.ResultStatus != OperationResultStatus.Successful || baseInformationResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer["Base Information Not Found"] });
            }

            if (baseInformationResult.ResultEntity.Gender==GenderType.Male)
            {
                requiredAttachments.Add(DocumentType.OnNationalCard);
                requiredAttachments.Add(DocumentType.BehindNationalCard);
                requiredAttachments.Add(DocumentType.BirthCertificateFirstPage);
                requiredAttachments.Add(DocumentType.BirthCertificateSecondPage);
                requiredAttachments.Add(DocumentType.BirthCertificateThirdPage);
                requiredAttachments.Add(DocumentType.BirthCertificateForthPage);
                requiredAttachments.Add(DocumentType.MilitaryService);
                requiredAttachments.Add(DocumentType.PersonalImage);
            }
            else
            {
                requiredAttachments.Add(DocumentType.OnNationalCard);
                requiredAttachments.Add(DocumentType.BehindNationalCard);
                requiredAttachments.Add(DocumentType.BirthCertificateFirstPage);
                requiredAttachments.Add(DocumentType.PersonalImage);
                requiredAttachments.Add(DocumentType.BirthCertificateSecondPage);
                requiredAttachments.Add(DocumentType.BirthCertificateThirdPage);
                requiredAttachments.Add(DocumentType.BirthCertificateForthPage);
            }

            var uploadedFiles = jobApplicantFileLogic.GetJobApplicantDocumentsExceptResume(relatedJobApplicant.ResultEntity.JobApplicantId);
            if (uploadedFiles.ResultStatus == OperationResultStatus.Successful && uploadedFiles.ResultEntity is not null)
            {
                filesCount=uploadedFiles.ResultEntity.Count();
            }

            filesCount+=files.Count();

            if (filesCount<requiredAttachments.Count)
            {
                return Json(new { result = "fail", message = localizer["Please Upload All Files"] });
            }

            var checkForExistingFiles = jobApplicantFileLogic.GetByJobApplicantId(model.JobApplicantId);

            var fileUploadResult = jobApplicantFileLogic.SaveFiles(files, model.JobApplicantId, checkForExistingFiles.ResultEntity);

            if (fileUploadResult.ResultEntity!=null)
            {
                if (fileUploadResult.ResultStatus != OperationResultStatus.Successful || fileUploadResult.ResultEntity.Select(x => x.IsSuccess).ToList().Contains(false))
                {
                    List<string> messages = fileUploadResult.ResultEntity.Where(x => x.Message!=null && x.Message.Length>1).Select(x => x.Message).ToList();
                    string concatenatedMessages = string.Join("\n", messages);
                    return Json(new { result = "fail", message = concatenatedMessages });
                }
            }
            else
            {
                return Json(new { result = "fail", message = fileUploadResult.AllMessages });
            }
            var finalUploadedFiles = jobApplicantFileLogic.GetByJobApplicantId(model.JobApplicantId);

            var uploadedAttachmentTypes = finalUploadedFiles.ResultEntity.Select(x => x.AttachmentTypeId).ToList();
            List<DocumentType> enumValues = uploadedAttachmentTypes.ConvertAll(value => (DocumentType)Enum.Parse(typeof(DocumentType), value.ToString()));

            bool containsAllRequired = requiredAttachments.All(requiredAttachments => enumValues.Contains(requiredAttachments));

            if (containsAllRequired)
            {
                relatedJobApplicant.ResultEntity.ProcessStatus=ProcessStatus.DoumentsUploaded;
                var updateResult = jobApplicantLogic.Update(relatedJobApplicant.ResultEntity);
                try
                {
                    if (relatedJobApplicant.ResultEntity.FlowType==FlowType.JobApplicant)
                    {

                        sendAsiaSMSService.SendMessage(new SendSmsModel
                        {
                            Receivers=model.MobileNumber,
                            SmsText=$"مدارک شما با موفقیت در سامانه ثبت شد \n ادامه فرآیند : B2n.ir/d36754"
                        });
                    }
                }
                catch (Exception)
                {
                    return Json(new { result = "fail", message = localizer["Error In Connecting To SMS Panel"] });
                }
            }
            else
            {

                return Json(new { result = "fail", message = localizer["For Continue Process You Must Upload All Required Files"] });
            }
            return Json(new { redirectUrl = (relatedJobApplicant.ResultEntity.FlowType==FlowType.JobApplicant) ? "/WorkWithUs/SuccessInfo" : "/WorkWithUs/EmployeeSuccessInfo", result = "ok", message = localizer[fileUploadResult.AllMessages] });
        }
    }
}
