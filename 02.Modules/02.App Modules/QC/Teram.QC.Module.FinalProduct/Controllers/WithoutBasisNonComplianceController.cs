using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Models;
using Teram.QC.Module.FinalProduct.Models.ServiceModels;
using Teram.QC.Module.FinalProduct.Services;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.QC.Module.FinalProduct.Controllers
{
  
    public class WithoutBasisNonComplianceController : ControlPanelBaseController<FinalProductNoncomplianceModel, FinalProductNoncompliance, int>
    {
        private readonly IQueryService queryService;

        public WithoutBasisNonComplianceController(ILogger<WithoutBasisNonComplianceController> logger
            , IStringLocalizer<WithoutBasisNonComplianceController> localizer, IQueryService queryService,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;
            Model = new ViewInformation<FinalProductNoncomplianceModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["WithoutBasisNonCompliance"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["WithoutBasisNonCompliance"],
                OperationColumns = true,
                HomePage = nameof(WithoutBasisNonComplianceController).Replace("Controller", "") + "/index",
                ExtraScripts="/ExternalModule/QC/Module/FinalProduct/Scripts/WithoutBasisNonCompliance.js",
            };
            this.queryService=queryService??throw new ArgumentNullException(nameof(queryService));
        }

        [ControlPanelMenu("WithoutBasisNonCompliance", ParentName = "FinalProductInspection", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
        public async Task<IActionResult> FetchRahkaranData(int orderNo)
        {
            var result = await queryService.GetOrderProducts(orderNo);
            if (result.ResultEntity.Any())
            {
                ViewBag.GoodsInfo=result;
                return Json(new
                {
                    message = "ok",
                    results = result.ResultEntity
                });
            }
            else
            {
                return Json(new
                {
                    message = "fail",
                    results = new List<OrderProductModel>()
                });
            }
        }
    }

}
