using OfficeOpenXml;

namespace Teram.HR.Module.Recruitment.Jobs
{
    public class ExcelImportJob
    {
        public void ImportData(string filePath, IProgress<int> progress)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                for (int row = 2; row <= rowCount; row++)
                {                  
                    var progressData = (int)(((double)row / rowCount) * 100);                    
                    progress.Report(progressData);
                }
            }
        }
    }
}
