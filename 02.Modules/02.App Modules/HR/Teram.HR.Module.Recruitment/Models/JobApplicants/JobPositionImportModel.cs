using Teram.Framework.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.JobApplicants
{
    public class JobPositionImportModel
    {
        [ImportFromExcel(ColumnIndex = 1)]
        public string Title { get; set; }
    }
}
