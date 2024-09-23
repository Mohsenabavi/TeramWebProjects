using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Logic;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Model;

namespace Teram.HR.Module.Recruitment.Controllers
{  
    public class JobApplicantApproveHistoryController : ControlPanelBaseController<JobApplicantApproveHistoryModel, JobApplicantApproveHistory, int>
    {
        private readonly IUserSharedService userSharedService;

        public JobApplicantApproveHistoryController(ILogger<JobApplicantApproveHistoryController> logger
            , IStringLocalizer<JobApplicantApproveHistoryController> localizer, IUserSharedService userSharedService,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;
            Model = new ViewInformation<JobApplicantApproveHistoryModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["JobApplicantApproveHistory"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["JobApplicantApproveHistory"],
                OperationColumns = true,
                ModelData = new JobApplicantApproveHistoryModel(),
                ModelType = typeof(JobApplicantApproveHistoryModel),
                GetDataUrl = nameof(GetGridData),
                HomePage = nameof(JobApplicantApproveHistoryController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/HR/Module/Recruitment/Scripts/JobApplicantApproveHistory.js",
            };
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
        }       
        public IActionResult Index(int id)
        {
            Model.GetDataUrl = "GetData?id=" + id;
            return View(Model);
        }

        public override IActionResult GetData([FromServices] ILogic<JobApplicantApproveHistoryModel> service, DatatablesSentModel model)
        {
            var logic = (JobApplicantApproveHistoryLogic)service;
            var jobApplicantId = Convert.ToInt32(Request.Query["id"]);
            var result = logic.GetByJobApplicantId(jobApplicantId, model.Start, model.Length);
            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity is null)
            {
                return Json(new { model.Draw, recordsTotal = 0, recordsFiltered = 0, error = result.AllMessages });
            }
            var data = result.ResultEntity;     
            
            var ApproverUserIds= data.Select(x=>x.ApprovedByUserId).ToList();

            var usersInfo= userSharedService.GetUserInfos(ApproverUserIds);
            
            foreach (var item in data)
            {
                var approverUser = usersInfo.FirstOrDefault(x => x.UserId==item.ApprovedByUserId);
                if (approverUser != null) {
                    item.ApprovedByName = approverUser.Username;
                }
            }

            return Json(new { model.Draw, recordsTotal = data.Count, recordsFiltered = result.Count, data, error = "" });
        }
    }

}
