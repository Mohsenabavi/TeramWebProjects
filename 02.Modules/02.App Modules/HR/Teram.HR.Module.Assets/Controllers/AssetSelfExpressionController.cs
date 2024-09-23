using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Assets.Entities;
using Teram.HR.Module.Assets.Logics;
using Teram.HR.Module.Assets.Logics.Interfaces;
using Teram.HR.Module.Assets.Models;
using Teram.HR.Module.Assets.Services;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Teram.Web.Core.Model;
using Teram.Web.Core.Security;

namespace Teram.HR.Module.Assets.Controllers
{

    public class AssetSelfExpressionController : ControlPanelBaseController<AssetSelfExpressionModel, AssetSelfExpression, int>
    {
        private readonly IGetAssetService getAssetService;
        private readonly IUserPrincipal userPrincipal;
        private readonly IUserSharedService userSharedService;
        private readonly IAssetSelfExpressionLogic assetSelfExpressionLogic;

        public AssetSelfExpressionController(ILogger<AssetSelfExpressionController> logger
            , IStringLocalizer<AssetSelfExpressionController> localizer, IGetAssetService getAssetService, IUserPrincipal userPrincipal, IUserSharedService userSharedService,
            IStringLocalizer<SharedResource> sharedLocalizer, IAssetSelfExpressionLogic assetSelfExpressionLogic)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<AssetSelfExpressionModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["AssetSelfExpression"],
                HasGrid = true,
                GetDataUrl = "",
                LoadAjaxData = false,
                HasManagmentForm = true,
                Title = localizer["AssetSelfExpression"],
                OperationColumns = true,
                HomePage = nameof(AssetSelfExpressionController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/HR/Module/Assets/Scripts/AssetSelfExpression.js",
            };
            this.getAssetService=getAssetService??throw new ArgumentNullException(nameof(getAssetService));
            this.userPrincipal=userPrincipal??throw new ArgumentNullException(nameof(userPrincipal));
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
            this.assetSelfExpressionLogic=assetSelfExpressionLogic??throw new ArgumentNullException(nameof(assetSelfExpressionLogic));
        }
        [ControlPanelMenu("AssetSelfExpression", ParentName = "PersonnelAssets", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }

        [ParentalAuthorize(nameof(Index))]
        public async Task<IActionResult> GetAssetInfoByPlaqueNumber(string plaqueNumber)
        {

            var data = await getAssetService.GetAssetByPlaqueNumber(plaqueNumber);

            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[data.AllMessages] });
            }

            return Json(new { result = "ok", data = data.ResultEntity, message = localizer[data.AllMessages] });
        }


        [ParentalAuthorize(nameof(Index))]
        public override IActionResult Save([FromServices] ILogic<AssetSelfExpressionModel> service, AssetSelfExpressionModel model)
        {

            model.CreateDate=DateTime.Now;
            model.CreatedBy=userPrincipal.CurrentUserId;
            var currentUser = userSharedService.GetUserById(userPrincipal.CurrentUserId).Result;
            model.UserName=currentUser.Username;
            return base.Save(service, model);
        }


        public IActionResult AdminPermission() {

            return Content("This is not Actual Action");
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetAssetsData(DatatablesSentModel model)
        {

            var currentUserId = Guid.Empty;

            var isAdmin = userPrincipal.CurrentUser.HasClaim("Permission", ":AssetSelfExpression:AdminPermission");

            if (!isAdmin)
            {
               currentUserId = userPrincipal.CurrentUserId;                
            }

            var assetsResult = assetSelfExpressionLogic.GetAllByFilter(currentUserId, model.Start, model.Length);
            if (assetsResult.ResultStatus != OperationResultStatus.Successful || assetsResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[assetsResult.AllMessages] });
            }
            var totalCount = assetsResult?.Count ?? 0;
            return Json(new { model.Draw, recordsTotal = totalCount, recordsFiltered = totalCount, data = assetsResult?.ResultEntity, error = "", result = "ok" });
        }
    }

}
