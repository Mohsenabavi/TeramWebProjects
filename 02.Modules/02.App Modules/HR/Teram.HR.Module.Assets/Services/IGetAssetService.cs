using Teram.Framework.Core.Logic;
using Teram.HR.Module.Assets.Models.QueryModels;

namespace Teram.HR.Module.Assets.Services
{
    public interface IGetAssetService
    {
        Task<List<AssetQueryModel>> GetAllAssetsFromRahkaran();

        Task<BusinessOperationResult<AssetInfo>> GetAssetByPlaqueNumber(string plaqueNumber);

    }
}
