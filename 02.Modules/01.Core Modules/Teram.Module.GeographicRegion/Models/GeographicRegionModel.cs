using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Module.GeographicRegion.Enums;
using Teram.Web.Core.Attributes;

namespace Teram.Module.GeographicRegion.Models
{
    public class GeographicRegionModel : ModelBase<Entities.GeographicRegion, int>
    {
        public int GeographicRegionId { get; set; }
        public int? ParentGeographicRegionId { get; set; }
        public GeographicType GeographicType { get; set; }

        [GridColumn(nameof(GeographicTypeName))]
        public string GeographicTypeName => GeographicType.GetDescription();

        [GridColumn(nameof(Code))]
        public int Code { get; set; }

        [GridColumn(nameof(Name))]
        public string Name { get; set; }

        [GridColumn(nameof(LatinName))]
        public string LatinName { get; set; }

        [GridColumn(nameof(ParentName))]
        public string ParentName { get; set; }
    }
}
