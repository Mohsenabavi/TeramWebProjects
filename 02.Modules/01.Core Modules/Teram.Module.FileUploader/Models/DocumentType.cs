using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Teram.HR.Module.FileUploader.Models
{
    public enum DocumentType
    {
        [Description("روی کارت ملی")]
        [Display(Name = "روی کارت ملی")]
        OnNationalCard = 1,

        [Description("پشت کارت ملی")]
        [Display(Name = "پشت کارت ملی")]
        BehindNationalCard = 2,

        [Description("صفحه اول شناسنامه")]
        [Display(Name = "صفحه اول شناسنامه")]
        BirthCertificateFirstPage = 3,

        [Description("صفحه دوم شناسنامه")]
        [Display(Name = "صفحه دوم شناسنامه")]
        BirthCertificateSecondPage = 4,

        [Description("صفحه سوم شناسنامه")]
        [Display(Name = "صفحه سوم شناسنامه")]
        BirthCertificateThirdPage = 5,

        [Description("صفحه چهارم شناسنامه")]
        [Display(Name = "صفحه چهارم شناسنامه")]
        BirthCertificateForthPage = 6,

        [Description("پایان خدمت")]
        [Display(Name = "پایان خدمت")]
        MilitaryService = 7,

        [Description("عکس پرسنلی")]
        [Display(Name = "عکس پرسنلی")]
        PersonalImage = 8,

        [Description("مدرک تحصیلی")]
        [Display(Name = "مدرک تحصیلی")]
        EducationCertificate = 9,

        [Description("سوابق بیمه")]
        [Display(Name = "سوابق بیمه")]
        InsuranceCard = 10,

        [Description("رزومه کاری")]
        [Display(Name = "رزومه کاری")]
        Resume = 11,

        [Description("ارجاعیه")]
        [Display(Name = "ارجاعیه")]
        Referral = 12,

        [Description("خلاصه پرونده")]
        [Display(Name = "خلاصه پرونده")]
        FileSummary = 13,


        [Description("کنترل اقلام ورودی")]
        [Display(Name = "کنترل اقلام ورودی")]
        IncomingGoods1 = 14,

        [Description("کنترل اقلام ورودی")]
        [Display(Name = "کنترل اقلام ورودی")]
        IncomingGoods2 = 15,

        [Description("کنترل اقلام ورودی")]
        [Display(Name = "کنترل اقلام ورودی")]
        IncomingGoods3 = 16,

        [Description("کنترل اقلام ورودی")]
        [Display(Name = "کنترل اقلام ورودی")]
        IncomingGoods4 = 17,

        [Description("سفته")]
        [Display(Name = "سفته")]
        PromissoryNote = 18,


        [Description("گواهی عدم سوء پیشینه")]
        [Display(Name = "گواهی عدم سوء پیشینه")]
        NoBadBackground = 19,

        [Description("فرم ارزیابی مصاحبه")]
        [Display(Name = "فرم ارزیابی مصاحبه")]
        InterviewEvaluation = 20,

        [Description("روی پرسش ‌نامه شخصیت آیزنک")]
        [Display(Name = "روی پرسش ‌نامه شخصیت آیزنک")]
        EysenckFormFront = 21,


        [Description("گواهی عدم سوء مصرف مواد")]
        [Display(Name = "گواهی عدم سوء مصرف مواد")]
        NoAddictionForm = 22,


        [Description("صفحه اول شناسنامه همسر")]
        [Display(Name = "صفحه اول شناسنامه همسر")]
        PartnerBirthCertFirstPage = 23,

        [Description("صفحه دوم شناسنامه همسر")]
        [Display(Name = "صفحه دوم شناسنامه همسر")]
        PartnerBirthCertSecondPage = 24,

        [Description("کارت ملی همسر")]
        [Display(Name = "کارت ملی همسر")]
        PartnerMelliCard = 25,

        [Description("شناسنامه فرزند اول")]
        [Display(Name = "شناسنامه فرزند اول")]
        FirstChildBirthCert = 26,

        [Description("شناسنامه فرزند دوم")]
        [Display(Name = "شناسنامه فرزند دوم")]
        SecondChildBirthCert = 27,

        [Description("شناسنامه فرزند سوم")]
        [Display(Name = "شناسنامه فرزند سوم")]
        ThirdChildBirthCert = 28,

        [Description("شناسنامه فرزند چهارم")]
        [Display(Name = "شناسنامه فرزند چهارم")]
        ForthChildBirthCert = 29,

        [Description("شناسنامه فرزند پنجم")]
        [Display(Name = "شناسنامه فرزند پنجم")]
        FifthChildBirthCert = 30,


        [Description("شناسنامه فرزند ششم")]
        [Display(Name = "شناسنامه فرزند ششم")]
        SixthChildBirthCert = 31,

        [Description("پشت پرسش ‌نامه شخصیت آیزنک")]
        [Display(Name = "پشت پرسش ‌نامه شخصیت آیزنک")]
        EysenckFormBehind = 32,

        [Description("روی پرسشنامه استخدامی")]
        [Display(Name = "روی پرسشنامه استخدامی")]
        OnEmploymentQuestionnaire = 33,


        [Description("پشت پرسشنامه استخدامی")]
        [Display(Name = "پشت پرسشنامه استخدامی")]
        BehindEmploymentQuestionnaire = 34,

        [Description("کنترل محصول نهایی")]
        [Display(Name = "کنترل محصول نهایی")]
        FinalGoods1 = 35,

        [Description("کنترل محصول نهایی")]
        [Display(Name = "کنترل محصول نهایی")]
        FinalGoods2 = 36,

        [Description("کنترل محصول نهایی")]
        [Display(Name = "کنترل محصول نهایی")]
        FinalGoods3 = 37,

        [Description("کنترل محصول نهایی")]
        [Display(Name = "کنترل محصول نهایی")]
        FinalGoods4 = 38,

        [Description("ضمیمه اول تحقیقات پیشینه")]
        [Display(Name = "ضمیمه اول تحقیقات پیشینه")]
        BackgroundAttchament1 = 39,

        [Description("ضمیمه دوم تحقیقات پیشینه")]
        [Display(Name = "ضمیمه دوم تحقیقات پیشینه")]
        BackgroundAttchament2 = 40,
    }
}

