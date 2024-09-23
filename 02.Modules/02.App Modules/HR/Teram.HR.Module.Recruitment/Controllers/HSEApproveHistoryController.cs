using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Configuration;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Logic;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.Module.AttachmentsManagement.Models;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Model;

namespace Teram.HR.Module.Recruitment.Controllers
{
    public class HSEApproveHistoryController : ControlPanelBaseController<HSEApproveHistoryModel, HSEApproveHistory, int>
    {
        private readonly IUserSharedService userSharedService;
        private readonly IJobApplicantLogic jobApplicantLogic;
        private readonly IConfiguration configuration;

        public HSEApproveHistoryController(ILogger<HSEApproveHistoryController> logger
            , IStringLocalizer<HSEApproveHistoryController> localizer, IUserSharedService userSharedService, IJobApplicantLogic jobApplicantLogic,
            IStringLocalizer<SharedResource> sharedLocalizer, IConfiguration configuration)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<HSEApproveHistoryModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["HSEApproveHistory"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["HSEApproveHistory"],
                OperationColumns = true,
                HomePage = nameof(HSEApproveHistoryController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/HR/Module/Recruitment/Scripts/HSEApproveHistory.js",
            };
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
            this.jobApplicantLogic=jobApplicantLogic;
            this.configuration=configuration??throw new ArgumentNullException(nameof(configuration));
        }
        public IActionResult Index(int id)
        {
            Model.GetDataUrl = "GetData?id=" + id;
            return View(Model);
        }

        public override IActionResult GetData([FromServices] ILogic<HSEApproveHistoryModel> service, DatatablesSentModel model)
        {
            var logic = (HSEApproveHistoryLogic)service;
            var jobApplicantId = Convert.ToInt32(Request.Query["id"]);
            var result = logic.GetbyJobApplicantId(jobApplicantId, model.Start, model.Length);
            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity is null)
            {
                return Json(new { model.Draw, recordsTotal = 0, recordsFiltered = 0, error = result.AllMessages });
            }

            var jobApplicantData = jobApplicantLogic.GetById(jobApplicantId);

            if (jobApplicantData.ResultStatus != OperationResultStatus.Successful || jobApplicantData.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[jobApplicantData.AllMessages] });
            }

            var data = result.ResultEntity;
            var ApproverUserIds = data.Select(x => x.ApprovedByUserId).ToList();

            var usersInfo = userSharedService.GetUserInfos(ApproverUserIds);

            foreach (var item in data)
            {

                item.Firstname=jobApplicantData.ResultEntity.FirstName;
                item.Lastname=jobApplicantData.ResultEntity.LastName;

                var downloadurl = configuration.GetSection("Attachment").Get<AttachmentSection>().DownloadUrl;

                if (item.ReferralAtachmentId!=null)
                    item.Referralfilelink=$"<a target='_blank' href={downloadurl}{item.ReferralAtachmentId}>فایل ارجاعیه</a>";

                if (item.FileSummaryAttchmanetId!=null)
                    item.FileSummaryLink=$"<a target='_blank' href={downloadurl}{item.FileSummaryAttchmanetId}>فایل خلاصه پرونده</a>";

                var approverUser = usersInfo.FirstOrDefault(x => x.UserId==item.ApprovedByUserId);
                if (approverUser != null)
                {
                    item.ApprovedByName = approverUser.Username;
                }
            }

            return Json(new { model.Draw, recordsTotal = data.Count, recordsFiltered = result.Count, data, error = "" });
        }
    }

}
