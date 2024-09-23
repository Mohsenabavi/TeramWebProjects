using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Transactions;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.Employees;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.HR.Module.Recruitment.Models.WorkWithUs;
using Teram.HR.Module.Recruitment.Services;
using Teram.Module.FileUploader.Models;
using Teram.Module.GeographicRegion.Logic.Interfaces;
using Teram.Module.SmsSender.Logic.Interfaces;
using Teram.Module.SmsSender.Models.AsiaSms;
using Teram.Module.SmsSender.Services;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Teram.Web.Core.Extensions;
using Teram.Web.Core.Model;
using Teram.Web.Core.Security;

namespace Teram.HR.Module.Recruitment.Controllers
{
    [ControlPanelMenu("ManageJobApplicants", Name = "ManageJobApplicants", Icon = "fa fa-tasks", PanelType = PanelType.User, Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar, Order = 1)]
    public class JobApplicantControlPanelController : ControlPanelBaseController<JobApplicantModel, JobApplicant, int>
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IWorkerJobBackgroundLogic workerJobBackgroundLogic;
        private readonly IEmployeeJobBackgroundLogic employeeJobBackgroundLogic;
        private readonly IBaseInformationLogic baseInformationLogic;
        private readonly PdfConverterService pdfConverterService;
        private readonly IUserPrincipal userPrincipal;
        private readonly IJobApplicantFileLogic jobApplicantFileLogic;
        private readonly ISendAsiaSMSService sendAsiaSMSService;
        private readonly IUserSharedService userSharedService;
        private readonly IJobApplicantLogic jobApplicantLogic;
        private readonly IJobPositionLogic jobPositionLogic;
        private readonly IGeographicRegionLogic geographicRegionLogic;
        private readonly IJobApplicantsIntroductionLetterLogic jobApplicantsIntroductionLetterLogic;
        private readonly IIntroductionGenerationService introductionGenerationService;

        public JobApplicantControlPanelController(ILogger<JobApplicantControlPanelController> logger, IWebHostEnvironment webHostEnvironment, IWorkerJobBackgroundLogic workerJobBackgroundLogic, IEmployeeJobBackgroundLogic employeeJobBackgroundLogic
            , IStringLocalizer<JobApplicantControlPanelController> localizer, IBaseInformationLogic baseInformationLogic, PdfConverterService pdfConverterService,
            IStringLocalizer<SharedResource> sharedLocalizer, IUserPrincipal userPrincipal, IJobApplicantFileLogic jobApplicantFileLogic, ISendAsiaSMSService sendAsiaSMSService,
            IUserSharedService userSharedService, IJobApplicantLogic jobApplicantLogic, IJobPositionLogic jobPositionLogic, IGeographicRegionLogic geographicRegionLogic,
            IJobApplicantsIntroductionLetterLogic jobApplicantsIntroductionLetterLogic, IIntroductionGenerationService introductionGenerationService)
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
                GridId = "JobApplicantGrid",
                GetDataUrl = "",
                LoadAjaxData = false,
                HomePage = nameof(JobApplicantControlPanelController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/HR/Module/Recruitment/Scripts/JobApplicantControlPanel.js",
                HasToolbar = true,
                ToolbarName="_adminToolbar"
            };
            this.webHostEnvironment=webHostEnvironment??throw new ArgumentNullException(nameof(webHostEnvironment));
            this.workerJobBackgroundLogic=workerJobBackgroundLogic;
            this.employeeJobBackgroundLogic=employeeJobBackgroundLogic??throw new ArgumentNullException(nameof(employeeJobBackgroundLogic));
            this.baseInformationLogic=baseInformationLogic??throw new ArgumentNullException(nameof(baseInformationLogic));
            this.pdfConverterService=pdfConverterService??throw new ArgumentNullException(nameof(pdfConverterService));
            this.userPrincipal = userPrincipal ?? throw new ArgumentNullException(nameof(userPrincipal));
            this.jobApplicantFileLogic=jobApplicantFileLogic??throw new ArgumentNullException(nameof(jobApplicantFileLogic));
            this.sendAsiaSMSService=sendAsiaSMSService??throw new ArgumentNullException(nameof(sendAsiaSMSService));
            this.userSharedService = userSharedService ?? throw new ArgumentNullException(nameof(userSharedService));
            this.jobApplicantLogic=jobApplicantLogic??throw new ArgumentNullException(nameof(jobApplicantLogic));
            this.jobPositionLogic=jobPositionLogic??throw new ArgumentNullException(nameof(jobPositionLogic));
            this.geographicRegionLogic=geographicRegionLogic??throw new ArgumentNullException(nameof(geographicRegionLogic));
            this.jobApplicantsIntroductionLetterLogic=jobApplicantsIntroductionLetterLogic??throw new ArgumentNullException(nameof(jobApplicantsIntroductionLetterLogic));
            this.introductionGenerationService=introductionGenerationService??throw new ArgumentNullException(nameof(introductionGenerationService));
        }

        [ControlPanelMenu("JobApplicant", ParentName = "ManageJobApplicants", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar, Order = 1)]


        #region Control Panel Methods
        public IActionResult Index()
        {
            ViewBag.JobPositions=JobPositions();
            return View(Model);
        }


        [ParentalAuthorize(nameof(Index))]
        public async Task<IActionResult> ImportFromExcel()
        {
            try
            {
                if (!Request.Form.Files.Any())
                {
                    return Json(new { Result = "fail", message = "هیچ فایلی انتخاب نشده است" });
                }
                var file = Request.Form.Files[0];

                var employeeList = new List<ImportJobApplicantsEmployeeModel>();

                using var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                employeeList = employeeList.ImportFromExcel(ms).ToList();

                var jobPositions = jobPositionLogic.GetActiveJobPositions();
                var insertedRecords = 0;
                var failedRescords = 0;
                var duplicateNationalCodes = new List<string>();

                foreach (var item in employeeList)
                {
                    var checkForExist = jobApplicantLogic.GetByNationalCode(item.NationalCode);

                    if (checkForExist.ResultEntity != null)
                    {
                        duplicateNationalCodes.Add(checkForExist.ResultEntity.NationalCode);
                        continue;
                    }


                    var relatedUserId = Guid.Empty;
                    var relatedJobPosition = jobPositions.ResultEntity.Where(x => x.Title==item.JobPositionTitle.Trim()).FirstOrDefault();
                    var relatedUserInfo = await userSharedService.GetUserInfoByUserName(item.NationalCode);

                    if (relatedUserInfo!=null)
                    {
                        var userRoleResult = await userSharedService.AddToRoleAsync(relatedUserInfo, "Employed");
                        relatedUserId=relatedUserInfo.UserId;
                    }

                    var insertModel = new JobApplicantModel
                    {
                        FirstName=item.FirstName,
                        LastName=item.LastName,
                        FlowType=FlowType.Employed,
                        FatherName=item.FatherName,
                        MobileNumber=(item.MobileNumber!=null) ? item.MobileNumber.Trim() : "0000000000",
                        PersonnelCode=int.Parse(item.PersonnelCode).ToString(),
                        NationalCode=item.NationalCode.Trim(),
                        IdentityNumber=item.IdentityNumber,
                        JobCategory=JobCategory.Worker,
                        JobPositionId=(relatedJobPosition != null) ? relatedJobPosition.JobPositionId : 169,
                        OccupationalMedicineRemarks=(relatedJobPosition != null) ? "" : item.JobPositionTitle,
                        PromissoryNoteAmount="Not Provided",
                        CreateDate=DateTime.Now,
                        CreatedBy=userPrincipal.CurrentUserId,
                        UserId=relatedUserId
                    };

                    var addResult = jobApplicantLogic.AddNew(insertModel);

                    if (addResult.ResultStatus != OperationResultStatus.Successful || addResult.ResultEntity is null)
                    {
                        failedRescords++;
                        return Json(new { result = "fail", message = localizer[addResult.AllMessages] });
                    }

                    insertedRecords++;
                }


                return Json(new { Result = "ok" });
            }
            catch (Exception)
            {
                return Json(new { Result = "fail", message = "درج برخی از ردیف ها با خطا مواجه شده" });
            }
        }
        private List<SelectListItem> JobPositions()
        {
            var result = new List<SelectListItem>();
            var data = jobPositionLogic.GetActiveJobPositions();
            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }
            return data.ResultEntity.ToSelectList(nameof(JobPositionModel.Title), nameof(JobPositionModel.JobPositionId));
        }
        public IActionResult GetJobApplicantsData(DatatablesSentModel model, string? personnelCode, string? nationalCode, string firstName, string lastName, bool viewInprogressStatus, FlowType? flowType, ProcessStatus? processStatus)
        {

            var jobApplicantsResult = jobApplicantLogic.GetByFilter(personnelCode, nationalCode, firstName, lastName, viewInprogressStatus, flowType, processStatus, model.Start, model.Length);

            if (jobApplicantsResult.ResultStatus != OperationResultStatus.Successful || jobApplicantsResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[jobApplicantsResult.AllMessages] });
            }

            var creatorUserIds = jobApplicantsResult.ResultEntity.Select(x => x.CreatedBy).ToList();
            var usersInfo = userSharedService.GetUserInfos(creatorUserIds);
            foreach (var item in jobApplicantsResult.ResultEntity)
            {
                var creatorUsername = usersInfo.FirstOrDefault(x => x.UserId==item.CreatedBy);
                item.CreatedByUserName=(creatorUsername!=null) ? creatorUsername.Fullname : "-";
            }


            var totalCount = jobApplicantsResult?.Count ?? 0;
            return Json(new { model.Draw, recordsTotal = totalCount, recordsFiltered = totalCount, data = jobApplicantsResult?.ResultEntity, error = "", result = "ok" });
        }

        protected override void ModifyItem(ILogic<JobApplicantModel> service, int id)
        {
            ViewBag.JobPositions=JobPositions();
            base.ModifyItem(service, id);
        }

        public override IActionResult Remove([FromServices] ILogic<JobApplicantModel> service, int key)
        {
            var relatedJobApplicantRecord = service.GetRow(key);

            if (relatedJobApplicantRecord.ResultStatus != OperationResultStatus.Successful || relatedJobApplicantRecord.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[relatedJobApplicantRecord.AllMessages] });
            }

            if (relatedJobApplicantRecord.ResultEntity.BaseInformationApproveStatus==ApproveStatus.FisrtApproved)
            {

                return Json(new { result = "fail", message = localizer["Approved Information Is Not Permitted For Delete"] });
            }

            var userId = relatedJobApplicantRecord.ResultEntity.UserId;
            var userInfo = userSharedService.GetUserById(userId).Result;

            if (userInfo == null)
            {
                return Json(new { result = "fail", message = localizer[relatedJobApplicantRecord.AllMessages] });
            }
            var removeUserInfo = userSharedService.DeleteUser(userId).Result;

            if (removeUserInfo.Succeeded)
            {
                return base.Remove(service, key);
            }
            return Json(new { result = "fail", message = localizer[relatedJobApplicantRecord.AllMessages] });
        }


        public IActionResult PrintExcel(string? personnelCode, string? nationalCode, string firstName, string lastName, bool viewInprogressStatus, FlowType? flowType, ProcessStatus? processStatus)
        {

            var jobApplicantResult = jobApplicantLogic.GetByFilter(personnelCode, nationalCode, firstName, lastName, viewInprogressStatus, flowType, processStatus);

            if (jobApplicantResult.ResultStatus != OperationResultStatus.Successful || jobApplicantResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[jobApplicantResult.AllMessages] });
            }

            var jobApplicantIds = jobApplicantResult.ResultEntity.Select(x => x.JobApplicantId).ToList();

            var relatedBaseInformation = baseInformationLogic.GetByJobApplicantIds(jobApplicantIds);

            if (relatedBaseInformation.ResultStatus != OperationResultStatus.Successful || relatedBaseInformation.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[relatedBaseInformation.AllMessages] });
            }

            foreach (var item in relatedBaseInformation.ResultEntity)
            {
                if (item.BirthLocationId.HasValue)
                {
                    var birthLocationInfo = geographicRegionLogic.GetById(item.BirthLocationId.Value);

                    if (birthLocationInfo.ResultStatus == OperationResultStatus.Successful && birthLocationInfo.ResultEntity is not null)
                    {
                        item.BirthLocationName = birthLocationInfo.ResultEntity.Name;
                    }
                }
            }

            var excelData = relatedBaseInformation.ResultEntity.ExportListExcel("مشخصات پرسنل استخدامی");
            if (excelData is null)
            {
                return Json(new { result = "fail", total = 0, rows = new List<BaseInformationModel>(), message = localizer["Unable to create file due to technical problems."] });
            }

            var fileName = "JobApplicants-" + DateTime.Now.ToPersianDate();
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + ".xlsx");

        }

        public IActionResult GetSMSHistory([FromServices] ISMSHistoryLogic sMSHistoryLogic, string mobileNumber)
        {

            var smsHistory = sMSHistoryLogic.GetSMSByRecieverNumber(mobileNumber);

            if (smsHistory.ResultStatus != OperationResultStatus.Successful || smsHistory.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[smsHistory.AllMessages] });
            }

            return PartialView("_SmsHistory", smsHistory.ResultEntity);
        }

        #endregion

        #region Introductions Methods
        public IActionResult IssuanceIntroductionLetters(int id)
        {
            var resultModel = new JobApplicantApproveResultModel
            {
                Message=localizer["Introduction Letters Created Successfully"],
                OperationResultStatus=OperationResultStatus.Successful
            };

            var personnalImageInfo = jobApplicantFileLogic.GetPersonalImage(id);

            if (personnalImageInfo.ResultStatus != OperationResultStatus.Successful || personnalImageInfo.ResultEntity is null)
            {
                resultModel.Errors.Add(localizer["PersonnelImageNotFound"]);
            }

            var relatedJobApplicant = jobApplicantLogic.GetById(id);

            if (relatedJobApplicant.ResultStatus != OperationResultStatus.Successful || relatedJobApplicant.ResultEntity is null)
            {
                resultModel.Errors.Add(localizer["JobApplicant Not Saved"]);
            }

            else if (relatedJobApplicant.ResultEntity.ProcessStatus<ProcessStatus.ApproveJobBackground)
            {

                resultModel.Errors.Add(localizer["Introduction Issuance Is Available After background Job Approve"]);
            }

            var backgroundJobErrors = CheckBackgroundJobApproval(relatedJobApplicant.ResultEntity);

            if (backgroundJobErrors.Count!=0)
            {
                foreach (var error in backgroundJobErrors)
                {
                    resultModel.Errors.Add(error);
                }
            }

            if (!relatedJobApplicant.ResultEntity.ExpreminetDeadline.HasValue)
            {
                resultModel.Errors.Add(localizer["Expreminet Dead Line Not Found"]);
            }

            if (resultModel.OperationResultStatus==OperationResultStatus.Successful && resultModel.Errors.Count==0)
            {

                var generateIntrusctionResult = introductionGenerationService.CreateIntroductionFiles(relatedJobApplicant.ResultEntity);

                if (generateIntrusctionResult.OperationResultStatus != OperationResultStatus.Successful || !generateIntrusctionResult.FileLinks.Any())
                {
                    resultModel.Errors.Add(localizer["Error In Generate Files"]);

                }
                resultModel.FileLinks=generateIntrusctionResult.FileLinks;
            }

            if (resultModel.Errors.Count!=0)
            {
                resultModel.OperationResultStatus=OperationResultStatus.Fail;
                resultModel.Message=localizer["Error In Generate Files"];
                return PartialView("_ApproveJobApplicantResult", resultModel);
            }

            try
            {
                sendAsiaSMSService.SendMessage(new SendSmsModel
                {
                    Receivers = relatedJobApplicant.ResultEntity.MobileNumber,
                    SmsText = $"حساب کاربری شما در سامانه منابع انسانی ایجاد شد \nنام کاربری : {relatedJobApplicant.ResultEntity.NationalCode} \nکلمه عبور : {relatedJobApplicant.ResultEntity.NationalCode} \nلینک ورود : B2n.ir/w95030 \nمعرفی نامه های شما صادر گردید\nلینک مشاهده معرفی نامه ها : B2n.ir/d36754 \nپس از انجام آزمایشات در بخش اعلام انجام مراحل اطلاعات لازم را ثبت نمایید \nلینک اعلام مراحل : B2n.ir/z46407"
                });
            }
            catch (Exception)
            {
                resultModel.Errors.Add(localizer["Error In Connecting To SMS Panel"]);
                resultModel.OperationResultStatus=OperationResultStatus.Fail;
            }

            return PartialView("_ApproveJobApplicantResult", resultModel);
        }

        public IActionResult GetIntroductionLetters(int id)
        {
            var jobApplicantLettrs = jobApplicantsIntroductionLetterLogic.GetByJobApplicantId(id);

            if (jobApplicantLettrs.ResultStatus != OperationResultStatus.Successful || !jobApplicantLettrs.ResultEntity.Any())
            {
                var returnErrorModel = new JobApplicantApproveResultModel
                {
                    OperationResultStatus=OperationResultStatus.Fail,
                    Message=localizer["Introdction Letters Not Found"],
                };
                return PartialView("_ApproveJobApplicantResult", returnErrorModel);
            }

            var links = jobApplicantLettrs.ResultEntity.Select(x => new FileLinkModel
            {
                Link=x.FileUrl,
                Title=x.IntroductionLetterType.GetDescription()
            }).ToList();

            var returnModel = new JobApplicantApproveResultModel
            {
                OperationResultStatus=OperationResultStatus.Successful,
                Message=localizer["Introdction Letters"],
                FileLinks=links,
            };
            return PartialView("_ApproveJobApplicantResult", returnModel);
        }

        #endregion

        #region Authorization Actions 

        public IActionResult CanEditAfterFinalApprove()
        {

            return Content("This is not Actual Action");
        }

        #endregion

        #region ViewSteps
        public IActionResult ViewStepsStatus(int jobApplicantId)
        {

            var resultModel = new List<StepModel>();

            var jobApplicantResult = jobApplicantLogic.GetById(jobApplicantId);

            if (jobApplicantResult.ResultStatus == OperationResultStatus.Successful && jobApplicantResult.ResultEntity is null)
            {
                resultModel.Add(new StepModel { IsCompleted=false, IsDone=false, Step=localizer["Job Applicant Step"], Message=localizer["JobApplicant Not Saved"] });
            }

            else
            {
                resultModel.Add(new StepModel { IsCompleted=true, IsDone=true, Step=localizer["Job Applicant Step"], Message=localizer["JobApplicant Saved"] });

            }

            var jobCategory = jobApplicantResult.ResultEntity.JobCategory;
            if (jobCategory==JobCategory.Worker)
            {

                var relatedJobBackground = workerJobBackgroundLogic.GetByJobApplicantId(jobApplicantResult.ResultEntity.JobApplicantId);

                if (relatedJobBackground.ResultStatus == OperationResultStatus.Successful && relatedJobBackground.ResultEntity is null)
                {
                    resultModel.Add(new StepModel { IsCompleted=false, IsDone=false, Step=localizer["Background Job Step"], Message=localizer["Background Job Step Not Done"] });
                }

                if (relatedJobBackground?.ResultEntity!=null && relatedJobBackground.ResultEntity.ApproveStatus!=BackgroundJobApproveStatus.Approved)
                {
                    resultModel.Add(new StepModel { IsCompleted=false, IsDone=false, Step=localizer["Background Job Approve Step"], Message=localizer["Background Job Step Not Approved"] });
                }

                if (relatedJobBackground?.ResultEntity!=null && relatedJobBackground.ResultEntity.ApproveStatus==BackgroundJobApproveStatus.Approved)
                {
                    resultModel.Add(new StepModel { IsCompleted=true, IsDone=true, Step=localizer["Background Job Approve Step"], Message=localizer["Background Job Step Approved"] });
                }
            }

            if (jobCategory==JobCategory.Employee)
            {
                var relatedJobBackground = employeeJobBackgroundLogic.GetByJobApplicantId(jobApplicantResult.ResultEntity.JobApplicantId);

                if (relatedJobBackground.ResultStatus == OperationResultStatus.Successful && relatedJobBackground.ResultEntity is null)
                {
                    resultModel.Add(new StepModel { IsCompleted=false, IsDone=false, Step=localizer["Background Job Step"], Message=localizer["Background Job Step Not Done"] });
                }

                if (relatedJobBackground?.ResultEntity!=null && relatedJobBackground.ResultEntity.ApproveStatus!=BackgroundJobApproveStatus.Approved)
                {
                    resultModel.Add(new StepModel { IsCompleted=false, IsDone=false, Step=localizer["Background Job Approve Step"], Message=localizer["Background Job Step Not Approved"] });
                }

                if (relatedJobBackground?.ResultEntity!=null && relatedJobBackground.ResultEntity.ApproveStatus==BackgroundJobApproveStatus.Approved)
                {
                    resultModel.Add(new StepModel { IsCompleted=true, IsDone=true, Step=localizer["Background Job Approve Step"], Message=localizer["Background Job Step Approved"] });
                }
            }


            var introductionLetters = jobApplicantsIntroductionLetterLogic.GetByJobApplicantId(jobApplicantId);

            if (introductionLetters.ResultStatus == OperationResultStatus.Successful && introductionLetters.ResultEntity.Count==0)
            {
                resultModel.Add(new StepModel { IsCompleted=false, IsDone=false, Step=localizer["Introductyion Letters Step"], Message=localizer["Introductyion Letters Step Not Done"] });
            }
            if (introductionLetters.ResultStatus == OperationResultStatus.Successful && introductionLetters?.ResultEntity?.Count>0)
            {
                resultModel.Add(new StepModel { IsCompleted=true, IsDone=true, Step=localizer["Introductyion Letters Step"], Message=localizer["Introductyion Letters Step Done"] });
            }



            var baseInformationResult = baseInformationLogic.GetByJobApplicantId(jobApplicantId);

            if (baseInformationResult.ResultStatus == OperationResultStatus.Successful && baseInformationResult.ResultEntity is null)
            {
                resultModel.Add(new StepModel { IsCompleted=false, IsDone=false, Step=localizer["Base Information Step"], Message=localizer["BaseInformation Not Saved"] });
            }
            else
            {
                resultModel.Add(new StepModel { IsCompleted=true, IsDone=true, Step=localizer["Base Information Step"], Message=localizer["BaseInformation Saved"] });
            }

            var checkJobApplicantFilesResult = CheckJobApplicantFiles(jobApplicantResult.ResultEntity, baseInformationResult.ResultEntity);

            if (checkJobApplicantFilesResult.Count==0)
            {
                resultModel.Add(new StepModel { IsCompleted=true, IsDone=true, Step=localizer["Upload Files Step"], Message=localizer["Upload Files Step Done"] });
            }

            else
            {
                resultModel.Add(new StepModel { IsCompleted=false, IsDone=false, Step=localizer["Upload Files Step"], Message=localizer["Upload Files Step not Done"] });
            }

            var checkUnApprovedDocuments = CheckJobApplicantFilesApproveStatus(jobApplicantResult.ResultEntity, baseInformationResult.ResultEntity);
            if (checkUnApprovedDocuments.Count==0 && checkJobApplicantFilesResult.Count==0)
            {
                resultModel.Add(new StepModel { IsCompleted=true, IsDone=true, Step=localizer["Approve Files Step"], Message=localizer["Files Approved"] });
            }
            else
            {

                resultModel.Add(new StepModel { IsCompleted=false, IsDone=false, Step=localizer["Approve Files Step"], Message=localizer["Some Files Not Approved"] });

            }



            if (!jobApplicantResult.ResultEntity.NoAddictionDone)
            {
                resultModel.Add(new StepModel { IsCompleted=false, IsDone=false, Step=localizer["announcement of NoAddiction Step"], Message=localizer["NoAddictionDone Not announced"] });
            }

            if (jobApplicantResult.ResultEntity.NoAddictionDone)
            {
                resultModel.Add(new StepModel { IsCompleted=true, IsDone=true, Step=localizer["announcement of NoAddiction Step"], Message=localizer["NoAddictionDone announced"] });
            }

            if (!jobApplicantResult.ResultEntity.NoBadBackgroundDone)
            {
                resultModel.Add(new StepModel { IsCompleted=false, IsDone=false, Step=localizer["announcement of NoBadBackground Step"], Message=localizer["NoBadBackgroundDone Not announced"] });
            }

            if (jobApplicantResult.ResultEntity.NoBadBackgroundDone)
            {
                resultModel.Add(new StepModel { IsCompleted=true, IsDone=true, Step=localizer["announcement of NoBadBackground Step"], Message=localizer["NoBadBackgroundDone announced"] });
            }

            if (jobApplicantResult.ResultEntity.OccupationalMedicineApproveStatus==OccupationalMedicineApproveStatus.DisApprove)
            {
                resultModel.Add(new StepModel { IsCompleted=false, IsDone=false, Step=localizer["Occupational Medicine Approve Step"], Message=localizer["Occupational Medicine Result Not Approved"] });
            }

            if (jobApplicantResult.ResultEntity.OccupationalMedicineApproveStatus==OccupationalMedicineApproveStatus.NoAction)
            {
                resultModel.Add(new StepModel { IsCompleted=false, IsDone=false, Step=localizer["Occupational Medicine Approve Step"], Message=localizer["Occupational Medicine Result Not Filled"] });
            }

            if (jobApplicantResult.ResultEntity.OccupationalMedicineApproveStatus==OccupationalMedicineApproveStatus.Referral)
            {
                resultModel.Add(new StepModel { IsCompleted=false, IsDone=false, Step=localizer["Occupational Medicine Approve Step"], Message=localizer["Occupational Medicine Has Referral"] });
            }

            if (jobApplicantResult.ResultEntity.OccupationalMedicineApproveStatus==OccupationalMedicineApproveStatus.Approve)
            {
                resultModel.Add(new StepModel { IsCompleted=true, IsDone=true, Step=localizer["Occupational Medicine Approve Step"], Message=localizer["Occupational Medicine Result Approved"] });
            }

            var lastApproveStatus = jobApplicantLogic.GetById(jobApplicantId);

            if (lastApproveStatus.ResultStatus == OperationResultStatus.Successful && lastApproveStatus.ResultEntity is not null)
            {
                var lastStatus = lastApproveStatus.ResultEntity.BaseInformationApproveStatus;

                if (lastStatus==ApproveStatus.NoAction)
                {
                    resultModel.Add(new StepModel { IsCompleted=false, IsDone=false, Step=localizer["Last Approve Status"], Message=lastStatus.GetDescription() });
                }

                if (lastStatus==ApproveStatus.FisrtApproved)
                {
                    resultModel.Add(new StepModel { IsCompleted=false, IsDone=true, Step=localizer["Last Approve Status"], Message=lastStatus.GetDescription() });
                }

                if (lastStatus==ApproveStatus.Disapproved)
                {
                    resultModel.Add(new StepModel { IsCompleted=false, IsDone=false, Step=localizer["Last Approve Status"], Message=lastStatus.GetDescription() });
                }

                if (lastStatus==ApproveStatus.FinalApprove)
                {
                    resultModel.Add(new StepModel { IsCompleted=false, IsDone=true, Step=localizer["Last Approve Status"], Message=lastStatus.GetDescription() });
                }
            }

            return PartialView("_Steps", resultModel);
        }

        #endregion

        #region Save jobApplicant Methods

        [HttpPost]
        public override IActionResult Save([FromServices] ILogic<JobApplicantModel> service, JobApplicantModel model)
        {
            try
            {
                UserInfo userInformation = InitializeUserInformation(model);

                if (model.JobApplicantId == 0)
                {
                    var checkForExistNationalCode = jobApplicantLogic.GetByNationalCode(model.NationalCode);

                    if (checkForExistNationalCode.ResultStatus == OperationResultStatus.Successful && checkForExistNationalCode.ResultEntity is not null)
                    {
                        return Json(new { result = "fail", message = localizer["This National Code Already Exist"] });
                    }
                    HandleNewJobApplicant(model, userInformation);
                }
                else
                {
                    HandleExistingJobApplicant(model);
                }

                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    if (model.JobApplicantId == 0)
                    {
                        var saveJobApplicantResult = service.AddNew(model);

                        if (saveJobApplicantResult.ResultStatus != OperationResultStatus.Successful || saveJobApplicantResult.ResultEntity is null)
                        {
                            scope.Complete();
                            return Json(new { result = "fail", message = localizer[saveJobApplicantResult.AllMessages] });
                        }
                        model.JobApplicantId=saveJobApplicantResult.ResultEntity.JobApplicantId;
                    }
                    else
                    {
                        var saveJobApplicantResult = service.Update(model);

                        if (saveJobApplicantResult.ResultStatus != OperationResultStatus.Successful || !saveJobApplicantResult.ResultEntity)
                        {
                            scope.Complete();
                            return Json(new { result = "fail", message = localizer[saveJobApplicantResult.AllMessages] });
                        }
                    }

                    var fileUploadResult = SaveJobApplicantFiles(model.JobApplicantId);

                    if (fileUploadResult.ResultStatus != OperationResultStatus.Successful || fileUploadResult.ResultEntity.Any(x => !x.IsSuccess))
                    {
                        string concatenatedMessages = GetConcatenatedFileUploadMessages(fileUploadResult);
                        return Json(new { result = "fail", message = concatenatedMessages });
                    }

                    scope.Complete();
                    return Json(new
                    {
                        result = "ok",
                        message = sharedLocalizer["Your data has been saved"],
                        title = sharedLocalizer["SaveTitle"]
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "fail", message = ex.Message });
            }
        }

        private UserInfo InitializeUserInformation(JobApplicantModel model)
        {
            if (model.JobApplicantId == 0)
            {
                model.CreateDate = DateTime.Now;
                model.CreatedBy = userPrincipal.CurrentUserId;
                model.ProcessStatus = ProcessStatus.NoAction;

                var userInfo = new UserInfo
                {
                    Username = model.NationalCode,
                    CreatedOn = DateTime.Now,
                    Name = $"{model.FirstName} {model.LastName}",
                };

                return userInfo;
            }

            return new UserInfo();
        }

        private void HandleNewJobApplicant(JobApplicantModel model, UserInfo userInformation)
        {
            if (userInformation != null)
            {
                HandleNewJobApplicantWithUserInfo(model, userInformation);
            }
            else
            {
                HandleExistingJobApplicant(model);
            }
        }

        private void HandleNewJobApplicantWithUserInfo(JobApplicantModel model, UserInfo userInformation)
        {
            var password = model.NationalCode;
            var existUserInfo = userSharedService.GetUserInfo(model.NationalCode);

            if (!existUserInfo.Any() && existUserInfo.Count == 0)
            {
                var result = userSharedService.CreateUserAsync(userInformation, password).Result;

                if (result.Succeeded)
                {
                    var createdUserInfo = userSharedService.GetUserInfo(model.NationalCode);
                    userInformation = createdUserInfo.FirstOrDefault();
                    var roleResult = userSharedService.AddToRoleAsync(userInformation, "User");
                    model.UserId = userInformation.UserId;
                }
            }
            else
            {
                userInformation = existUserInfo.FirstOrDefault();
                model.UserId = userInformation.UserId;
                var roleResult = userSharedService.AddToRoleAsync(userInformation, "User");
            }
        }

        private void HandleExistingJobApplicant(JobApplicantModel model)
        {
            if (!string.IsNullOrEmpty(model.PersonnelCode))
            {
                var checkForDuplicatePersonnelCode = jobApplicantLogic.GetByPersonnelCode(model.PersonnelCode);

                if (checkForDuplicatePersonnelCode.ResultStatus == OperationResultStatus.Successful && checkForDuplicatePersonnelCode.ResultEntity is not null)
                {
                    Json(new { result = "fail", message = localizer["Duplicate Personnel Code"] });
                }
            }

            var canEditAfterFinalApprove = userPrincipal.CurrentUser.HasClaim("Permission", ":JobApplicantControlPanel:CanEditAfterFinalApprove");

            if (model.BaseInformationApproveStatus == ApproveStatus.FinalApprove && !canEditAfterFinalApprove)
            {
                Json(new { result = "fail", message = localizer["Approved Information Is Not Permitted For Edit"] });
            }
        }


        private BusinessOperationResult<List<FileUploadResultModel>> SaveJobApplicantFiles(int jobApplicantId)
        {
            var files = Request.Form.Files;
            var checkForExistingFiles = jobApplicantFileLogic.GetByJobApplicantId(jobApplicantId);

            try
            {
                return jobApplicantFileLogic.SaveFiles(files, jobApplicantId, checkForExistingFiles.ResultEntity);
            }
            finally
            {
                // Cleanup code, if any, can be added here
            }
        }

        #endregion

        #region Document Files Methods

        public List<FileUploader.Models.DocumentType> CheckJobApplicantFilesApproveStatus(JobApplicantModel jobApplicant, BaseInformationModel baseInformationModel)
        {

            var jobApplicantFiles = jobApplicantFileLogic.GetByJobApplicantId(jobApplicant.JobApplicantId);

            var unApprovedDocuments = jobApplicantFiles.ResultEntity.Where(x => x.ApproveStatus!=ApproveStatus.FisrtApproved).ToList();

            var jobApplicantDocumentTypeIds = unApprovedDocuments.Select(x => x.AttachmentTypeId).ToList();

            var resturnList = new List<FileUploader.Models.DocumentType>();

            foreach (var item in jobApplicantDocumentTypeIds)
            {
                resturnList.Add((FileUploader.Models.DocumentType)Enum.Parse(typeof(FileUploader.Models.DocumentType), item.ToString()));
            }
            return resturnList;
        }


        public List<FileUploader.Models.DocumentType> CheckJobApplicantFiles(JobApplicantModel jobApplicant, BaseInformationModel baseInformationModel)
        {
            var result = new List<FileUploader.Models.DocumentType>();

            List<FileUploader.Models.DocumentType> maleDocuments = [
                FileUploader.Models.DocumentType.OnNationalCard,
                FileUploader.Models.DocumentType.BehindNationalCard,
                FileUploader.Models.DocumentType.BirthCertificateFirstPage,
                FileUploader.Models.DocumentType.MilitaryService,
                FileUploader.Models.DocumentType.PersonalImage,
            ];

            List<FileUploader.Models.DocumentType> femaleDocuments = [
               FileUploader.Models.DocumentType.OnNationalCard,
                FileUploader.Models.DocumentType.BehindNationalCard,
                FileUploader.Models.DocumentType.BirthCertificateFirstPage,
                FileUploader.Models.DocumentType.PersonalImage,
            ];

            var jobApplicantFiles = jobApplicantFileLogic.GetByJobApplicantId(jobApplicant.JobApplicantId);

            if (jobApplicantFiles.ResultStatus != OperationResultStatus.Successful || jobApplicantFiles.ResultEntity is null)
            {
                return result;
            }

            var jobApplicantDocumentTypeIds = jobApplicantFiles.ResultEntity.Select(x => x.AttachmentTypeId).ToList();

            var jobApplicantDocumentTypes = new List<FileUploader.Models.DocumentType>();

            foreach (var item in jobApplicantDocumentTypeIds)
            {
                jobApplicantDocumentTypes.Add((FileUploader.Models.DocumentType)Enum.Parse(typeof(FileUploader.Models.DocumentType), item.ToString()));
            }

            var missingDocuments = new List<FileUploader.Models.DocumentType>();

            if (baseInformationModel != null && baseInformationModel.Gender==GenderType.Male)
                missingDocuments = maleDocuments.Except(jobApplicantDocumentTypes).ToList();
            else
                missingDocuments = femaleDocuments.Except(jobApplicantDocumentTypes).ToList();

            return missingDocuments;
        }

        public List<FileUploader.Models.DocumentType> CheckHRDocuments(JobApplicantModel jobApplicant)
        {
            var result = new List<FileUploader.Models.DocumentType>();

            List<FileUploader.Models.DocumentType> hrDocuments = [
                FileUploader.Models.DocumentType.PromissoryNote,
                FileUploader.Models.DocumentType.NoBadBackground,
                FileUploader.Models.DocumentType.NoAddictionForm,
                FileUploader.Models.DocumentType.InterviewEvaluation,
                FileUploader.Models.DocumentType.EysenckFormFront,
                FileUploader.Models.DocumentType.EysenckFormBehind,
            ];

            var jobApplicantFiles = jobApplicantFileLogic.GetByJobApplicantId(jobApplicant.JobApplicantId);

            if (jobApplicantFiles.ResultStatus != OperationResultStatus.Successful || jobApplicantFiles.ResultEntity is null)
            {
                return result;
            }

            var jobApplicantDocumentTypeIds = jobApplicantFiles.ResultEntity.Select(x => x.AttachmentTypeId).ToList();

            var jobApplicantDocumentTypes = new List<FileUploader.Models.DocumentType>();

            foreach (var item in jobApplicantDocumentTypeIds)
            {
                jobApplicantDocumentTypes.Add((FileUploader.Models.DocumentType)Enum.Parse(typeof(FileUploader.Models.DocumentType), item.ToString()));
            }

            var missingDocuments = new List<FileUploader.Models.DocumentType>();


            missingDocuments = hrDocuments.Except(jobApplicantDocumentTypes).ToList();


            return missingDocuments;
        }


        public List<FileUploader.Models.DocumentType> CheckDocumentsForInvitioan(JobApplicantModel jobApplicant, BaseInformationModel baseInformationModel)
        {


            var result = new List<FileUploader.Models.DocumentType>();

            List<FileUploader.Models.DocumentType> requiredDocuments = [
                FileUploader.Models.DocumentType.OnNationalCard,
                FileUploader.Models.DocumentType.BehindNationalCard,
                FileUploader.Models.DocumentType.BirthCertificateFirstPage,
                FileUploader.Models.DocumentType.PersonalImage,
                FileUploader.Models.DocumentType.FileSummary,
                FileUploader.Models.DocumentType.InterviewEvaluation,
                FileUploader.Models.DocumentType.EysenckFormFront,
                FileUploader.Models.DocumentType.EysenckFormBehind,
                FileUploader.Models.DocumentType.OnEmploymentQuestionnaire,
                FileUploader.Models.DocumentType.BehindEmploymentQuestionnaire,
            ];

            if (baseInformationModel.MarriageStatus==MaritalStatus.Married)
            {

                requiredDocuments.Add(FileUploader.Models.DocumentType.PartnerBirthCertFirstPage);
                requiredDocuments.Add(FileUploader.Models.DocumentType.PartnerBirthCertSecondPage);
                requiredDocuments.Add(FileUploader.Models.DocumentType.PartnerMelliCard);
            }

            if (baseInformationModel.Gender==GenderType.Male)
            {

                requiredDocuments.Add(FileUploader.Models.DocumentType.MilitaryService);
            }

            if (baseInformationModel.ChildCount>0)
            {

                requiredDocuments.Add(FileUploader.Models.DocumentType.FirstChildBirthCert);
            }

            if (baseInformationModel.ChildCount>1)
            {
                requiredDocuments.Add(FileUploader.Models.DocumentType.SecondChildBirthCert);
            }

            if (baseInformationModel.ChildCount>2)
            {
                requiredDocuments.Add(FileUploader.Models.DocumentType.ThirdChildBirthCert);
            }

            if (baseInformationModel.ChildCount>3)
            {
                requiredDocuments.Add(FileUploader.Models.DocumentType.ForthChildBirthCert);
            }
            if (baseInformationModel.ChildCount>4)
            {
                requiredDocuments.Add(FileUploader.Models.DocumentType.FifthChildBirthCert);
            }
            if (baseInformationModel.ChildCount>5)
            {
                requiredDocuments.Add(FileUploader.Models.DocumentType.SixthChildBirthCert);
            }

            var jobApplicantFiles = jobApplicantFileLogic.GetByJobApplicantId(jobApplicant.JobApplicantId);

            if (jobApplicantFiles.ResultStatus != OperationResultStatus.Successful || jobApplicantFiles.ResultEntity is null)
            {
                return result;
            }

            var jobApplicantDocumentTypeIds = jobApplicantFiles.ResultEntity.Select(x => x.AttachmentTypeId).ToList();

            var jobApplicantDocumentTypes = new List<FileUploader.Models.DocumentType>();

            foreach (var item in jobApplicantDocumentTypeIds)
            {
                jobApplicantDocumentTypes.Add((FileUploader.Models.DocumentType)Enum.Parse(typeof(FileUploader.Models.DocumentType), item.ToString()));
            }

            var missingDocuments = new List<FileUploader.Models.DocumentType>();

            missingDocuments = requiredDocuments.Except(jobApplicantDocumentTypes).ToList();

            return missingDocuments;
        }

        public IActionResult DownloadFile(string link)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", "fa-IR", "Word", link);

            var fileContent = System.IO.File.ReadAllBytes(filePath);

            return File(fileContent, "application/octet-stream", link);
        }

        private static string GetConcatenatedFileUploadMessages(BusinessOperationResult<List<FileUploadResultModel>> fileUploadResult)
        {

            if (fileUploadResult.ResultEntity!=null)
            {
                List<string> messages = fileUploadResult.ResultEntity
                    .Where(x => x.Message != null && x.Message.Length > 1)
                    .Select(x => x.Message)
                    .ToList();

                return string.Join("\n", messages);
            }

            else return fileUploadResult.AllMessages;


        }

        #endregion

        #region Approves And Invitations

        public IActionResult ApproveOperation(int id)
        {
            var jobApplicantResult = jobApplicantLogic.GetById(id);

            var resultModel = new JobApplicantApproveResultModel();

            if (jobApplicantResult.ResultStatus != OperationResultStatus.Successful || jobApplicantResult.ResultEntity is null)
            {
                resultModel.OperationResultStatus=OperationResultStatus.Fail;
                resultModel.Message=localizer["Base Information Not Found"];
                return PartialView("_ApproveJobApplicantResult", resultModel);
            }

            var baseInformationResult = baseInformationLogic.GetByJobApplicantId(id);

            if (baseInformationResult.ResultStatus != OperationResultStatus.Successful || baseInformationResult.ResultEntity is null)
            {
                resultModel.OperationResultStatus=OperationResultStatus.Fail;
                resultModel.Message=localizer["Base Information Not Found"];
                return PartialView("_ApproveJobApplicantResult", resultModel);
            }

            var currentState = jobApplicantResult.ResultEntity.BaseInformationApproveStatus;
            switch (currentState)
            {

                case ApproveStatus.Disapproved:
                    {
                        resultModel = FirstApprove(jobApplicantResult.ResultEntity, baseInformationResult.ResultEntity);
                        resultModel.Info=localizer["First Approve Step"];
                        break;
                    }

                case ApproveStatus.NoAction:
                    {

                        resultModel = FirstApprove(jobApplicantResult.ResultEntity, baseInformationResult.ResultEntity);
                        resultModel.Info=localizer["First Approve Step"];
                        break;
                    }

                case ApproveStatus.FisrtApproved:
                    {
                        resultModel = DocumentsApprove(jobApplicantResult.ResultEntity, baseInformationResult.ResultEntity);
                        resultModel.Info=localizer["Documents Approve Step"];
                        break;
                    }

                case ApproveStatus.DocumentsApprove:
                    {
                        resultModel.OperationResultStatus=OperationResultStatus.Fail;
                        resultModel.Message=localizer["You Must Save Invitation Befor Final Approve"];
                        return PartialView("_ApproveJobApplicantResult", resultModel);
                    }

                case ApproveStatus.InvitedToWork:
                    {
                        resultModel = FinalApprove(jobApplicantResult.ResultEntity, baseInformationResult.ResultEntity);
                        resultModel.Info=localizer["Final Approve Step"];
                        break;
                    }

                case ApproveStatus.FinalApprove:
                    {
                        resultModel.Message=localizer["Approve Is not Permitted"];
                        var errorMessgae = $"{localizer["Information Has Already Approved"]}";
                        resultModel.Errors.Add(errorMessgae);
                        break;
                    }               
            }

            return PartialView("_ApproveJobApplicantResult", resultModel);

        }


        public IActionResult EmployeeApproveOperation(int id)
        {
            var jobApplicantResult = jobApplicantLogic.GetById(id);

            var resultModel = new JobApplicantApproveResultModel();

            if (jobApplicantResult.ResultStatus != OperationResultStatus.Successful || jobApplicantResult.ResultEntity is null)
            {
                resultModel.OperationResultStatus=OperationResultStatus.Fail;
                resultModel.Message=localizer["Base Information Not Found"];
                return PartialView("_ApproveJobApplicantResult", resultModel);
            }

            var baseInformationResult = baseInformationLogic.GetByJobApplicantId(id);

            if (baseInformationResult.ResultStatus != OperationResultStatus.Successful || baseInformationResult.ResultEntity is null)
            {
                resultModel.OperationResultStatus=OperationResultStatus.Fail;
                resultModel.Message=localizer["Base Information Not Found"];
                return PartialView("_ApproveJobApplicantResult", resultModel);
            }

            resultModel = EmployeeApprove(jobApplicantResult.ResultEntity, baseInformationResult.ResultEntity);
            resultModel.Info=localizer["Final Approve Step"];

            return PartialView("_ApproveJobApplicantResult", resultModel);

        }

        public JobApplicantApproveResultModel FirstApprove(JobApplicantModel jobApplicantModel, BaseInformationModel baseInformationModel)
        {
            var resultModel = new JobApplicantApproveResultModel();

            if (jobApplicantModel.BaseInformationApproveStatus>=ApproveStatus.FisrtApproved) {

                resultModel.Message=localizer["Information Has Already Approved"];
                resultModel.OperationResultStatus = OperationResultStatus.Fail;
                return resultModel;
            }         
            var checkJobApplicantFilesResult = CheckJobApplicantFiles(jobApplicantModel, baseInformationModel);
            var checkUnApprovedDocuments = CheckJobApplicantFilesApproveStatus(jobApplicantModel, baseInformationModel);

            if (checkJobApplicantFilesResult.Count!=0 || checkUnApprovedDocuments.Count!=0)
            {
                resultModel.Message=localizer["Approve Is not Permitted"];
                resultModel.OperationResultStatus = OperationResultStatus.Fail;

                foreach (var item in checkJobApplicantFilesResult)
                {
                    var errorMessgae = $"{item.GetDescription()} {localizer["not uploaded"]}";
                    resultModel.Errors.Add(errorMessgae);
                }

                foreach (var item in checkUnApprovedDocuments)
                {
                    var errorMessgae = $"{item.GetDescription()} {localizer["not approved"]}";
                    resultModel.Errors.Add(errorMessgae);
                }
            }
            var backgroundJobErrors = CheckBackgroundJobApproval(jobApplicantModel);

            if (backgroundJobErrors.Count!=0)
            {

                foreach (var error in backgroundJobErrors)
                {
                    resultModel.Message=localizer["Approve Is not Permitted"];
                    resultModel.Errors.Add(error);
                }
            }          
            if (resultModel.Errors.Count==0)
            {
                var result = jobApplicantLogic.FirstApprove(jobApplicantModel);

                if (result.ResultStatus != OperationResultStatus.Successful || !result.ResultEntity)
                {
                    return new JobApplicantApproveResultModel
                    {
                        OperationResultStatus=OperationResultStatus.Fail,
                        Message=localizer[result.AllMessages]
                    };
                }
                resultModel.OperationResultStatus=OperationResultStatus.Successful;
                resultModel.Message=localizer["Base Information First Approved Successfully"];
            }
            return resultModel;
        }


        public JobApplicantApproveResultModel DocumentsApprove(JobApplicantModel jobApplicantModel, BaseInformationModel baseInformationModel)
        {
            var resultModel = new JobApplicantApproveResultModel();
            var checkRequiredDocuments = CheckDocumentsForInvitioan(jobApplicantModel, baseInformationModel);
            if (checkRequiredDocuments.Count!=0)
            {
                resultModel.Message=localizer["Approve Is not Permitted"];
                resultModel.OperationResultStatus = OperationResultStatus.Fail;

                foreach (var item in checkRequiredDocuments)
                {
                    var errorMessgae = $"{item.GetDescription()} {localizer["not uploaded"]}";
                    resultModel.Errors.Add(errorMessgae);
                }
            }

            if (resultModel.Errors.Count==0)
            {
                var result = jobApplicantLogic.ApproveDocuments(jobApplicantModel);

                if (result.ResultStatus != OperationResultStatus.Successful || !result.ResultEntity)
                {
                    return new JobApplicantApproveResultModel
                    {
                        OperationResultStatus=OperationResultStatus.Fail,
                        Message=localizer[result.AllMessages]
                    };
                }
                resultModel.OperationResultStatus=OperationResultStatus.Successful;
                resultModel.Message=localizer["Documents Approved Successfully"];
            }
            return resultModel;
        }

        public IActionResult ShowInvitation(int id)
        {
            var relatedJobApplicant = jobApplicantLogic.GetById(id);

            var model = new InvitationModel
            {
                InvitationToWorkDate=relatedJobApplicant.ResultEntity.InvitationToWorkDate,
                JobApplicantId=relatedJobApplicant.ResultEntity.JobApplicantId,
                PersianInvitationToWorkDate=relatedJobApplicant.ResultEntity.PersianInvitationToWorkDate,
            };

            return PartialView("_Invite", model);
        }


        public JobApplicantApproveResultModel FinalApprove(JobApplicantModel jobApplicantModel, BaseInformationModel baseInformationModel)
        {
            var resultModel = new JobApplicantApproveResultModel();
            var checkJobApplicantFilesResult = CheckJobApplicantFiles(jobApplicantModel, baseInformationModel);
            var checkUnApprovedDocuments = CheckJobApplicantFilesApproveStatus(jobApplicantModel, baseInformationModel);
            var checkHRDocuments = CheckHRDocuments(jobApplicantModel);

            if (checkJobApplicantFilesResult.Count!=0 || checkUnApprovedDocuments.Count!=0 || checkHRDocuments.Count!=0)
            {
                resultModel.Message=localizer["Approve Is not Permitted"];
                resultModel.OperationResultStatus = OperationResultStatus.Fail;

                foreach (var item in checkJobApplicantFilesResult)
                {
                    var errorMessgae = $"{item.GetDescription()} {localizer["not uploaded"]}";
                    resultModel.Errors.Add(errorMessgae);
                }

                foreach (var item in checkUnApprovedDocuments)
                {
                    var errorMessgae = $"{item.GetDescription()} {localizer["not approved"]}";
                    resultModel.Errors.Add(errorMessgae);
                }

                foreach (var item in checkHRDocuments)
                {
                    var errorMessgae = $"{item.GetDescription()} {localizer["not uploaded"]}";
                    resultModel.Errors.Add(errorMessgae);
                }
            }

            if (jobApplicantModel.OccupationalMedicineApproveStatus==OccupationalMedicineApproveStatus.DisApprove)
            {
                resultModel.Message=localizer["Approve Is not Permitted"];
                var errorMessgae = $"{localizer["Occupational Medicine Result Not Approved"]}";
                resultModel.Errors.Add(errorMessgae);
            }

            if (jobApplicantModel.OccupationalMedicineApproveStatus==OccupationalMedicineApproveStatus.NoAction)
            {
                resultModel.Message=localizer["Approve Is not Permitted"];
                var errorMessgae = $"{localizer["Occupational Medicine Result Not Filled"]}";
                resultModel.Errors.Add(errorMessgae);
            }

            if (jobApplicantModel.OccupationalMedicineApproveStatus==OccupationalMedicineApproveStatus.Referral)
            {
                resultModel.Message=localizer["Approve Is not Permitted"];
                var errorMessgae = $"{localizer["Occupational Medicine Has Referral"]}";
                resultModel.Errors.Add(errorMessgae);
            }

            if (!jobApplicantModel.NoAddictionDone)
            {
                resultModel.Message=localizer["Approve Is not Permitted"];
                var errorMessgae = $"{localizer["NoAddictionDone Not announced"]}";
                resultModel.Errors.Add(errorMessgae);
            }

            if (!jobApplicantModel.NoBadBackgroundDone)
            {
                resultModel.Message=localizer["Approve Is not Permitted"];
                var errorMessgae = $"{localizer["NoBadBackgroundDone Not announced"]}";
                resultModel.Errors.Add(errorMessgae);
            }

            if (!jobApplicantModel.OccupationalMedicineDone)
            {
                resultModel.Message=localizer["Approve Is not Permitted"];
                var errorMessgae = $"{localizer["OccupationalMedicineDone Not announced"]}";
                resultModel.Errors.Add(errorMessgae);
            }

            if (string.IsNullOrEmpty(jobApplicantModel.PersonnelCode))
            {
                resultModel.OperationResultStatus=OperationResultStatus.Fail;
                resultModel.Message=localizer["Error In Final Approve"];
                resultModel.Errors.Add(localizer["You Must Assign Personnel Code Befor Final Approve"]);
            }

            if (resultModel.Errors.Count==0)
            {
                var result = jobApplicantLogic.FinalApprove(jobApplicantModel);
                if (result.ResultStatus != OperationResultStatus.Successful || !result.ResultEntity)
                {
                    return new JobApplicantApproveResultModel
                    {
                        OperationResultStatus=OperationResultStatus.Fail,
                        Message=localizer[result.AllMessages]
                    };
                }
                else
                {
                    resultModel.OperationResultStatus=OperationResultStatus.Successful;
                    resultModel.Message=localizer["Base Information Approved Successfully"];
                }
            }
            return resultModel;
        }



        public JobApplicantApproveResultModel EmployeeApprove(JobApplicantModel jobApplicantModel, BaseInformationModel baseInformationModel)
        {
            var resultModel = new JobApplicantApproveResultModel();
            var checkJobApplicantFilesResult = CheckJobApplicantFiles(jobApplicantModel, baseInformationModel);
            var checkUnApprovedDocuments = CheckJobApplicantFilesApproveStatus(jobApplicantModel, baseInformationModel);

            if (checkJobApplicantFilesResult.Count!=0 || checkUnApprovedDocuments.Count!=0)
            {
                resultModel.Message=localizer["Approve Is not Permitted"];
                resultModel.OperationResultStatus = OperationResultStatus.Fail;

                foreach (var item in checkJobApplicantFilesResult)
                {
                    var errorMessgae = $"{item.GetDescription()} {localizer["not uploaded"]}";
                    resultModel.Errors.Add(errorMessgae);
                }

                foreach (var item in checkUnApprovedDocuments)
                {
                    var errorMessgae = $"{item.GetDescription()} {localizer["not approved"]}";
                    resultModel.Errors.Add(errorMessgae);
                }
            }

            if (string.IsNullOrEmpty(jobApplicantModel.PersonnelCode))
            {
                resultModel.OperationResultStatus=OperationResultStatus.Fail;
                resultModel.Message=localizer["Error In Final Approve"];
                resultModel.Errors.Add(localizer["You Must Assign Personnel Code Befor Final Approve"]);
            }

            if (resultModel.Errors.Count==0)
            {
                var result = jobApplicantLogic.FinalApprove(jobApplicantModel);
                if (result.ResultStatus != OperationResultStatus.Successful || !result.ResultEntity)
                {
                    return new JobApplicantApproveResultModel
                    {
                        OperationResultStatus=OperationResultStatus.Fail,
                        Message=localizer[result.AllMessages]
                    };
                }
                else
                {
                    resultModel.OperationResultStatus=OperationResultStatus.Successful;
                    resultModel.Message=localizer["Base Information Approved Successfully"];
                }
            }
            return resultModel;
        }

        private List<string> CheckBackgroundJobApproval(JobApplicantModel jobApplicant)
        {

            var resultModel = new List<string>();

            var jobCategory = jobApplicant.JobCategory;

            if (jobCategory==JobCategory.Worker)
            {

                var relatedJobBackground = workerJobBackgroundLogic.GetByJobApplicantId(jobApplicant.JobApplicantId);

                if (relatedJobBackground.ResultStatus != OperationResultStatus.Successful || relatedJobBackground.ResultEntity is null)
                {
                    var errorMessgae = $"{localizer["backGroundJob Form Is Not Filled"]}";
                    resultModel.Add(errorMessgae);
                }

                if (relatedJobBackground?.ResultEntity!=null && relatedJobBackground.ResultEntity.ApproveStatus!=BackgroundJobApproveStatus.Approved)
                {
                    var errorMessgae = $"{localizer["backGroundJob Form Is Not Approved"]}";
                    resultModel.Add(errorMessgae);
                }
            }

            if (jobCategory==JobCategory.Employee)
            {
                var relatedJobBackground = employeeJobBackgroundLogic.GetByJobApplicantId(jobApplicant.JobApplicantId);

                if (relatedJobBackground.ResultStatus != OperationResultStatus.Successful || relatedJobBackground.ResultEntity is null)
                {
                    var errorMessgae = $"{localizer["backGroundJob Form Is Not Filled"]}";
                    resultModel.Add(errorMessgae);
                }

                if (relatedJobBackground?.ResultEntity!=null && relatedJobBackground.ResultEntity.ApproveStatus!=BackgroundJobApproveStatus.Approved)
                {
                    var errorMessgae = $"{localizer["backGroundJob Form Is Not Approved"]}";
                    resultModel.Add(errorMessgae);
                }
            }
            return resultModel;
        }


        public IActionResult SaveInvitationDate(int jobApplicantId, DateTime invitationDate)
        {

            var relatedJobApplicant = jobApplicantLogic.GetById(jobApplicantId);
            if (relatedJobApplicant.ResultStatus != OperationResultStatus.Successful || relatedJobApplicant.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[relatedJobApplicant.AllMessages] });
            }
            if (relatedJobApplicant.ResultEntity.BaseInformationApproveStatus==ApproveStatus.InvitedToWork)
            {

                return Json(new { result = "fail", message = localizer["Personnel Alreday Invited"] });
            }
            relatedJobApplicant.ResultEntity.InvitationToWorkDate = invitationDate;
            relatedJobApplicant.ResultEntity.InvitedBy=userPrincipal.CurrentUserId;
            relatedJobApplicant.ResultEntity.BaseInformationApproveStatus=ApproveStatus.InvitedToWork;
            var updateResult = jobApplicantLogic.Update(relatedJobApplicant.ResultEntity);

            if (updateResult.ResultStatus != OperationResultStatus.Successful || !updateResult.ResultEntity)
            {
                return Json(new { result = "fail", message = localizer[updateResult.AllMessages] });
            }

            sendAsiaSMSService.SendMessage(new SendSmsModel
            {
                Receivers = relatedJobApplicant.ResultEntity.MobileNumber,
                SmsText = $"همکار گرامی : {relatedJobApplicant.ResultEntity.FirstName} {relatedJobApplicant.ResultEntity.LastName} \n ورود شما را به خانواده ترام تبریک می گوییم ، جهت هماهنگی سرویس با شماره 09921566748 آقای خانبازی هماهنگ شوید ، لطفا مدارک مورد نیاز را همراه داشته باشید"
            });

            return Json(new { result = "ok", message = localizer["Invitation Saved Successfully"] });


        }


        #endregion

        #region DisApprove Methods

        public IActionResult ShowDisApproveModal(int id)
        {

            return PartialView("_DisApproveModal", id);

        }


        public IActionResult DisApproveInfo(int id, string reason)
        {
            var relatedJobApplicant = jobApplicantLogic.GetById(id);
            if (relatedJobApplicant.ResultStatus != OperationResultStatus.Successful || relatedJobApplicant.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[relatedJobApplicant.AllMessages] });
            }
            relatedJobApplicant.ResultEntity.BaseInformationApproveStatus=ApproveStatus.Disapproved;
            relatedJobApplicant.ResultEntity.BaseInformationApproveDate=DateTime.Now;
            relatedJobApplicant.ResultEntity.BaseInformationApprovedBy=userPrincipal.CurrentUserId;
            relatedJobApplicant.ResultEntity.BaseInformationErrors=reason;

            var updateResult = jobApplicantLogic.Update(relatedJobApplicant.ResultEntity);

            if (updateResult.ResultStatus != OperationResultStatus.Successful || !updateResult.ResultEntity)
            {
                return Json(new { result = "fail", message = localizer["Error In Update Status"] });
            }

            sendAsiaSMSService.SendMessage(new SendSmsModel
            {
                Receivers=relatedJobApplicant.ResultEntity.MobileNumber,
                SmsText=$"اطلاعات شما در سامانه منابع انسانی به دلیل زیر تایید نشد\n{reason}\nشرکت ترام چاپ"
            });

            return Json(new { result = "ok", message = localizer["DisApprovedSuccessfully"] });

        }


        #endregion
    }
}
