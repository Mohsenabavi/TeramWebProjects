using Newtonsoft.Json;

namespace Teram.HR.Module.Recruitment.Models.ChartModels
{
    public class ColumnDrillDownChartModel
    {
        /// <summary>
        /// عنوان نمودار
        /// </summary>
        public string ChartTitle { get; set; }
        /// <summary>
        /// آیتم های محور افقی
        /// </summary>
        public List<string> Categories { get; set; }
        /// <summary>
        /// دیتای خط ها
        /// </summary>
        public List<DataDrillDown> OveralChartModelSeries { get; set; }
        /// <summary>
        /// عنوان محور عمودی
        /// </summary>
        public string YAxisTitle { get; set; }
        /// <summary>
        /// عنوان محور افقی
        /// </summary>
        public string XAxisTitle { get; set; }

        /// <summary>
        /// مقادیر تشریح شده در نمودار
        /// </summary>
        public List<drilldownSerries> DrillDownSerries { get; set; }
        public List<Series> Series { get; set; }
    }

    public class DataDrillDown
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }

        [JsonProperty("drilldown")]
        public string Drilldown { get; set; }
    }

    public class drilldownSerries
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("data")]
        public List<List<object>> Data { get; set; }
    }

    public class Series
    {
        public string Name { get; set; }

        public List<int> Data { get; set; }
    }
}
