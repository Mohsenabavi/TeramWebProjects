using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Module.SmsSender.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.Module.SmsSender.Models
{
    public class SMSHistoryModel:ModelBase<SMSHistory,int>
    {
        public int SMSHistoryId { get; set; }

        [GridColumn(nameof(SenderId))]
        public string SenderId {  get; set; }

        [GridColumn(nameof(RecieverNumber))]
        public string RecieverNumber { get; set; }
        public DateTime SendDate { get; set; }

        [GridColumn(nameof(SendDatePersian))]
        public string SendDatePersian => SendDate.ToPersianDateTime();

        [GridColumn(nameof(MessageContext))]
        public string MessageContext {  get; set; }
        public Guid BatchId {  get; set; }
        public bool IsSuccessful { get; set; }

        [GridColumn(nameof(IsSuccessfulText))]
        public string IsSuccessfulText => IsSuccessful ? "ارسال موفق" : "ارسال ناموفق";
        public int StatusCode {  get; set; }
        public string Message { get; set; }
    }
}
