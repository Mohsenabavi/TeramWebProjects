using Mapster;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.Module.GeographicRegion.Enums;
using Teram.Module.GeographicRegion.Logic.Interfaces;
using Teram.Module.GeographicRegion.Models;
using Teram.ServiceContracts.Enum;
using Teram.ServiceContracts;
using Teram.Web.Core.Extensions;
using Teram.Web.Core.TagHelpers;
using System.Linq.Expressions;
using Teram.Module.GeographicRegion.Entities;
using Teram.Framework.Core.Extensions;

namespace Teram.Module.GeographicRegion.Logic
{

    public class GeographicRegionLogic : BusinessOperations<GeographicRegionModel, Entities.GeographicRegion, int>, IGeographicRegionLogic, IGeoSharedService
    {

        public GeographicRegionLogic(IPersistenceService<Entities.GeographicRegion> service) : base(service)
        {
           
        }
        public List<SelectItem> GetAllCitiesByProvinceId(int provinceId)
        {
            var data = GetData<GeographicRegionModel>(x => x.GeographicType == GeographicType.City && x.ParentGeographicRegionId == provinceId);
            return data.ResultEntity.ToSelectListItem(nameof(GeographicRegionModel.Name), nameof(GeographicRegionModel.GeographicRegionId));
        }
        public List<GeoRegionInfo> GetAllGeoByParentId(int parentId)
        {
            var result = GetByParent(parentId);
            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity == null)
            {
                return new List<GeoRegionInfo>();
            }

            return result.ResultEntity.Select(x => new GeoRegionInfo
            {
                Code = x.Code,
                GeographicRegionId = x.GeographicRegionId,
                Name = x.Name,
                LatinName = x.LatinName,
                GeographicType = (ServiceContracts.Enum.GeoType)x.GeographicType,
                ParentGeographicRegionId = x.ParentGeographicRegionId
            }).ToList();
        }
        public List<GeoRegionInfo> GetCityByName(string name)
        {
            var result = GetByProject<GeoRegionInfo>(x => x.GeographicType == GeographicType.City && x.Name.Contains(name));
            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity is null)
            {
                return new List<GeoRegionInfo>();
            }

            return result.ResultEntity;
        }
        public List<GeoRegionInfo> GetProvinceByName(string name)
        {
            var result = GetByProject<GeoRegionInfo>(x => x.GeographicType == GeographicType.Province && x.Name.Contains(name));
            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity is null)
            {
                return new List<GeoRegionInfo>();
            }

            return result.ResultEntity;
        }
        public List<GeoRegionInfo> GetAllByName(string name)
        {
            var result = GetByProject<GeoRegionInfo>(x => x.Name.Contains(name));
            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity is null)
            {
                return new List<GeoRegionInfo>();
            }

            return result.ResultEntity;
        }
        public GeoRegionInfo GetGeoByParentId(int parentId)
        {

            var result = GetByChildId(parentId);
            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity == null)
            {
                return new GeoRegionInfo();
            }

            return new GeoRegionInfo
            {
                Code = result.ResultEntity.Code,
                GeographicRegionId = result.ResultEntity.GeographicRegionId,
                Name = result.ResultEntity.Name,
                LatinName = result.ResultEntity.LatinName,
                GeographicType = (ServiceContracts.Enum.GeoType)result.ResultEntity.GeographicType,
                ParentGeographicRegionId = result.ResultEntity.ParentGeographicRegionId
            };
        }
        public GeoRegionInfo GetGeoByChildId(int childId)
        {

            var result = GetByChildId(childId);
            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity == null)
            {
                return new GeoRegionInfo();
            }

            return new GeoRegionInfo
            {
                Code = result.ResultEntity.Code,
                GeographicRegionId = result.ResultEntity.GeographicRegionId,
                Name = result.ResultEntity.Name,
                LatinName = result.ResultEntity.LatinName,
                GeographicType = (ServiceContracts.Enum.GeoType)result.ResultEntity.GeographicType,
                ParentGeographicRegionId = result.ResultEntity.ParentGeographicRegionId
            };
        }
        public List<SelectItem> GetAllProvince(int country)
        {
            var data = GetData<GeographicRegionModel>(x => x.GeographicType == GeographicType.Province && x.ParentGeographicRegionId == country);
            return data.ResultEntity.ToSelectListItem(nameof(GeographicRegionModel.Name), nameof(GeographicRegionModel.GeographicRegionId));
        }
        public BusinessOperationResult<GeographicRegionModel> GetById(int id) => GetFirst<GeographicRegionModel>(x => x.GeographicRegionId == id);
        public BusinessOperationResult<List<GeographicRegionModel>> GetByParent(int parentId)
        {
            return GetData<GeographicRegionModel>(x => x.ParentGeographicRegionId == parentId);
        }
        public List<GeoRegionInfo> GetSamecitiesByCity(int cityId)
        {
            var cityResult = GetFirstByProject<GeoRegionInfo>(x => x.GeographicRegionId == cityId);
            if (cityResult.ResultStatus != OperationResultStatus.Successful || cityResult.ResultEntity is null)
            {
                return new List<GeoRegionInfo>();
            }

            int? provinceId = cityResult.ResultEntity.ParentGeographicRegionId;
            if (provinceId == null)
            {
                return new List<GeoRegionInfo> { cityResult.ResultEntity };
            }
            var result = GetByProject<GeoRegionInfo>(x => x.ParentGeographicRegionId == provinceId && x.GeographicType == GeographicType.City);
            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity is null)
            {
                return new List<GeoRegionInfo>();
            }
            return result.ResultEntity;
        }
        public List<SelectItem> GetAllProvince()
        {
            return GetAllProvince(87);
        }
        public List<SelectItem> GetByParentId(int parentId)
        {
            var data = GetData<GeographicRegionModel>(x => x.ParentGeographicRegionId == parentId);
            return data.ResultEntity.ToSelectListItem(nameof(GeographicRegionModel.Name), nameof(GeographicRegionModel.GeographicRegionId));

        }
        public BusinessOperationResult<List<GeographicRegionModel>> GetByType(GeographicType geoType)
        {
            return GetData<GeographicRegionModel>(x => x.GeographicType == geoType);
        }
        public BusinessOperationResult<GeographicRegionModel> GetByChildId(int id)
        {

            var dataResult = GetFirst<GeographicRegionModel>(x => x.GeographicRegionId == id);
            if (dataResult.ResultStatus != OperationResultStatus.Successful || dataResult.ResultEntity is null)
            {
                return new BusinessOperationResult<GeographicRegionModel>();
            }

            int? parentId = dataResult.ResultEntity.ParentGeographicRegionId;
            if (parentId != null)
            {
                var result = GetFirst<GeographicRegionModel>(x => x.GeographicRegionId == parentId);
                return result;
            }

            return new BusinessOperationResult<GeographicRegionModel>();
        }
        public BusinessOperationResult<GeoRegionInfo> GetParentById(int id)
        {
            var finalResult = new BusinessOperationResult<GeoRegionInfo>();

            var data = GetFirst<GeographicRegionModel>(x => x.GeographicRegionId == id);
            if (data != null)
            {
                int? parentId = data.ResultEntity.ParentGeographicRegionId;
                if (parentId != null)
                {
                    var result = GetFirst<GeographicRegionModel>(x => x.GeographicRegionId == parentId).ResultEntity.Adapt<GeoRegionInfo>();
                    finalResult.SetSuccessResult(result);
                    return finalResult;
                }
            }
            return new BusinessOperationResult<GeoRegionInfo>();

        }
        public BusinessOperationResult<GeoRegionInfo> GetByGeoId(int id)
        {
            var result = new BusinessOperationResult<GeoRegionInfo>();
            var data = GetFirst<GeographicRegionModel>(x => x.GeographicRegionId == id).ResultEntity.Adapt<GeoRegionInfo>();
            result.SetSuccessResult(data);
            return result;
        }
        BusinessOperationResult<List<GeoRegionInfo>> IGeoSharedService.GetByParent(int parentId)
        {
            var result = GetData<GeographicRegionModel>(x => x.ParentGeographicRegionId == parentId);
            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity is null)
            {
                new List<GeoRegionInfo>();
            }
            var sharedModel = result?.ResultEntity?.Select(x => new GeoRegionInfo
            {
                Code = x.Code,
                GeographicRegionId = x.GeographicRegionId,
                GeographicType = (GeoType)x.GeographicType,
                LatinName = x.LatinName,
                Name = x.Name,
                ParentGeographicRegionId = x.ParentGeographicRegionId,
            }).ToList();
            var sharedResult = new BusinessOperationResult<List<GeoRegionInfo>>();
            sharedResult.SetSuccessResult(sharedModel);
            return sharedResult;
        }
        public List<SelectItem> GetByIds(List<int> ids)
        {
            var result = GetData<GeographicRegionModel>(x => ids.Contains(x.GeographicRegionId));
            return result.ResultEntity.ToSelectListItem(nameof(GeographicRegionModel.GeographicRegionId), nameof(GeographicRegionModel.Name));
        }
        List<GeoRegionInfo> IGeoSharedService.GetGeographicByIds(List<int> ids)
        {
            var result = GetData<GeographicRegionModel>(x => ids.Contains(x.GeographicRegionId));
            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity is null)
            {
                new List<GeoRegionInfo>();
            }

            return result?.ResultEntity?.Select(x => new GeoRegionInfo
            {
                Code = x.Code,
                GeographicRegionId = x.GeographicRegionId,
                GeographicType = (GeoType)x.GeographicType,
                LatinName = x.LatinName,
                Name = x.Name,
                ParentName = x.ParentName,
                ParentGeographicRegionId = x.ParentGeographicRegionId,
            }).ToList();
        }

        public BusinessOperationResult<GeographicRegionModel> GetParentByCode(int id)
        {
            return GetFirst<GeographicRegionModel>(x => x.Code == id);
        }

        public BusinessOperationResult<List<GeographicRegionModel>> GetByParentId(int? parentId, string term)
        {

            Expression<Func<Entities.GeographicRegion, bool>> query = x => x.ParentGeographicRegionId == parentId;
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.AndAlso(x => x.Name.Contains(term));
            }
            return GetData<GeographicRegionModel>(query);
        }

        public BusinessOperationResult<List<GeographicRegionModel>> GetByRegionType(GeographicType geographicType, string term)
        {
            Expression<Func<Entities.GeographicRegion, bool>> query = x => x.GeographicType == geographicType;
            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.AndAlso(x => x.Name.Contains(term));
            }
            return GetData<GeographicRegionModel>(query);
        }
    }
}



