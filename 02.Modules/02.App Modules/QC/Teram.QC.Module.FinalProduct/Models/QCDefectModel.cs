using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models
{
    public class QCDefectModel : ModelBase<QCDefect, int>
    {
        public int QCDefectId { get; set; }

        [GridColumn(nameof(Title))]
        public string Title { get; set; }

        [GridColumn(nameof(Code))]
        public string Code { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public Guid? UserId {  get; set; }

        [GridColumn(nameof(UserFullName))]
        public string? UserFullName {  get; set; }

        [GridColumn(nameof(IsActiveText))]
        public string IsActiveText => IsActive ? "فعال" : "غیرفعال";
    }
}
