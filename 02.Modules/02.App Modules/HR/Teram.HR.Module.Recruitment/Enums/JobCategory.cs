using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Teram.HR.Module.Recruitment.Enums
{
    public enum JobCategory
    {
        [Display(Name = "کارمندی")]
        [Description("کارمندی")]
        Worker = 1,
        [Display(Name = "اداری")]
        [Description("اداری")]
        Employee = 2
    }
}
