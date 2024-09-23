using Microsoft.Extensions.Hosting;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Assets.Entities;
using Teram.HR.Module.Assets.Models;

namespace Teram.HR.Module.Assets.Logics.Interfaces
{
    public interface IRahkaranAssetLogic : IBusinessOperations<RahkaranAssetModel, RahkaranAsset, int>
    {
        BusinessOperationResult<List<RahkaranAssetModel>> GetAssetsByPersonnelCode(int personnelCode);
        BusinessOperationResult<List<RahkaranAssetModel>> GetAllByFilter(string? nationalCode, int? personnelCode, string? title, string? fullName, long? assetID, string? code,
            string? plaqueNumber, string? groupTitle, string? depreciatedMethodTitle,
            string? costCenter, string? settlementPlace, string? collector, int? start = null, int? length = null);     
    }

}
