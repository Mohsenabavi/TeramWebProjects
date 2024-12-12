using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Teram.QC.Module.FinalProduct.Enums
{
    public enum FormStatus
    {
        [Display(Name = "تعیین تکلیف توسط مدیر کنترل کیفی")]
        [Description("تعیین تکلیف توسط مدیر کنترل کیفی")]
        InitialRegistration = 0,

        [Display(Name = "تایید اولیه مدیر کنترل کیفی")]
        [Description("تایید اولیه مدیر کنترل کیفی")]
        QCManagerApproved = 1,

        [Display(Name = "باطل شده توسط مدیر کنترل کیفیت")]
        [Description("باطل شده توسط مدیر کنترل کیفیت")]
        QcManagerVoided = 2,

        [Display(Name = "اصلاح اطلاعات اولیه")]
        [Description("اصلاح اطلاعات اولیه")]
        QCManagerModifyOrder = 3,

        [Display(Name = "تعیین تکلیف توسط مدیر کنترل کیفی پس از اصلاح")]
        [Description("تعیین تکلیف توسط مدیر کنترل کیفی پس از اصلاح")]
        ModifiedByQCSupervisor = 4,

        [Display(Name = "ثبت نظر واحد فروش")]
        [Description("ثبت نظر واحد فروش")]
        SalesUnitOpinion = 5,

        [Display(Name = "تعیین تکلیف اولیه توسط مدیرعامل")]
        [Description("تعیین تکلیف اولیه توسط مدیرعامل")]
        CEOFirstOpinion = 6,

        [Display(Name = "انجام عملیات جداسازی")]
        [Description("انجام عملیات جداسازی")]
        Seperation = 7,

        [Display(Name = "تعیین تکلیف نهایی توسط مدیرعامل")]
        [Description("تعیین تکلیف نهایی توسط مدیرعامل")]
        CEOLastOpinion = 8,

        [Display(Name = "ثبت نظر بررسی کنندگان")]
        [Description("ثبت نظر بررسی کنندگان")]
        OthersOpinion = 9,

        [Display(Name = "انجام عملیات ضایعات محصول")]
        [Description("انجام عملیات ضایعات محصول")]
        WasteOperation = 10,

        [Display(Name = "درخواست مجدد تعیین عامل عدم انطباق")]
        [Description("درخواست مجدد تعیین عامل عدم انطباق")]
        RequestForDeterminingReason = 11,

        [Display(Name = "درخواست بازنگری توسط مدیر کنترل کیفی")]
        [Description("درخواست بازنگری توسط مدیر کنترل کیفی")]
        RequestForReviewByQCManager = 12,

        [Display(Name = "درخواست بررسی توسط اشخاص دیگر")]
        [Description("درخواست بررسی توسط اشخاص دیگر")]
        RequestForReviewByOthers = 13,

        [Description("تعیین عامل عدم انطباق")]
        [Display(Name = "تعیین عامل عدم انطباق")]
        DeterminingReason = 14,

        [Description("پایان یافته و بایگانی شده")]
        [Display(Name = "پایان یافته و بایگانی شده")]
        ProcessCompleted = 15,

        [Description("ارجاع به سایر مدیران جهت علت یابی")]
        [Display(Name = "ارجاع به سایر مدیران جهت علت یابی")]
        RequestForCausationbyOtherManagers = 16,

        [Description("ارجاع به سرپرست تضمین کیفیت")]
        [Display(Name = "ارجاع به سرپرست تضمین کیفیت")]
        RefferedToQA = 17,

        [Description("ارجاع به مدیر بهره برداری")]
        [Display(Name = "ارجاع به مدیر بهره برداری")]
        RefferedToOperationManager = 18,
    }
}
