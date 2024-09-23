using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Assets.Entities;
using Teram.HR.Module.Assets.Logics.Interfaces;
using Teram.HR.Module.Assets.Models;
using Teram.ServiceContracts;

namespace Teram.HR.Module.Assets.Logics
{

    public class RahkaranAssetLogic : BusinessOperations<RahkaranAssetModel, RahkaranAsset, int>, IRahkaranAssetLogic
    {
        public RahkaranAssetLogic(IPersistenceService<RahkaranAsset> service) : base(service)
        {

        }

        public BusinessOperationResult<List<RahkaranAssetModel>> GetAllByFilter(string? nationalCode,  int? personnelCode, string? title, string? fullName, long? assetID, string? code,
            string? plaqueNumber, string? groupTitle, string? depreciatedMethodTitle,
            string? costCenter, string? settlementPlace, string? collector, int? start = null, int? length = null)
        {

            var query = CreateFilterExpression(nationalCode,personnelCode, title, fullName, assetID, code, plaqueNumber, groupTitle, depreciatedMethodTitle, costCenter, settlementPlace, collector);

            if (start.HasValue && length.HasValue)
            {
                return GetData<RahkaranAssetModel>(query, row: start.Value, max: length.Value, orderByMember: "PersonnelCode", orderByDescending: true);
            }
            return GetData<RahkaranAssetModel>(query, null, null, false, null);
        }    

        public BusinessOperationResult<List<RahkaranAssetModel>> GetAssetsByPersonnelCode(int personnelCode)
        {
            return GetData<RahkaranAssetModel>(x => x.PersonnelCode == personnelCode);
        }

        private Expression<Func<RahkaranAsset, bool>> CreateFilterExpression(string? nationalCode, int? personnelCode, string? title, string? fullName, long? assetID, string? code,
            string? plaqueNumber, string? groupTitle, string? depreciatedMethodTitle,
            string? costCenter, string? settlementPlace, string? collector)
        {
            Expression<Func<RahkaranAsset, bool>> query = x => true;

            if (!string.IsNullOrEmpty(nationalCode))
            {
                query = query.AndAlso(x => x.NationalID.Contains(nationalCode));
            }

            if (personnelCode.HasValue && personnelCode.Value > 0)
            {
                query = query.AndAlso(x => x.PersonnelCode == personnelCode);
            }

            if (!string.IsNullOrEmpty(title))
            {
                query = query.AndAlso(x => x.Title.Contains(title));
            }

            if (!string.IsNullOrEmpty(fullName))
            {
                query = query.AndAlso(x => x.FullName.Contains(fullName));
            }
            if (assetID.HasValue && assetID.Value > 0)
            {
                query = query.AndAlso(x => x.AssetID == assetID);
            }
            if (!string.IsNullOrEmpty(code))
            {
                query = query.AndAlso(x => x.Code.Contains(code));
            }
            if (!string.IsNullOrEmpty(plaqueNumber))
            {
                query = query.AndAlso(x => x.PlaqueNumber.Contains(plaqueNumber));
            }
            if (!string.IsNullOrEmpty(groupTitle))
            {
                query = query.AndAlso(x => x.GroupTitle.Contains(groupTitle));
            }

            if (!string.IsNullOrEmpty(depreciatedMethodTitle))
            {
                query = query.AndAlso(x => x.DepreciatedMethodTitle.Contains(depreciatedMethodTitle));
            }

            if (!string.IsNullOrEmpty(costCenter))
            {
                query = query.AndAlso(x => x.CostCenter.Contains(costCenter));
            }

            if (!string.IsNullOrEmpty(settlementPlace))
            {
                query = query.AndAlso(x => x.SettlementPlace.Contains(settlementPlace));
            }

            if (!string.IsNullOrEmpty(collector))
            {
                query = query.AndAlso(x => x.Collector.Contains(collector));
            }

            return query;
        }
    }

}
