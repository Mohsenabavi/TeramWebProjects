using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Teram.QC.Module.FinalProduct.Enums
{
    public enum ReferralStatus
    {
        [Display(Name = "ثبت اولیه")]
        [Description("ثبت اولیه")]
        InitialRegistration = 1,

        [Display(Name = "ارجاع به مدیر کنترل کیفی")]
        [Description("ارجاع به مدیر کنترل کیفی")]
        ReferredToQCManager = 2,

        [Display(Name = "ارجاع به  سرپرست کنترل کیفی")]
        [Description("ارجاع به  سرپرست کنترل کیفی")]
        ReferredToQCSupervisor = 3,

        [Display(Name = "ارجاع به مدیر تولید / جایگزین مدیر تولید")]
        [Description("ارجاع به مدیر تولید / جایگزین مدیر تولید")]
        ReferredToProductionManager = 4,

        [Display(Name = "ارجاع به بررسی کنندگان")]
        [Description("ارجاع به بررسی کنندگان")]
        ReferredToReviewers = 5,

        [Display(Name = "ارجاع شده به واحد جداسازی")]
        [Description("ارجاع شده به واحد جداسازی")]
        ReferredToISeparationUnit = 6,

        [Display(Name = "ارجاع به  مدیر عامل")]
        [Description("ارجاع به  مدیر عامل")]
        ReferredToCEO = 7,
  
        [Display(Name = "ارجاع به جمعدار")]
        [Description("ارجاع به جمعدار")]
        ReferredToCollector = 8,

        [Display(Name = "ارجاع به واحد فروش")]
        [Description("ارجاع به واحد فروش")]
        ReferredToSalesUnit = 9,

        [Display(Name = "ارجاع به بازرس کنترل کیفی")]
        [Description("ارجاع به بازرس کنترل کیفی")]
        ReferredToQCInspector = 10,

        [Description("پایان عملیات")]
        [Display(Name = "پایان عملیات")]
        ProcessCompleted = 11,

        [Description("ارجاع به سایرین جهت علت یابی")]
        [Display(Name = "ارجاع به سایرین جهت علت یابی")]
        ReferredToOthersForCausation = 12,

        [Description("ارجاع شده به تضمین کیفیت")]
        [Display(Name = "ارجاع شده به تضمین کیفیت")]
        ReferredToQA = 13,

        [Description("ارجاع به مدیر بهره برداری")]
        [Display(Name = "ارجاع به مدیر بهره برداری")]
        RefferedToOperationManager = 14,
    }
}
