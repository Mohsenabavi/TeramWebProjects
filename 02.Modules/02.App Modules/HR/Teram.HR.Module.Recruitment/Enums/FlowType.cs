using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Teram.HR.Module.Recruitment.Enums
{
    public enum FlowType
    {
        [Description("متقاضی استخدام")]
        [Display(Name = "متقاضی استخدام")]
        JobApplicant = 0,

        [Description("شاغل")]
        [Display(Name = "شاغل")]
        Employed = 1,
    }
}
