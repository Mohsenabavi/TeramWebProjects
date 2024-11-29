using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Teram.QC.Module.FinalProduct.Enums
{
    public enum ConcessionarylicenseForRawMaterialStatus
    {
        [Display(Name = "مجوز ارفاقی داشته است")]
        [Description("مجوز ارفاقی داشته است")]
        HasConcessionarylicense = 0,

        [Display(Name = "ایراد پذیرفته شده است ")]
        [Description("ایراد پذیرفته شده است ")]
        IsObjectionAccepted = 1,

        [Display(Name = "خطای بازرسی")]
        [Description("خطای بازرسی")]
        HasInspectionError = 2,
    }
}
