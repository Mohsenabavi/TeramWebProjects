using Microsoft.AspNetCore.Mvc;
using Teram.Framework.Core.Extensions;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.ChartModels;

namespace Teram.HR.Module.Recruitment.Controllers
{
    public class ViewRegistredPeopleByProcessStatusChartController : Controller
    {
        private readonly IJobApplicantLogic jobApplicantLogic;

        public ViewRegistredPeopleByProcessStatusChartController(IJobApplicantLogic jobApplicantLogic)
        {
            this.jobApplicantLogic=jobApplicantLogic??throw new ArgumentNullException(nameof(jobApplicantLogic));
        }
        public IActionResult GetChartData(int controlPanelDashboardId)
        {
            var chartData = jobApplicantLogic.GetAll().ResultEntity;
            var chartModel = new ColumnDrillDownChartModel
            {
                OveralChartModelSeries = chartData.GroupBy(x => x.ProcessStatus)
              .Select(x => new DataDrillDown
              {
                  Name = x.Key.GetDescription(),
                  Y = x.Count(),                  
              })
              .OrderBy(x => x.Name)
              .ToList(),
                ChartTitle = "آمار افراد بر اساس وضعیت ",
                YAxisTitle = "تعداد",
                XAxisTitle = "وضعیت",
            };

            if (chartModel == null)
                return Json(new { result = "fail", chartModel });

            return Json(new { result = "ok", chartModel });
        }
    }
}
