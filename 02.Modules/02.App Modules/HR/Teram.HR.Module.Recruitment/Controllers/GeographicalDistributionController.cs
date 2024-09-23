using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.WorkWithUs;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.HR.Module.Recruitment.Controllers
{
    public class GeographicalDistributionController : BasicControlPanelController
    {
        private readonly IBaseInformationLogic baseInformationLogic;

        public GeographicalDistributionController(ILogger<GeographicalDistributionController> logger
            , IStringLocalizer<GeographicalDistributionController> localizer, IBaseInformationLogic baseInformationLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.baseInformationLogic=baseInformationLogic??throw new ArgumentNullException(nameof(baseInformationLogic));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            this.sharedLocalizer = sharedLocalizer ?? throw new ArgumentNullException(nameof(sharedLocalizer));

            Model = new ViewInformation<string>
            {
                HasGrid = false,
                HasManagmentForm = true,
                Title = localizer["GeographicalDistribution"],
                HomePage = nameof(GeographicalDistributionController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/HR/Module/Recruitment/Scripts/GeographicalDistribution.js",
            };
        }

        [ControlPanelMenu("GeographicalDistribution", ParentName = "ManageJobApplicants", Icon = "fa fa-map-marker", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar , Order =9)]
        public IActionResult Index()
        {
            return View(Model);
        }

        public IActionResult GetAddressPositions()
        {
            var data = baseInformationLogic.GetAll();

            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[data.AllMessages] });
            }

            var result = data.ResultEntity.Where(x => x.Latitude.HasValue && x.Longitude.HasValue).Select(x => new GeographicalDistributionModel
            {
                Lat=x.Latitude.Value,
                Long=x.Longitude.Value,
                Name=x.Name + " " + x.Lastname

            }).ToList();

            return Json(new { result = "ok", message = string.Empty, data = result });
        }
    }
}