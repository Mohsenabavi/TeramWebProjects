using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Teram.QC.Module.FinalProduct.Enums
{
    public enum OpinionType
    {

        [Display(Name = "مجوز ارفاقی")]
        [Description("مجوز ارفاقی")]
        Leniency = 1,


        [Display(Name = "نظر مدیرعامل")]
        [Description("نظر مدیرعامل")]
        CEOOpinion = 2,

        [Display(Name = "جداسازی")]
        [Description("جداسازی")]
        Seperation = 3,

        [Display(Name = "ضایعات")]
        [Description("ضایعات")]
        Waste = 4,

    }
}
