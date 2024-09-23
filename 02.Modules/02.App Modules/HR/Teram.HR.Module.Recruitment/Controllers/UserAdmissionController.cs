using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Teram.Web.Core.Model;
using Teram.Web.Core.Security;

namespace Teram.HR.Module.Recruitment.Controllers
{
    public class UserAdmissionController : ControlPanelBaseController<UserAdmissionModel, JobApplicant, int>
    {
        private readonly IJobApplicantsIntroductionLetterLogic jobApplicantsIntroductionLetterLogic;
        private readonly IUserSharedService userSharedService;
        private readonly IUserPrincipal userPrincipal;
        private readonly IJobApplicantLogic jobApplicantLogic;

        public UserAdmissionController(ILogger<UserAdmissionController> logger, IJobApplicantsIntroductionLetterLogic jobApplicantsIntroductionLetterLogic
            , IStringLocalizer<UserAdmissionController> localizer, IUserSharedService userSharedService,
            IUserPrincipal userPrincipal, IJobApplicantLogic jobApplicantLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<UserAdmissionModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["UserAdmission"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["UserAdmission"],
                OperationColumns = true,
                HomePage = nameof(UserAdmissionController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/HR/Module/Recruitment/Scripts/UserAdmission.js",
                GetDataUrl = "",
                LoadAjaxData = false,
                ModelData=new UserAdmissionModel(),
            };
            this.jobApplicantsIntroductionLetterLogic=jobApplicantsIntroductionLetterLogic??throw new ArgumentNullException(nameof(jobApplicantsIntroductionLetterLogic));
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
            this.userPrincipal=userPrincipal??throw new ArgumentNullException(nameof(userPrincipal));
            this.jobApplicantLogic=jobApplicantLogic??throw new ArgumentNullException(nameof(jobApplicantLogic));
        }

        [ControlPanelMenu("UserAdmission", ParentName = "ManageJobApplicants", Icon = "fa fa-list", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar, Order = 4)]
        public IActionResult Index()
        {
            var isAdmin = userPrincipal.CurrentUser.HasClaim("Permission", ":JobApplicant:AdminPermission");

            if (!isAdmin)
            {
                var relatedUserJobApplicant = jobApplicantLogic.GetByUserId(userPrincipal.CurrentUserId);
                if (relatedUserJobApplicant.ResultStatus == OperationResultStatus.Successful && relatedUserJobApplicant.ResultEntity is not null)
                {
                    var data = relatedUserJobApplicant.ResultEntity.Adapt<UserAdmissionModel>();
                    Model.ModelData=data;

                    if (relatedUserJobApplicant.ResultEntity.ProcessStatus<Enums.ProcessStatus.FirstApprove)
                    {
                        Model.ModelData.IsShow=false;
                        Model.HasGrid=false;
                    }
                    else
                    {
                        Model.HasGrid=true;
                        Model.ModelData.IsShow=true;
                    }
                }
            }           
            Model.ModelData.IsAdmin=isAdmin;
            return View(Model);
        }
        protected override void ModifyItem(ILogic<UserAdmissionModel> service, int id)
        {
            var isAdmin = userPrincipal.CurrentUser.HasClaim("Permission", ":JobApplicant:AdminPermission");
            Model.ModelData.IsAdmin=isAdmin;

            var currentData = service.GetRow(id);

            if (currentData.ResultStatus == OperationResultStatus.Successful && currentData.ResultEntity is not null)
            {
                Model.ModelData.JobApplicantId=currentData.ResultEntity.JobApplicantId;
                Model.ModelData.NoAddictionDone=currentData.ResultEntity.NoAddictionDone;
                Model.ModelData.NoBadBackgroundDone=currentData.ResultEntity.NoBadBackgroundDone;
                Model.ModelData.OccupationalMedicineDone=currentData.ResultEntity.OccupationalMedicineDone;
                Model.ModelData.NoAddictionDate=currentData.ResultEntity?.NoAddictionDate;
                Model.ModelData.NoBadBackgroundDate=currentData.ResultEntity?.NoBadBackgroundDate;
                Model.ModelData.OccupationalMedicineDate=currentData.ResultEntity?.OccupationalMedicineDate;
            }
            var relatedjobApplicant = jobApplicantLogic.GetRow(id);

            if (relatedjobApplicant.ResultStatus == OperationResultStatus.Successful && relatedjobApplicant.ResultEntity is not null)
            {
                if (relatedjobApplicant.ResultEntity.ProcessStatus<Enums.ProcessStatus.FirstApprove)
                {
                    Model.ModelData.IsShow=false;
                    Model.HasGrid=false;
                }
                else
                {
                    Model.ModelData.IsShow=true;
                    Model.HasGrid=true;
                }
            }
        }

        public override IActionResult Save([FromServices] ILogic<UserAdmissionModel> service, UserAdmissionModel model)
        {
            var checkForIntroductions = jobApplicantsIntroductionLetterLogic.GetByJobApplicantId(model.JobApplicantId);

            if (checkForIntroductions.ResultStatus != OperationResultStatus.Successful || !checkForIntroductions.ResultEntity.Any())
            {
                return Json(new { result = "fail", message = localizer["Save is Not Permitted Before Issue Introductions"] });
            }

            var relatedJobApplicantResult = jobApplicantLogic.GetById(model.JobApplicantId);

            if (relatedJobApplicantResult.ResultStatus != OperationResultStatus.Successful || relatedJobApplicantResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[relatedJobApplicantResult.AllMessages] });
            }
            if (relatedJobApplicantResult.ResultEntity.BaseInformationApproveStatus==Enums.ApproveStatus.FinalApprove)
            {

                return Json(new { result = "fail", message = localizer["Approved Data has No Permission For Edit"] });
            }

            if (relatedJobApplicantResult.ResultEntity.ProcessStatus==Enums.ProcessStatus.ApproveHSE)
            {
                return Json(new { result = "fail", message = localizer["Approved Data has No Permission For Edit"] });
            }

            if (!model.NoAddictionDone && model.NoAddictionDate.HasValue)
            {
                return Json(new { result = "fail", message = localizer["Please Check NoAddiction Done CheckBox"] });
            }

            if (!model.OccupationalMedicineDone && model.OccupationalMedicineDate.HasValue)
            {
                return Json(new { result = "fail", message = localizer["Please Check OccupationalMedicine Done CheckBox"] });
            }

            if (!model.NoBadBackgroundDone && model.NoAddictionDate.HasValue)
            {
                return Json(new { result = "fail", message = localizer["Please Check NoBadBackground Done CheckBox"] });
            }

            if (model.NoAddictionDone && model.NoAddictionDate.HasValue && model.NoBadBackgroundDone && model.NoAddictionDate.HasValue)
            {
                relatedJobApplicantResult.ResultEntity.ProcessStatus=Enums.ProcessStatus.AdmittingToDoExpriments;
                jobApplicantLogic.Update(relatedJobApplicantResult.ResultEntity);
            }

            model.NoAddictionApprovedBy=userPrincipal.CurrentUserId;
            model.NoBadBackgroundApprovedBy=userPrincipal.CurrentUserId;
            return base.Save(service, model);
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetGridData(DatatablesSentModel model, string? personnelCode, string? nationalCode, string firstName, string lastName, bool viewInprogressStatus,FlowType? flowType,ProcessStatus? processStatus)
        {

            BusinessOperationResult<List<JobApplicantModel>>? gridData;

            flowType=FlowType.JobApplicant;

            gridData = jobApplicantLogic.GetByFilter(personnelCode, nationalCode, firstName, lastName, viewInprogressStatus, flowType, processStatus, model.Start, model.Length);

            if (gridData.ResultStatus != OperationResultStatus.Successful)
            {
                return Json(new { result = "fail", message = localizer[gridData.AllMessages] });
            }

            var gridNoAdditionApproverIds = gridData.ResultEntity
                .Where(x => x.NoAddictionApprovedBy.HasValue)
                .Select(x => x.NoAddictionApprovedBy.Value)
                .ToList();

            var gridNobadBackgroundApproveIds = gridData.ResultEntity
                .Where(x => x.NoBadBackgroundApprovedBy.HasValue)
                .Select(x => x.NoBadBackgroundApprovedBy.Value)
                .ToList();

            var approversUnionAll = gridNoAdditionApproverIds
                .Union(gridNobadBackgroundApproveIds)
                .ToList();

            var usersInfo = userSharedService.GetUserInfos(approversUnionAll);

            foreach (var item in gridData.ResultEntity)
            {
                var noAdditionApproveUserName = usersInfo.FirstOrDefault(x => x.UserId==item.NoAddictionApprovedBy);

                if (noAdditionApproveUserName != null)
                {

                    item.NoAddictionApprovedByName=noAdditionApproveUserName.Fullname;
                }

                var NoBadbackgroundApproveUserName = usersInfo.FirstOrDefault(x => x.UserId==item.NoBadBackgroundApprovedBy);

                if (NoBadbackgroundApproveUserName != null)
                {

                    item.NoBadBackgroundApprovedByName=NoBadbackgroundApproveUserName.Fullname;
                }
            }

            var totalCount = gridData?.Count ?? 0;

            return Json(new { model.Draw, recordsTotal = totalCount, recordsFiltered = totalCount, data = gridData?.ResultEntity, error = "", result = "ok" });
        }
    }

}
