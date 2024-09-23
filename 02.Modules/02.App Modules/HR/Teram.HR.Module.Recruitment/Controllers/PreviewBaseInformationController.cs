using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.WorkWthUs;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.WorkWithUs;
using Teram.Module.GeographicRegion.Logic;
using Teram.Module.GeographicRegion.Logic.Interfaces;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;

namespace Teram.HR.Module.Recruitment.Controllers
{
    public class PreviewBaseInformationController : ControlPanelBaseController<BaseInformationModel, BaseInformation, int>
    {
        private readonly IGeographicRegionLogic geographicRegionLogic;
        private readonly IBaseInformationLogic baseInformationLogic;

        public PreviewBaseInformationController(ILogger<PreviewBaseInformationController> logger, IGeographicRegionLogic geographicRegionLogic
            , IStringLocalizer<PreviewBaseInformationController> localizer, IBaseInformationLogic baseInformationLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<BaseInformationModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["PreviewBaseInformation"],
                HasGrid = false,
                HasManagmentForm = true,
                Title = localizer["PreviewBaseInformation"],
                OperationColumns = true,
                HomePage = nameof(PreviewBaseInformationController).Replace("Controller", "") + "/index",
            };
            this.geographicRegionLogic=geographicRegionLogic??throw new ArgumentNullException(nameof(geographicRegionLogic));
            this.baseInformationLogic=baseInformationLogic??throw new ArgumentNullException(nameof(baseInformationLogic));
        }
        public IActionResult Index(int id)
        {
            var data = baseInformationLogic.GetByJobApplicantId(id);
            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return View("_MessageContainer", localizer["Information Has Not Been Registered Yet"].ToString());
            }
            var birthLocationInfo = geographicRegionLogic.GetById(data.ResultEntity.BirthLocationId.Value);
            if (birthLocationInfo.ResultStatus == OperationResultStatus.Successful && birthLocationInfo.ResultEntity is not null)
            {
                data.ResultEntity.BirthLocationName = birthLocationInfo.ResultEntity.Name;
            }
            return View("Add", data.ResultEntity);
        }
    }
}
