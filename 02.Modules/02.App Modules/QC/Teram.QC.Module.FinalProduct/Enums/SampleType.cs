using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Teram.QC.Module.FinalProduct.Enums
{
    public enum SampleType
    {
        [Display(Name = "نمونه اول")]
        [Description("نمونه اول")]
        FirstSample = 1,

        [Display(Name = "نمونه دوم")]
        [Description("نمونه دوم")]
        SecondSample = 2,

        [Display(Name = "نمونه سوم")]
        [Description("نمونه سوم")]
        ThirdSample = 3,

        [Display(Name = "نمونه چهارم")]
        [Description("نمونه چهارم")]
        ForthSample = 4,
    }
}
