using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Teram.HR.Module.Recruitment.Enums
{

    [Description("مقطع تحصیلی")]
    public enum EducationGrade
    {
        [Display(Name = "زیر دیپلم")]
        [Description("زیر دیپلم")]
        Highschool = 1,

        [Display(Name = "دیپلم")]
        [Description("دیپلم")]
        Diploma = 2,

        [Display(Name = "فوق دیپلم")]
        [Description("فوق دیپلم")]
        AssociateDegree = 3,


        [Display(Name = "لیسانس")]
        [Description("لیسانس")]
        Bachelor = 4,


        [Display(Name = "فوق لیسانس")]
        [Description("فوق لیسانس")]
        MA = 5,


        [Display(Name = "دکتری")]
        [Description("دکتری")]
        PHD = 6,
    }
}
