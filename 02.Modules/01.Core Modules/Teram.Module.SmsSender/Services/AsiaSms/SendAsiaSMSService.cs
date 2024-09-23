using Newtonsoft.Json;
using Teram.Module.SmsSender.Entities;
using Teram.Module.SmsSender.Logic.Interfaces;
using Teram.Module.SmsSender.Models;
using Teram.Module.SmsSender.Models.AsiaSms;

namespace Teram.Module.SmsSender.Services.AsiaSms
{
    public class SendAsiaSMSService : ISendAsiaSMSService
    {
        private readonly IConfiguration configuration;
        private readonly ISMSHistoryLogic sMSHistoryLogic;

        public SendAsiaSMSService(IConfiguration configuration, ISMSHistoryLogic sMSHistoryLogic)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.sMSHistoryLogic=sMSHistoryLogic??throw new ArgumentNullException(nameof(sMSHistoryLogic));
        }

        public SendSmsResultModel SendMessage(SendSmsModel smsModel)
        {


            smsModel.SmsText = smsModel.SmsText+ "\n لغو 11";

            var asiaSmsSection = configuration.GetSection("AsisSmsSection").Get<AsisSmsSection>();

            string apiUrl = asiaSmsSection.ApiUrl;
            string userName = asiaSmsSection.UserName;
            string password = asiaSmsSection.Password;
            string senderId = asiaSmsSection.SenderId;

            using HttpClient client = new();
            var result = new SendSmsResultModel();
            var queryString = new System.Collections.Specialized.NameValueCollection
            {
                ["Username"] = userName,
                ["password"] = password,
                ["senderId"] = senderId,
                ["SmsText"] = smsModel.SmsText,
                ["Receivers"] = smsModel.Receivers
            };
            string fullUrl = $"{apiUrl}?{ToQueryString(queryString)}";
            HttpResponseMessage response = client.GetAsync(fullUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                string responseBody = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(responseBody))
                {
                    result = JsonConvert.DeserializeObject<SendSmsResultModel>(responseBody);
                    if (result != null && result.IsSuccessful)
                    {

                        var historyModel = new SMSHistoryModel
                        {
                            MessageContext=smsModel.SmsText,
                            RecieverNumber=smsModel.Receivers,
                            SendDate=DateTime.Now,
                            SenderId=senderId,
                            BatchId=result.BatchId,
                            IsSuccessful=result.IsSuccessful,
                            Message=(string.IsNullOrEmpty(result.Message) ? "-" : result.Message),
                            StatusCode = result.StatusCode
                        };

                        var logHistoryResult = sMSHistoryLogic.AddNew(historyModel);
                    }
                    return result;
                }
                return result;
            }
            else
            {
                return new SendSmsResultModel
                {
                    BatchId = Guid.Empty,
                    IsSuccessful = false,
                    Message = "Error Occured",
                    StatusCode = 0
                };
            }
        }
        private static string ToQueryString(System.Collections.Specialized.NameValueCollection nvc)
        {
            return string.Join("&", Array.ConvertAll(nvc.AllKeys, key => $"{key}={nvc[key]}"));
        }
    }
}
