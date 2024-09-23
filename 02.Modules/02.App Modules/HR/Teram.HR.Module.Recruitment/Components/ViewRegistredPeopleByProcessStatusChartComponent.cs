using Microsoft.AspNetCore.Mvc;
using Teram.HR.Module.Recruitment.Models.ChartModels;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;

namespace Teram.HR.Module.Recruitment.Components
{

    [Widget(Name = "آمار افراد بر اساس وضعیت", Description = "آمار افراد بر اساس وضعیت",
   Image = "/Images/Widget/move.png", ModuleName = "ViewRegistredPeopleByProcessStatusChartComponent", ColumnSize = 12, PanelType = PanelType.Managment)]
    public class ViewRegistredPeopleByProcessStatusChartComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int moduleId)
        {
            var data = new ChartReportModel { ModuleId = moduleId };
            return View(data);
        }
    }
}
