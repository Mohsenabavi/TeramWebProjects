using System.ComponentModel;

namespace Teram.HR.Module.Recruitment.Enums
{
    public enum ApproveStatus
    {
        [Description("------")]
        NoAction = 0,

        [Description("تایید شده")]
        FisrtApproved = 1,
      
        [Description("تایید تکمیل مدارک")]
        DocumentsApprove =2,

        [Description("دعوت به کار شده")]
        InvitedToWork = 3,

        [Description("تایید کارگزینی")]
        FinalApprove = 4,

        [Description("در حال بررسی")]
        Disapproved = 5        
    }
}
