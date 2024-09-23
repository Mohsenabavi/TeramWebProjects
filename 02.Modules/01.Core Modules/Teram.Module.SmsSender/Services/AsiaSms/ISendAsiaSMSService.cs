using Teram.Module.SmsSender.Models.AsiaSms;

namespace Teram.Module.SmsSender.Services
{
    public interface ISendAsiaSMSService
    {
        SendSmsResultModel SendMessage(SendSmsModel smsModel);
    }
}
