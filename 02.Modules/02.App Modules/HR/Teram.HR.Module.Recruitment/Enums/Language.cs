using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Teram.HR.Module.Recruitment.Enums
{
    public enum Language
    {
        [Display(Name = "انگلیسی")]
        [Description("انگلیسی")]
        English = 1,

        [Display(Name = "فرانسوی")]
        [Description("فرانسوی")]
        French = 2,

        [Display(Name = "عربی")]
        [Description("عربی")]
        Arabic = 3,

        [Display(Name = "سایر")]
        [Description("سایر")]
        Other = 4
    }
}
