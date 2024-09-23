using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Teram.HR.Module.Recruitment.Enums
{
    [Description("وضعیت وظیفه خدمت")]
    public enum MilitaryServiceStatus
    {
        [Display(Name = "پایان خدمت")]
        [Description("پایان خدمت")]
        TheEndOfService = 1,

        
        [Display(Name = "معافیت دائم")]
        [Description("معافیت دائم")]
        PermanentExemptionCard = 2,


        [Display(Name = "کارت معافیت پزشکی")]
        [Description("کارت معافیت پزشکی")] 
        MedicalExemptionCard = 3,
        
        
        [Display(Name = "کارت معافیت کفالت")]
        [Description("کارت معافیت کفالت")] 
        BailBondsmanshipExemptionCard = 4,
        
        
        [Display(Name = "کارت معافیت دائم صلح")]
        [Description("کارت معافیت دائم صلح")] 
        PeacePermanentExemptionCard = 5,

        [Display(Name = "کارت خرید خدمت")]
        [Description("کارت خرید خدمت")] 
        PurchasingMilitaryService = 6,

        [Display(Name = "کارت معافیت تحصیلی")]
        [Description("کارت معافیت تحصیلی")] 
        EducationalExemption = 7,


        [Display(Name = "معافیت موارد خاص")]
        [Description("معافیت موارد خاص")]
        ExemptionForSpecificCases= 8,
    }
}
