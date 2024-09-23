using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Teram.HR.Module.Recruitment.Enums
{
    [Description("میزان مهارت")]
    public enum SkillLevel
    {

        [Display(Name = "ضعیف")]
        [Description( "ضعیف")]
        Waek = 1,
        [Display(Name = "متوسط")]
        [Description("متوسط")]
        Middle = 2,
        [Display(Name = "عالی")]
        [Description("عالی")]
        Excellent = 3
    }
}
