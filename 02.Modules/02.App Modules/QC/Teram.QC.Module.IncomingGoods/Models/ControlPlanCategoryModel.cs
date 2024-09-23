using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.IncomingGoods.Models
{
    public class ControlPlanCategoryModel : ModelBase<ControlPlanCategory, int>
    {
        public int ControlPlanCategoryId { get; set; }

        [GridColumn(nameof(Title))]
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public string? Remarks { get; set; }

        [GridColumn(nameof(IsActiveText))]
        public string IsActiveText => IsActive ? "فعال" : "غیر فعال";

        [GridColumn(nameof(PersianCreateDate))]
        public string PersianCreateDate => CreateDate.ToPersianDate();

    }
}
