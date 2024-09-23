using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Data.SqlClient;
using System.Globalization;
using System.Transactions;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.FileUploader.Models;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Migrations;
using Teram.HR.Module.Recruitment.Models;
using Teram.HR.Module.Recruitment.Models.WorkWithUs;
using Teram.Module.Authentication.Models;
using Teram.Module.GeographicRegion.Logic;
using Teram.Module.GeographicRegion.Logic.Interfaces;
using Teram.Module.SmsSender.Services;
using Teram.ServiceContracts;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Security;

namespace Teram.HR.Module.Recruitment.Controllers
{
    public class WorkWithUsController : BasicControlPanelController
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IBaseInformationLogic baseInformationLogic;
        private readonly IUserPrincipal userPrincipal;
        private readonly IGeographicRegionLogic geographicRegionLogic;
        private readonly IJobApplicantFileLogic jobApplicantFileLogic;
        private readonly ISendAsiaSMSService sendAsiaSMSService;
        private readonly ILogger<WorkWithUsController> logger;
        private readonly IStringLocalizer<WorkWithUsController> localizer;
        private readonly IJobApplicantLogic jobApplicantLogic;

        public WorkWithUsController(IServiceProvider serviceProvider,
            ILogger<WorkWithUsController> logger, IStringLocalizer<WorkWithUsController> localizer, IJobApplicantLogic jobApplicantLogic,
            IBaseInformationLogic baseInformationLogic, IUserPrincipal userPrincipal,
            IGeographicRegionLogic geographicRegionLogic, IJobApplicantFileLogic jobApplicantFileLogic, ISendAsiaSMSService sendAsiaSMSService)
        {
            this.serviceProvider = serviceProvider;
            this.baseInformationLogic = baseInformationLogic;
            this.userPrincipal=userPrincipal??throw new ArgumentNullException(nameof(userPrincipal));
            this.geographicRegionLogic = geographicRegionLogic ?? throw new ArgumentNullException(nameof(geographicRegionLogic));
            this.jobApplicantFileLogic=jobApplicantFileLogic??throw new ArgumentNullException(nameof(jobApplicantFileLogic));
            this.sendAsiaSMSService=sendAsiaSMSService??throw new ArgumentNullException(nameof(sendAsiaSMSService));
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            this.jobApplicantLogic=jobApplicantLogic??throw new ArgumentNullException(nameof(jobApplicantLogic));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [ControlPanelMenu("Base Information", ParentName = "ManageJobApplicants", Icon = "fa fa-database", PanelType = PanelType.User, Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar, Order = 1)]
        public IActionResult Index()
        {
            return View("Add");
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult SuccessInfo()
        {
            return View("SuccessInfo");
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult EmployeeSuccessInfo()
        {
            return View("EmployeeSuccessInfo");
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult SaveInfo([FromServices] ILogic<BaseInformationModel> service, BaseInformationModel model)
        {
            BusinessOperationResult<BaseInformationModel>? result = null;
            try
            {
                using var transaction = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);

                var files = Request.Form.Files;

                var resource = baseInformationLogic.GetBaseInformationByNationalCode(model.NationalCode);

                if (resource.ResultEntity != null && resource.ResultStatus == OperationResultStatus.Successful)
                    return Json(new { result = "fail", message = localizer["Your information is already registered in the system."] });

                if (!model.IsAgreed)
                {
                    return Json(new { result = "fail", message = localizer["The agreement section has not been approved"] });
                }

                model.CreatedOn = DateTime.Now;
                result = service.AddNew(model);
                if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity == null)
                {
                    return Json(new { result = "fail", message = localizer["Add Error"] });
                }

                var relatedJobApplicant = jobApplicantLogic.GetByNationalCode(model.NationalCode);
                if (relatedJobApplicant.ResultStatus == OperationResultStatus.Successful &&  relatedJobApplicant.ResultEntity is not null)
                {
                    relatedJobApplicant.ResultEntity.MobileNumber=model.Mobile;
                    relatedJobApplicant.ResultEntity.Address=model.Address;
                    jobApplicantLogic.Update(relatedJobApplicant.ResultEntity);
                }

                var existingFiles = jobApplicantFileLogic.GetByJobApplicantId(result.ResultEntity.JobApplicantId);


                if (existingFiles.ResultEntity.FirstOrDefault(x => x.AttachmentTypeId==(int)DocumentType.Resume)==null)
                {
                    var fileUploadResult = jobApplicantFileLogic.SaveFiles(files, result.ResultEntity.JobApplicantId, existingFiles.ResultEntity);

                    if (fileUploadResult.ResultStatus != OperationResultStatus.Successful || fileUploadResult.ResultEntity.Select(x => x.IsSuccess).ToList().Contains(false))
                    {
                        return Json(new { result = "fail", message = localizer["Error in Upload Some Files"] });
                    }
                }

                transaction.Complete();

                if (model.FlowType==Enums.FlowType.JobApplicant)
                {
                    sendAsiaSMSService.SendMessage(new Teram.Module.SmsSender.Models.AsiaSms.SendSmsModel
                    {
                        Receivers=model.Mobile,
                        SmsText=$"{model.Name} {model.Lastname} \n اطلاعات پایه شما در سامانه منابع انسانی با موفقیت ثبت شد \n ادامه فرآیند : B2n.ir/j18826"
                    });
                }

                return Json(new { result = "ok", message = localizer["Your data has been saved"] });
            }
            catch (Exception ex)
            {
                return Json(new { result = "fail", message = localizer["Add Error"] });
            }
        }    
        public IActionResult ViewInfoToApprove([FromServices] ILogic<BaseInformationModel> service, BaseInformationModel model)
        {

            if (model.Latitude is null || model.Longitude is null)
            {

                return Json(new { result = "fail", message = localizer["Please Define Your Location on Map"] });

            }

            var files = Request.Form.Files;

            var birthLocationInfo = geographicRegionLogic.GetById(model.BirthLocationId.Value);

            if (birthLocationInfo.ResultStatus != OperationResultStatus.Successful || birthLocationInfo.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[birthLocationInfo.AllMessages] });
            }

            model.BirthLocationName = birthLocationInfo.ResultEntity.Name;
            ViewBag.CurrentPersianDate = System.DateTime.Now.ToPersianDate();
            return PartialView("PreviewContainer", model);
        }

        public IActionResult GetGeoByParent([FromServices] IGeographicRegionLogic logic, int? parentId, string search)
        {
            var result = logic.GetByParentId(parentId, search);
            if (result.ResultStatus != OperationResultStatus.Successful)
            {
                return Json(new { result = "fail", message = result.AllMessages });
            }
            var data = result.ResultEntity.Select(x => new { id = x.GeographicRegionId, text = x.Name }).ToList();
            return Json(new { results = data });
        }
        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetTrainingRecord(int index)
        {
            ViewBag.index = index;
            return PartialView("_TrainingRecord");
        }
        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetResume(int index)
        {
            ViewBag.index = index;
            return PartialView("_Resume");
        }

        [ParentalAuthorize(nameof(Index))]

        public IActionResult GetLanguages(int index)
        {

            ViewBag.Index = index;
            return PartialView("_PersonnelLanguages");
        }


        [ParentalAuthorize(nameof(Index))]

        public IActionResult GetComputerSkills(int index)
        {
            ViewBag.Index = index;
            return PartialView("_PersonnelComputerSkill");
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetEmergencyContact(int index)
        {
            ViewBag.index = index;
            return PartialView("_EmergencyContact");

        }
        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetEducation(int index)
        {
            ViewBag.index = index;
            return PartialView("_Education");
        }
    }
}
