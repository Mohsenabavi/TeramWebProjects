using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.BaseInfo;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.BaseInfo
{
    public class MajorModel : ModelBase<Major, int>
    {
        public int MajorId { get; set; }

        [GridColumn(nameof(Title))]
        public string Title { get; set; }
    }

}
