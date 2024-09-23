using Microsoft.AspNetCore.Mvc;
using Terma.Module.Reports.Services;

namespace Terma.Module.Reports.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IStimulsoftReportService stimulsoftReportService;

        public ReportsController(IStimulsoftReportService stimulsoftReportService)
        {
            this.stimulsoftReportService=stimulsoftReportService??throw new ArgumentNullException(nameof(stimulsoftReportService));
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewReport() {

            stimulsoftReportService.PrintOM("Test");

            return Ok();
        }

    }
}
