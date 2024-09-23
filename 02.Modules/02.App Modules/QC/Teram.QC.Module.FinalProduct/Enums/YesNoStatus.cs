using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Teram.QC.Module.FinalProduct.Enums
{
    public enum YesNoStatus
    {
        [Display(Name = "خیر")]
        [Description("خیر")]
        No = 0,

        [Display(Name = "بله")]
        [Description("بله")]
        Yes = 1,

    }
}
