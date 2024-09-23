using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Teram.HR.Module.Recruitment.Enums
{
    public enum ProcessStatus
    {
        [Description("ثبت اولیه")]
        [Display(Name = "ثبت اولیه")]
        NoAction =0,

        [Description("اتمام فرآیند پیشنیه شغلی")]
        [Display(Name = "اتمام فرآیند پیشنیه شغلی")]
        ApproveJobBackground = 1,

        [Description("ثبت اطلاعات پایه")]
        [Display(Name = "ثبت اطلاعات پایه")]
        BaseInformationAdded =2,      

        [Description("بارگذاری مدارک")]
        [Display(Name = "بارگذاری مدارک")]
        DoumentsUploaded =3,
             
        [Description("تایید اولیه")]
        [Display(Name = "تایید اولیه")]
        FirstApprove = 4,  

        [Description(" اقرار انجام آزمایشات و دریافت گواهی عدم سوء پیشینه")]
        [Display(Name = "قرار انجام آزمایشات و دریافت گواهی عدم سوء پیشینه")]
        AdmittingToDoExpriments = 5,

        [Description("تایید طب کار")]
        [Display(Name = "تایید طب کار")]
        ApproveHSE = 6,

        [Description("تایید نهایی توسط منابع انسانی")]
        [Display(Name = "تایید نهایی توسط منابع انسانی")]
        FinalApproveByHR = 7,
    }
}