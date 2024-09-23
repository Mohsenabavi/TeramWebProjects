using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Teram.HR.Module.Recruitment.Enums
{
    public enum MaritalStatus
    {
        [Display(Name = "مجرد")]
        [Description("مجرد")]
        Single = 1,

        [Display(Name = "متاهل")]
        [Description("متاهل")]
        Married = 2,

        [Display(Name = "متارکه")]
        [Description("متارکه")]
        Divorced = 3
    }
}
