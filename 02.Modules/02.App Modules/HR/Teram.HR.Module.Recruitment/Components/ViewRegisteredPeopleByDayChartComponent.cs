using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Microsoft.AspNetCore.Mvc;
using Teram.HR.Module.Recruitment.Models.ChartModels;

namespace Teram.HR.Module.Recruitment.Components
{

    [Widget(Name = "آمار میزان ثبت نام", Description = "میزان ثبت نام",
     Image = "/Images/Widget/move.png", ModuleName = "ViewRegisteredPeopleByDayChartComponent", ColumnSize = 12, PanelType = PanelType.Managment)]
    public class ViewRegisteredPeopleByDayChartComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int moduleId)
        {
            var data = new ChartReportModel { ModuleId = moduleId };
            return View(data);
        }
    }
}
