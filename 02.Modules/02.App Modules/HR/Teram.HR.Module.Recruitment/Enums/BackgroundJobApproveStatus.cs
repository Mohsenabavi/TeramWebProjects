using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Teram.HR.Module.Recruitment.Enums
{
    public enum BackgroundJobApproveStatus
    {
        [Display(Name = "تایید")]
        [Description("تایید ")]
        Approved = 1,
        [Display(Name = "عدم تایید")]
        [Description("عدم تایید")]
        Disapproved = 2
    }
}
