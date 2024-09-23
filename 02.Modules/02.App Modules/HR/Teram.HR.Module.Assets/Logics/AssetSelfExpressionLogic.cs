using DocumentFormat.OpenXml.EMMA;
using System.Linq.Expressions;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Assets.Entities;
using Teram.HR.Module.Assets.Logics.Interfaces;
using Teram.HR.Module.Assets.Models;

namespace Teram.HR.Module.Assets.Logics
{
 
    public class AssetSelfExpressionLogic : BusinessOperations<AssetSelfExpressionModel, AssetSelfExpression, int>, IAssetSelfExpressionLogic
    {
        public AssetSelfExpressionLogic(IPersistenceService<AssetSelfExpression> service) : base(service)
        {

        }

        public BusinessOperationResult<List<AssetSelfExpressionModel>> GetAllByFilter(Guid? currentUserId, int? start = null, int? length = null)
        {
            var query = CreateFilterExpression(currentUserId);

            if (start.HasValue && length.HasValue)
            {
                return GetData<AssetSelfExpressionModel>(query, row: start.Value, max: length.Value, orderByMember: "AssetSelfExpressionId", orderByDescending: true);
            }
            return GetData<AssetSelfExpressionModel>(query, null, null, false, null);
        }

        private Expression<Func<AssetSelfExpression, bool>> CreateFilterExpression(Guid? currentUserId)
        {
            Expression<Func<AssetSelfExpression, bool>> query = x => true;

            if (currentUserId.HasValue && currentUserId!=Guid.Empty)
            {
                query = query.AndAlso(x => x.CreatedBy == currentUserId.Value);
            }          
            return query;
        }
    }
}
