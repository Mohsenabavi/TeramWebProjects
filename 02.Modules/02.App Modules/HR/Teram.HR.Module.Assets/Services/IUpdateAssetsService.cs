using Teram.Framework.Core.Logic;
using Teram.HR.Module.Assets.Models;
using Teram.HR.Module.Assets.Models.QueryModels;

namespace Teram.HR.Module.Assets.Services
{
    public interface IUpdateAssetsService
    {
        Task<BusinessOperationResult<List<RahkaranAssetModel>>> UpdateAssets(List<AssetQueryModel> assets);
    }
}
