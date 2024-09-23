using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Teram.QC.Module.FinalProduct.Enums
{
    public enum ApproveStatus
    {
        [Display(Name = "ثبت نشده")]
        [Description("ثبت نشده")]
        NotRegistered = 0,

        [Display(Name = "تایید ")]
        [Description("تایید ")]
        Approved = 1,

        [Display(Name = "اصلاح")]
        [Description("اصلاح")]
        Modification = 2,

        [Display(Name = "باطل ")]
        [Description("باطل ")]
        Void = 3,
    }
}
