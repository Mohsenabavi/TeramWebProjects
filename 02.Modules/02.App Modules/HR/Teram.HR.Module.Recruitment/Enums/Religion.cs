using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Teram.HR.Module.Recruitment.Enums
{
    [Description("دین")]
    public enum Religion
    {
        [Description("اسلام")]
        [Display(Name = "اسلام")]
        Islam = 1,

        [Description("مسیحیت")]
        [Display(Name = "مسیحیت")]
        Christianity = 2,

        [Description("زرتشت")]
        [Display(Name = "زرتشت")]
        Zoroastrianism = 3,

        [Description("یهودیت")]
        [Display(Name = "یهودیت")]
        Jewism = 4,

        [Description("سایر")]
        [Display(Name = "سایر")]
        Other = 5
    }
}
