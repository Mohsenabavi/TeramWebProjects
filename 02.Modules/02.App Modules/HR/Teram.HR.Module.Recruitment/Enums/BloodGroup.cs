using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Teram.HR.Module.Recruitment.Enums
{

    [Description("گروه خونی")]
    public enum BloodGroup
    {
        [Description("-O")]
        [Display(Name = "-O")]
        MinusO = 1,

        [Description("+O")]
        [Display(Name = "+O")]
        PositiveO = 2,

        [Description("+A")]
        [Display(Name = "+A")]
        PositiveA = 3,

        [Description("-A")]
        [Display(Name = "-A")]
        MinusA = 4,

        [Description("-B")]
        [Display(Name = "-B")]
        MinusB = 5,

        [Description("+B")]
        [Display(Name = "+B")]
        PositiveB = 6,

        [Description("-AB")]
        [Display(Name = "-AB")]
        MinusAB = 7,
        [Description("+AB")]
        [Display(Name = "+AB")]
        PositiveAB = 8
    }
}
