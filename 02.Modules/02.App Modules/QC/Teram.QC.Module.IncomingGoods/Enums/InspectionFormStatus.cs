using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Teram.QC.Module.IncomingGoods.Enums
{
    public enum InspectionFormStatus
    {
        [Description("بدون وضعیت")]
        [Display(Name = "بدون وضعیت")]
        None = 0,

        [Description("ارجاع به سرپرست تولید")]
        [Display(Name = "ارجاع به سرپرست تولید")]
        ReferralToSupervisor = 1,

        [Description("ارجاع به مدیر تولید")]
        [Display(Name = "ارجاع به مدیر تولید")]
        ReferralToProductionManager = 2,

        [Description("ارجاع به مدیر کنترل کیفی")]
        [Display(Name = "ارجاع به مدیر کنترل کیفی")]
        ReferralToQCManager = 3,

        [Description("ارجاع به ارسال کننده")]
        [Display(Name = "ارجاع به ارسال کننده")]
        ReferralToCreator = 4,

        [Description("پایان عملیات")]
        [Display(Name = "پایان عملیات")]
        ProcessCompleted = 5,
    }
}
