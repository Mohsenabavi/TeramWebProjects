using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Teram.QC.Module.FinalProduct.Enums
{
    public enum QualityControlManagerOpinion
    {
        [Display(Name = "تایید")]
        [Description("تایید")]
        Approved = 1,

        [Display(Name = "اصلاح")]
        [Description("اصلاح")]
        Correction = 2,

        [Display(Name = "بطال")]
        [Description("بطال")]
        Cancellation = 3,

    }
}
