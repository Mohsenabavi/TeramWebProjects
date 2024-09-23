using Teram.Framework.Core.Logic;
using Teram.Module.GeographicRegion.Enums;
using Teram.Module.GeographicRegion.Models;
using Teram.Web.Core.TagHelpers;

namespace Teram.Module.GeographicRegion.Logic.Interfaces
{

    public interface IGeographicRegionLogic : IBusinessOperations<GeographicRegionModel, Entities.GeographicRegion, int>
    {
        List<SelectItem> GetAllCitiesByProvinceId(int provinceId);
        List<SelectItem> GetAllProvince(int country);
        BusinessOperationResult<List<GeographicRegionModel>> GetByParent(int parentId);
        List<SelectItem> GetByParentId(int parentId);
        BusinessOperationResult<List<GeographicRegionModel>> GetByType(GeographicType geoType);
        BusinessOperationResult<GeographicRegionModel> GetByChildId(int id);
        BusinessOperationResult<GeographicRegionModel> GetById(int id);
        BusinessOperationResult<GeographicRegionModel> GetParentByCode(int id);
        BusinessOperationResult<List<GeographicRegionModel>> GetByParentId(int? parentId, string term);
        BusinessOperationResult<List<GeographicRegionModel>> GetByRegionType(GeographicType geographicType, string term);
    }

}
