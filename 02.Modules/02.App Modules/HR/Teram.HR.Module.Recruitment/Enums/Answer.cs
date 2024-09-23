using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Teram.HR.Module.Recruitment.Enums
{
    public enum Answer
    {
        [Display(Name = "بدون پاسخ")]
        [Description("بدون پاسخ")]
        NoAnswer = 0,
        [Display(Name = "بله")]
        [Description("بله")]
        Yes = 1,
        [Display(Name = "خیر")]
        [Description("خیر")]
        No = 2
    }
}
