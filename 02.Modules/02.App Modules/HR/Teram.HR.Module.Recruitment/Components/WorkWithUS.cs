using Azure.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Models.WorkWithUs;
using Teram.Module.GeographicRegion.Logic.Interfaces;
using System.ComponentModel;
using Teram.Module.GeographicRegion.Enums;
using Teram.Web.Core.Security;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.Framework.Core.Extensions;
using Teram.Module.GeographicRegion.Logic;
using Teram.ServiceContracts;
using Teram.HR.Module.Recruitment.Enums;

namespace Teram.HR.Module.Recruitment.Components
{
    public class WorkWithUS : ViewComponent
    {
        private readonly IGeographicRegionLogic geographyRegionLogic;
        private readonly IBaseInformationLogic baseInformationLogic;
        private readonly IUserPrincipal userPrincipal;
        private readonly IJobApplicantLogic jobApplicantLogic;
        private readonly IGeographicRegionLogic geographicRegionLogic;
        private readonly IGeoSharedService geoSharedService;

        public WorkWithUS(IGeographicRegionLogic geographyRegionLogic, IBaseInformationLogic baseInformationLogic,
            IUserPrincipal userPrincipal, IJobApplicantLogic jobApplicantLogic, IGeographicRegionLogic geographicRegionLogic, IGeoSharedService geoSharedService)
        {
            this.geographyRegionLogic = geographyRegionLogic;
            this.baseInformationLogic = baseInformationLogic ?? throw new ArgumentNullException(nameof(baseInformationLogic));
            this.userPrincipal = userPrincipal ?? throw new ArgumentNullException(nameof(userPrincipal));
            this.jobApplicantLogic = jobApplicantLogic ?? throw new ArgumentNullException(nameof(jobApplicantLogic));
            this.geographicRegionLogic = geographicRegionLogic ?? throw new ArgumentNullException(nameof(geographicRegionLogic));
            this.geoSharedService = geoSharedService ?? throw new ArgumentNullException(nameof(geoSharedService));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await GetItemsAsync();
            return View("~/Views/Shared/Components/WorkWithUS/Default.cshtml", items);
        }
        private List<SelectListItem> FillProvince()
        {
            var data = geographicRegionLogic.GetByParentId(102072).Select(x => new SelectListItem { Text = x.Text, Value = x.Id }).ToList();
            return data;
        }
        public Task<BaseInformationModel> GetItemsAsync()
        {
            var task = Task.Run(() =>
            {
                var model = new BaseInformationModel();

                ViewBag.provinces = FillProvince();
                var currentUserId = userPrincipal.CurrentUserId;
                var jobApplicantResult = jobApplicantLogic.GetByUserId(currentUserId);

                if (jobApplicantResult.ResultStatus != OperationResultStatus.Successful || jobApplicantResult.ResultEntity is null)
                {
                    model.IsShow = false;
                    return model;
                }                

                if (jobApplicantResult.ResultStatus == OperationResultStatus.Successful && jobApplicantResult.ResultEntity is not null
                && jobApplicantResult.ResultEntity.ProcessStatus<Enums.ProcessStatus.ApproveJobBackground && jobApplicantResult.ResultEntity.FlowType==FlowType.JobApplicant)
                {
                    model.IsShow = false;
                    return model;
                }

                var registeredData = baseInformationLogic.GetByJobApplicantId(jobApplicantResult.ResultEntity.JobApplicantId);

                if (registeredData.ResultStatus != OperationResultStatus.Successful || registeredData.ResultEntity is null)
                {
                    model.FlowType=jobApplicantResult.ResultEntity.FlowType;
                    model.NationalCode = jobApplicantResult.ResultEntity.NationalCode;
                    model.Mobile=jobApplicantResult.ResultEntity.MobileNumber;
                    model.Name = jobApplicantResult.ResultEntity.FirstName;
                    model.Lastname = jobApplicantResult.ResultEntity.LastName;
                    model.JobApplicantId = jobApplicantResult.ResultEntity.JobApplicantId;
                    model.Address=(!string.IsNullOrWhiteSpace(jobApplicantResult.ResultEntity.Address))? jobApplicantResult.ResultEntity.Address:null;
                    model.IsShow = true;
                    model.HasAlreadyRegistered = false;
                    model.IdentityNumber=(jobApplicantResult.ResultEntity.IdentityNumber!= null) ? jobApplicantResult.ResultEntity.IdentityNumber : null;
                    model.FatherName=(jobApplicantResult.ResultEntity.FatherName!= null) ? jobApplicantResult.ResultEntity.FatherName : null;
                    var result = geographyRegionLogic.GetByRegionType(GeographicType.Country, null);
                    return model;
                }
                model = registeredData.ResultEntity;
                model.HasAlreadyRegistered = true;
                var birthLocationInfo = geographicRegionLogic.GetById(model.BirthLocationId.Value);
                if (birthLocationInfo.ResultStatus == OperationResultStatus.Successful && birthLocationInfo.ResultEntity is not null)
                {
                    model.BirthLocationName = birthLocationInfo.ResultEntity.Name;
                }
                ViewBag.CurrentPersianDate = DateTime.Now.ToPersianDate();
                return model;
            });
            return task;
        }
    }
}
