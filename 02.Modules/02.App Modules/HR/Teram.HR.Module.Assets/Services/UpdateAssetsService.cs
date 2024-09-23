using Mapster;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Assets.Logics.Interfaces;
using Teram.HR.Module.Assets.Models;
using Teram.HR.Module.Assets.Models.QueryModels;

namespace Teram.HR.Module.Assets.Services
{
    public class UpdateAssetsService : IUpdateAssetsService
    {
        private readonly IRahkaranAssetLogic rahkaranAssetLogic;

        public UpdateAssetsService(IRahkaranAssetLogic rahkaranAssetLogic)
        {
            this.rahkaranAssetLogic = rahkaranAssetLogic ?? throw new ArgumentNullException(nameof(rahkaranAssetLogic));
        }
        public async Task<BusinessOperationResult<List<RahkaranAssetModel>>> UpdateAssets(List<AssetQueryModel> assets)
        {
            var updateModel = assets.Adapt<List<RahkaranAssetModel>>();
            var updateResult = await rahkaranAssetLogic.BulkMergeAsync(updateModel,compareColumnNames:new List<string> { "AssetID" },
                updateColumnNames:new List<string> { "AssetID", "PlaqueNumber", "Title", "UtilizeDate", "UtilizationDate", "GroupTitle", "DepreciatedMethodTitle", "CostCenter", "SettlementPlace", "Collector", "FullName", "Cost", "Code", "PersonnelCode", "NationalID" });
            return updateResult;
        }
    }
}
