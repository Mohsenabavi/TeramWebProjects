using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.JobApplicants
{
    public class JobPositionModel :ModelBase<JobPosition,int>
    {
        public int JobPositionId { get; set; }

        [GridColumn(nameof(Title))]
        public string Title { get; set; }
        public bool IsActive {  get; set; }

        [GridColumn(nameof(IsActiveText))]
        public string IsActiveText => IsActive ? "فعال" : "غیر فعال";
    }
}
