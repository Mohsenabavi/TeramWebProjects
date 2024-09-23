using Microsoft.AspNetCore.Mvc;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Logic;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.JobApplicants;
using Teram.HR.Module.Recruitment.Models.WorkWithUs;
using Teram.Module.GeographicRegion.Enums;
using Teram.Module.GeographicRegion.Logic;
using Teram.Web.Core.Security;

namespace Teram.HR.Module.Recruitment.Components
{
    public class ViewStepsComponent : ViewComponent
    {
        private readonly IJobApplicantLogic jobApplicantLogic;
        private readonly IUserPrincipal userPrincipal;

        public ViewStepsComponent(IJobApplicantLogic jobApplicantLogic, IUserPrincipal userPrincipal)
        {
            this.jobApplicantLogic=jobApplicantLogic??throw new ArgumentNullException(nameof(jobApplicantLogic));
            this.userPrincipal=userPrincipal??throw new ArgumentNullException(nameof(userPrincipal));
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUserId = userPrincipal.CurrentUserId;
            var jobApplicantResult = jobApplicantLogic.GetByUserId(currentUserId);

            if (jobApplicantResult.ResultStatus != OperationResultStatus.Successful || jobApplicantResult.ResultEntity is null)
            {
                return View("~/Views/Shared/Components/ViewSteps/Default.cshtml", 0);
            }

            var step = await GetStep(jobApplicantResult);

            if (jobApplicantResult.ResultEntity.FlowType==Enums.FlowType.JobApplicant)
            {
                return View("~/Views/Shared/Components/ViewSteps/Default.cshtml", step);
            }
            else
            {
                return View("~/Views/Shared/Components/ViewSteps/EmployeeSteps.cshtml", step);
            }
        }
        public Task<int> GetStep(BusinessOperationResult<JobApplicantModel> jobApplicantModel)
        {
            var task = Task.Run(() =>
            {
                var step = 0;
                if (jobApplicantModel.ResultEntity is null)
                {
                    return step;
                }
                var isAdmin = userPrincipal.CurrentUser.HasClaim("Permission", ":JobApplicant:AdminPermission");

                switch (jobApplicantModel.ResultEntity.FlowType)
                {
                    case Enums.FlowType.JobApplicant:

                        if (isAdmin)
                        {
                            return -1;
                        }

                        if (jobApplicantModel.ResultStatus != OperationResultStatus.Successful || jobApplicantModel.ResultEntity is null)
                        {
                            return step;
                        }

                        if (jobApplicantModel.ResultEntity.ProcessStatus==Enums.ProcessStatus.NoAction)
                        {
                            step=1;
                        }

                        else if (jobApplicantModel.ResultEntity.ProcessStatus==Enums.ProcessStatus.ApproveJobBackground)
                        {
                            step=2;
                        }

                        if (jobApplicantModel.ResultEntity.ProcessStatus==Enums.ProcessStatus.BaseInformationAdded)
                        {
                            step=3;
                        }
                        else if (jobApplicantModel.ResultEntity.ProcessStatus==Enums.ProcessStatus.DoumentsUploaded)
                        {
                            step=4;
                        }

                        else if (jobApplicantModel.ResultEntity.ProcessStatus==Enums.ProcessStatus.FirstApprove)
                        {

                            step=5;
                        }

                        else if (jobApplicantModel.ResultEntity.ProcessStatus==Enums.ProcessStatus.AdmittingToDoExpriments)
                        {

                            step=6;
                        }

                        else if (jobApplicantModel.ResultEntity.ProcessStatus==Enums.ProcessStatus.ApproveHSE)
                        {
                            step=7;
                        }

                        else if (jobApplicantModel.ResultEntity.ProcessStatus==Enums.ProcessStatus.FinalApproveByHR)
                        {

                            step=8;
                        }

                        break;
                    case Enums.FlowType.Employed:
                        if (isAdmin)
                        {
                            return -1;
                        }

                        if (jobApplicantModel.ResultStatus != OperationResultStatus.Successful || jobApplicantModel.ResultEntity is null)
                        {
                            return step;
                        }

                        if (jobApplicantModel.ResultEntity.ProcessStatus==Enums.ProcessStatus.NoAction)
                        {
                            step=1;
                        }

                        if (jobApplicantModel.ResultEntity.ProcessStatus==Enums.ProcessStatus.BaseInformationAdded)
                        {
                            step=2;
                        }
                        else if (jobApplicantModel.ResultEntity.ProcessStatus==Enums.ProcessStatus.DoumentsUploaded)
                        {
                            step=3;
                        }
                        else if (jobApplicantModel.ResultEntity.ProcessStatus==Enums.ProcessStatus.FinalApproveByHR)
                        {

                            step=4;
                        }
                        break;
                }
                ViewBag.Step=step;
                return step;
            });
            return task;
        }
    }
}

