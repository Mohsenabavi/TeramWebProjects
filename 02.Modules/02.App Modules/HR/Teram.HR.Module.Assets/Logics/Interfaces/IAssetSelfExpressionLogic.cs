using Teram.Framework.Core.Logic;
using Teram.HR.Module.Assets.Entities;
using Teram.HR.Module.Assets.Models;
namespace Teram.HR.Module.Assets.Logics.Interfaces
{
   
    public interface IAssetSelfExpressionLogic : IBusinessOperations<AssetSelfExpressionModel, AssetSelfExpression, int>
    {
        BusinessOperationResult<List<AssetSelfExpressionModel>> GetAllByFilter(Guid? currentUserId, int? start = null, int? length = null);
    }

}
