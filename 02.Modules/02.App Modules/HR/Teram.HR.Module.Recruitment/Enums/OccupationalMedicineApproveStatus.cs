using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Teram.HR.Module.Recruitment.Enums
{
    public enum OccupationalMedicineApproveStatus
    {
        [Description("ثبت اولیه")]
        [Display(Name = "ثبت اولیه")]
        NoAction = 0,
        [Description("تایید")]
        [Display(Name = "تایید")]
        Approve = 1,
        [Description("عدم تایید")]
        [Display(Name = "عدم تایید")]
        DisApprove = 2,
        [Display(Name = "ارجاع")]
        [Description("ارجاع")]
        Referral = 3,
        [Display(Name = "مشروط")]
        [Description("مشروط")]
        Conditionally = 4,
    }
}
