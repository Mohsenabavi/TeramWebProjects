using System.ComponentModel;

namespace Teram.Module.GeographicRegion.Enums
{
    public enum GeographicType
    {
        [Description("قاره")]
        Continent = 1,
        [Description("کشور")]
        Country = 2,
        [Description("استان")]
        Province = 3,
        [Description("شهر")]
        City = 4,
        [Description("شهرستان")]
        County = 5,
        [Description("روستا")]
        Rural = 6
    }
}
