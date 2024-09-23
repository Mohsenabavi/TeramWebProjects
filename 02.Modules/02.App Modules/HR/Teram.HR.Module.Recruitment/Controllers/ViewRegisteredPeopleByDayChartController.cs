using Microsoft.AspNetCore.Mvc;
using Teram.Framework.Core.Extensions;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.ChartModels;

namespace Teram.HR.Module.Recruitment.Controllers
{
    public class ViewRegisteredPeopleByDayChartController:Controller
    {
        private readonly IJobApplicantLogic jobApplicantLogic;

        public ViewRegisteredPeopleByDayChartController(IJobApplicantLogic jobApplicantLogic)
        {
            this.jobApplicantLogic=jobApplicantLogic??throw new ArgumentNullException(nameof(jobApplicantLogic));
        }
        public IActionResult GetChartData(int controlPanelDashboardId)
        {
            var endDate = DateTime.Now.AddDays(1);
            var startDate = DateTime.Now.AddDays(-30);

            var chartData = jobApplicantLogic.GetByPeriod(startDate, endDate).ResultEntity;

            var chartModel = new ColumnDrillDownChartModel
            {
                OveralChartModelSeries = chartData
                .GroupBy(x => x.CreateDate.Date)
                .Select(x => new DataDrillDown
                {
                    Name = x.Key.ToPersianDate(),
                    Y = x.Count(),
                    Drilldown = x.Key.ToPersianDate()
                })
                .OrderBy(x => x.Name)
                .ToList(),
                ChartTitle = "گزارش میزان ثبت نام افراد در ماه گذشته ",
                YAxisTitle = "تعداد افراد",
                XAxisTitle = "روزها",
                Categories = chartData.OrderBy(z => z.CreateDate).Select(x => x.PersianCreateDate).ToList()
            };

            if (chartModel == null)
                return Json(new { result = "fail", chartModel });

            return Json(new { result = "ok", chartModel });
        }
    }
}
