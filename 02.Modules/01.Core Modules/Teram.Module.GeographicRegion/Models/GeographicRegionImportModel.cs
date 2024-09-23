using Teram.Framework.Core.Attributes;

namespace Teram.Module.GeographicRegion.Models
{
    public class GeographicRegionImportModel
    {
        [ImportFromExcel(ColumnIndex = 1)]
        public string GeographicType { get; set; }

        [ImportFromExcel(ColumnIndex = 2)]
        public int Code { get; set; }

        [ImportFromExcel(ColumnIndex = 3)]
        public string Name { get; set; }

        [ImportFromExcel(ColumnIndex = 4)]
        public string LatinName { get; set; }

        [ImportFromExcel(ColumnIndex = 5)]
        public int ParentCode { get; set; }
    }
}
