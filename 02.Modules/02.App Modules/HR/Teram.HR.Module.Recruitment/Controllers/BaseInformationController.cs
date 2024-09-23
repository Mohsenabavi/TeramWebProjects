using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.WorkWthUs;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.WorkWithUs;
using Teram.Module.GeographicRegion.Logic;
using Teram.Module.GeographicRegion.Logic.Interfaces;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;

namespace Teram.HR.Module.Recruitment.Controllers
{
    [ControlPanelMenu("EditBaseInformation", Name = "EditBaseInformation", Icon = "fa fa-tasks", PanelType = PanelType.User, Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar)]
    public class BaseInformationController : ControlPanelBaseController<BaseInformationModel, BaseInformation, int>
    {
      
        private readonly IGeographicRegionLogic geographicRegionLogic;
        private readonly IGeoSharedService geoSharedService;

        public BaseInformationController(ILogger<BaseInformationController> logger
            , IStringLocalizer<BaseInformationController> localizer, IGeographicRegionLogic geographicRegionLogic, IGeoSharedService geoSharedService,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<BaseInformationModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["BaseInformation"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["BaseInformation"],
                OperationColumns = true,
                HomePage = nameof(BaseInformationController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/HR/Module/Recruitment/Scripts/WorkWithUS.js",
            };
            this.geographicRegionLogic = geographicRegionLogic ?? throw new ArgumentNullException(nameof(geographicRegionLogic));
            this.geoSharedService = geoSharedService ?? throw new ArgumentNullException(nameof(geoSharedService));
        }

        [ControlPanelMenu("BaseInformation", ParentName = "EditBaseInformation", Icon = "fa fa-tasks", PanelType = PanelType.User, Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index(int id)
        {
            ViewBag.provinces = FillProvince();
            return View(Model);
        }    

        private List<SelectListItem> FillProvince()
        {
            var data = geographicRegionLogic.GetByParentId(102072).Select(x => new SelectListItem { Text = x.Text, Value = x.Id }).ToList();
            return data;
        }

        protected override void ModifyItem(ILogic<BaseInformationModel> service, int id)
        {
            ViewBag.provinces = FillProvince();
            var selectedBaseInformation = service.GetRow(id);

            if (selectedBaseInformation.ResultStatus != OperationResultStatus.Successful || selectedBaseInformation.ResultEntity is null)
            {
                base.ModifyItem(service, id);
                return;
            }
            #region BirthLocation
            var bithLocationParent = geoSharedService.GetGeoByParentId(selectedBaseInformation.ResultEntity.BirthLocationId.Value);
            ViewBag.ProvinceId = bithLocationParent.GeographicRegionId;
            #endregion            

            #region ContactLocation

            if (selectedBaseInformation.ResultEntity.HomeCityId.HasValue)
            {
                var contactHomeCityParent = geoSharedService.GetGeoByParentId(selectedBaseInformation.ResultEntity.HomeCityId.Value);
                ViewBag.HomeCityProvinceId = contactHomeCityParent.GeographicRegionId;
            }
            #endregion

            Model.ModelData = selectedBaseInformation.ResultEntity;


            base.ModifyItem(service, id);
        }
    }
}
