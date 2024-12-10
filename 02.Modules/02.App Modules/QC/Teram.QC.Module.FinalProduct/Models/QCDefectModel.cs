using Teram.Framework.Core.Attributes;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models
{
    public class QCDefectModel : ModelBase<QCDefect, int>
    {
        public int QCDefectId { get; set; }

        [ExportToExcel("ایراد")]

        [GridColumn(nameof(Title))]
        public string Title { get; set; }

        [ExportToExcel("کد ایراد")]
        [GridColumn(nameof(Code))]
        public string Code { get; set; }

        [ExportToExcel("توضیحات")]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public Guid? UserId {  get; set; }

        [GridColumn(nameof(UserFullName))]
        public string? UserFullName {  get; set; }

        [ExportToExcel("وضعیت")]
        [GridColumn(nameof(IsActiveText))]
        public string IsActiveText => IsActive ? "فعال" : "غیرفعال";
    }
}
