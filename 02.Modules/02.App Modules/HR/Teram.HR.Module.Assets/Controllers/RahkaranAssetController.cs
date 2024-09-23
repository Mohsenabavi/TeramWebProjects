using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Assets.Entities;
using Teram.HR.Module.Assets.Logics.Interfaces;
using Teram.HR.Module.Assets.Models;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Teram.Web.Core.Model;
using Teram.Web.Core.Security;

namespace Teram.HR.Module.Assets.Controllers
{
    [ControlPanelMenu("PersonnelAssets", Name = "PersonnelAssets", Icon = "fa fa-tasks", PanelType = PanelType.User, Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar)]
    public class RahkaranAssetController : ControlPanelBaseController<RahkaranAssetModel, RahkaranAsset, int>
    {
        private readonly IRahkaranAssetLogic rahkaranAssetLogic;
        private readonly IUserPrincipal userPrincipal;
        private readonly IUserSharedService userSharedService;

        public RahkaranAssetController(ILogger<RahkaranAssetController> logger
            , IStringLocalizer<RahkaranAssetController> localizer, IRahkaranAssetLogic rahkaranAssetLogic, IUserPrincipal userPrincipal, IUserSharedService userSharedService,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<RahkaranAssetModel>
            {

                Title = localizer["RahkaranAssets"],
                EditInSamePage = true,
                HasGrid = true,
                HasManagmentForm = true,
                ModelData = new RahkaranAssetModel(),
                ExtraScripts = "/ExternalModule/HR/Module/Assets/Scripts/RahkaranAsset.js",
                OperationColumns = true,
                LoadAjaxData = false,
                GridId = "AssetsGrid",
                GetDataUrl = "",
                HomePage = nameof(RahkaranAssetController).Replace("Controller", "") + "/index",
            };
            this.rahkaranAssetLogic = rahkaranAssetLogic ?? throw new ArgumentNullException(nameof(rahkaranAssetLogic));
            this.userPrincipal=userPrincipal??throw new ArgumentNullException(nameof(userPrincipal));
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
        }

        [ControlPanelMenu("RahkaranAssets", ParentName = "PersonnelAssets", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
        public IActionResult AdminPermission()
        {
            return Content("This Is Not Actual Action");
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetAssetsData(DatatablesSentModel model, string? nationalCode, int? personnelCode, string title, string? fullName, long? assetID,
            string? code, string? plaqueNumber, string? groupTitle, string? depreciatedMethodTitle, string? costCenter,
            string? settlementPlace, string? collector)
        {

            var isAdmin = userPrincipal.CurrentUser.HasClaim("Permission", ":RahkaranAsset:AdminPermission");

            if (!isAdmin)
            {
                var currentUserId = userPrincipal.CurrentUserId;
                var userInfo = userSharedService.GetUserById(currentUserId).Result;
                nationalCode=userInfo.Username;
            }

            var assetsResult = rahkaranAssetLogic.GetAllByFilter(nationalCode, personnelCode, title, fullName, assetID, code, plaqueNumber, groupTitle, depreciatedMethodTitle, costCenter, settlementPlace, collector, model.Start, model.Length);
            if (assetsResult.ResultStatus != OperationResultStatus.Successful || assetsResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[assetsResult.AllMessages] });
            }
            var totalCount = assetsResult?.Count ?? 0;
            return Json(new { model.Draw, recordsTotal = totalCount, recordsFiltered = totalCount, data = assetsResult?.ResultEntity, error = "", result = "ok" });
        }

        [HttpGet]      
        public IActionResult PrintExcel(string? nationalCode, int? personnelCode, string? title, string? fullName, long? assetID,
            string? code, string? plaqueNumber, string? groupTitle, string? depreciatedMethodTitle, string? costCenter,
            string? settlementPlace, string? collector)
        {
            try
            {
                var assetsResult = rahkaranAssetLogic.GetAllByFilter(nationalCode,personnelCode, title, fullName, assetID, code, plaqueNumber, groupTitle, depreciatedMethodTitle, costCenter, settlementPlace, collector);
                if (assetsResult.ResultStatus != OperationResultStatus.Successful || assetsResult.ResultEntity is null)
                {
                    return Json(new { result = "fail", message = localizer[assetsResult.AllMessages] });
                }
                var excelData = assetsResult.ResultEntity.ExportListExcel("لسیت دارایی های سیستم راهکاران");
                if (excelData is null)
                {
                    return Json(new { result = "fail", total = 0, rows = new List<RahkaranAssetModel>(), message = localizer["Unable to create file due to technical problems."] });
                }

                var fileName = "AssetsList-" + DateTime.Now.ToPersianDate();
                return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + ".xlsx");

            }
            catch (Exception ex)
            {
                logger.LogError(3002, ex, "PrintExcel: " + ex.Message);
                return Json(new { result = "fail", total = 0, rows = new List<RahkaranAssetModel>(), message = localizer["Unable to create file due to technical problems."] });
            }
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult ShowAssetApproveInfo(int rahkaranAssetId)
        {
            var data = rahkaranAssetLogic.GetRow(rahkaranAssetId);
            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[data.AllMessages] });
            }
            return PartialView("_ApproveAsset", data.ResultEntity);
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult UpdateAssetStatus(RahkaranAssetModel model)
        {
            var selectedRowData = rahkaranAssetLogic.GetRow(model.RahkaranAssetId);
            if (selectedRowData.ResultStatus != OperationResultStatus.Successful || selectedRowData.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[selectedRowData.AllMessages] });
            }
            selectedRowData.ResultEntity.ApproverRemarks = model.ApproverRemarks;
            selectedRowData.ResultEntity.ApproveDate=DateTime.Now;
            selectedRowData.ResultEntity.ApproveStatus = model.ApproveStatus;
            selectedRowData.ResultEntity.ApprovedBy=userPrincipal.CurrentUserId;
            rahkaranAssetLogic.Update(selectedRowData.ResultEntity);
            return Json(new { result = "ok", message = localizer["Saved Successfully"] });
        }
    }

}
