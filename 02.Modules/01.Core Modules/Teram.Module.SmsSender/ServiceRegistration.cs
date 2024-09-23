using Teram.Module.SmsSender.Logic;
using Teram.Module.SmsSender.Logic.Interfaces;
using Teram.Module.SmsSender.Services;
using Teram.Module.SmsSender.Services.AsiaSms;

namespace Teram.Module.SmsSender
{
    public class ServiceRegistration
    {
        public static void Register(IServiceCollection services)
        {
            #region Services

            services.AddScoped<ISendAsiaSMSService, SendAsiaSMSService>();

            #endregion

            #region Logics

            services.AddScoped<ISMSHistoryLogic, SMSHistoryLogic>();            
            services.AddScoped<ISMSTemplateLogic, SMSTemplateLogic>();

            #endregion

        }
    }
}
