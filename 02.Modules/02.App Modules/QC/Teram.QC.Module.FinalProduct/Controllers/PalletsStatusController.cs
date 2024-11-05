using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Logic;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Teram.Web.Core.Model;

namespace Teram.QC.Module.FinalProduct.Controllers
{
    public class PalletsStatusController : ControlPanelBaseController<FinalProductNoncomplianceDetailModel, FinalProductNoncomplianceDetail, int>
    {
        private readonly IFinalProductInspectionLogic finalProductInspectionLogic;
        private readonly IFinalProductNoncomplianceDetailLogic finalProductNoncomplianceDetailLogic;
        private readonly IUserSharedService userSharedService;

        public PalletsStatusController(ILogger<PalletsStatusController> logger
            , IStringLocalizer<PalletsStatusController> localizer, IFinalProductInspectionLogic finalProductInspectionLogic,
            IFinalProductNoncomplianceDetailLogic finalProductNoncomplianceDetailLogic, IUserSharedService userSharedService,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<PalletsStatusModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["PalletsStatus"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["PalletsStatus"],
                OperationColumns = true,
                HomePage = nameof(PalletsStatusController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/QC/Module/FinalProduct/Scripts/PalletsStatus.js",
                GetDataUrl = "",
                LoadAjaxData = false,
            };
            this.finalProductInspectionLogic = finalProductInspectionLogic ?? throw new ArgumentNullException(nameof(finalProductInspectionLogic));
            this.finalProductNoncomplianceDetailLogic = finalProductNoncomplianceDetailLogic ?? throw new ArgumentNullException(nameof(finalProductNoncomplianceDetailLogic));
            this.userSharedService = userSharedService ?? throw new ArgumentNullException(nameof(userSharedService));
        }

        [ControlPanelMenu("PalletsStatus", ParentName = "FinalProductInspection", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetPalletsStatus(
        DatatablesSentModel model,
        string number,
        string orderNo,
        string productName,
        int? sampleCount,
        string tracingCode,
        string productCode)
        {
            var data = finalProductNoncomplianceDetailLogic.GetPalletsStatusAsync(number, orderNo, productName,
                sampleCount, tracingCode, productCode, model.Start, model.Length).Result;
            var totalCount = data.TotalCount;
            var result = data.Items;
            return Json(new { model.Draw, recordsTotal = totalCount, recordsFiltered = totalCount, data = result, error = "", result = "ok" });
        }

    }

}
