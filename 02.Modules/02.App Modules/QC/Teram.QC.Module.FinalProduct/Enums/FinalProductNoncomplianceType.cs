using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Teram.QC.Module.FinalProduct.Enums
{
    public enum FinalProductNoncomplianceType
    {
        [Display(Name = "مبنا دار")]
        [Description("مبنا دار")]
        HasBasis = 1,
        [Display(Name = "بدون مبنا")]
        [Description("بدون مبنا")]
        WithoutBasis = 2
    }
}
