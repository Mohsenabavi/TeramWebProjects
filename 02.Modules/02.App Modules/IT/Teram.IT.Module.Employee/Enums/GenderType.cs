using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Teram.IT.Module.Employee.Enums
{
    public enum GenderType
    {
        [Display(Name = "مرد")]
        [Description("مرد")]
        Male = 1,
        [Display(Name = "زن")]
        [Description("زن")]
        Female = 2
    }
}
