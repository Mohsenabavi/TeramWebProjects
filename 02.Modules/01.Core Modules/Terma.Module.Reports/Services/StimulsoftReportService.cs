using Stimulsoft.Base;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using System.Collections;
using System.Text;

namespace Terma.Module.Reports.Services
{
    public class StimulsoftReportService : IStimulsoftReportService
    {
        private string StimulsoftReportFilePath = "wwwroot/Reports/";
        public StiReport PrintOM(string model)
        {            
            StiReport report = new StiReport();
            report.Load(StimulsoftReportFilePath + "//report.mrt");
            report.RegData("VisitePrint", model);                      
            report.Render(false);
            StiPdfExportSettings settings = new StiPdfExportSettings();
            settings.AutoPrintMode = StiPdfAutoPrintMode.None;                    
            report.ExportDocument(StiExportFormat.Pdf, StimulsoftReportFilePath + "//print2.pdf", settings);
            return report;
        }
    }
}
