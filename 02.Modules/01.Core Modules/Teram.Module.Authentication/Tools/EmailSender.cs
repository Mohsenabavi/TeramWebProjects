using Teram.Framework.Core.Providers.EmailHistories;
using Teram.Framework.Core.Tools;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Teram.Module.Authentication.Tools
{
    public class EmailSender : MailService , ITeramEmailSender
    {
        private readonly string from;

        public EmailSender(IConfiguration configuration, ILogger<MailService> mailLogger, IHistoryService historyService)
        {
            if (mailLogger is null)
            {
                throw new ArgumentNullException("LOGER Error : " + nameof(mailLogger));
            }

            var identityConfig = configuration.GetSection("MailServer").GetSection("IdentityConfig");
            Host = identityConfig.GetValue<string>("host");
            Port = identityConfig.GetValue<int>("port");
            UserName = identityConfig.GetValue<string>("username");
            Password = identityConfig.GetValue<string>("password");
            IsSslRequired = identityConfig.GetValue<bool>("ssl");
            from = identityConfig.GetValue<string>("from");
            Logger = mailLogger;
            this.historyService = historyService ?? throw new ArgumentNullException(nameof(historyService));
        }
        public EmailSender(IConfiguration configuration, string configName, ILogger<MailService> mailLogger, IHistoryService historyService)
        {
            if (mailLogger is null)
            {
                throw new ArgumentNullException("LOGER Error : " + nameof(mailLogger));
            }

            var identityConfig = configuration.GetSection("MailServer").GetSection(configName);
            Host = identityConfig.GetValue<string>("host");
            Port = identityConfig.GetValue<int>("port");
            UserName = identityConfig.GetValue<string>("username");
            Password = identityConfig.GetValue<string>("password");
            IsSslRequired = identityConfig.GetValue<bool>("ssl");
            from = identityConfig.GetValue<string>("from");
            Logger = mailLogger;
            this.historyService = historyService;
        }
        public EmailSender(string host, string username, string password, int port, bool ssl, string from, ILogger<MailService> logger, IHistoryService historyService) :
            base(host, username, password, logger, port, ssl,historyService)
        {
            this.from = from;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage,string emailType)
        {
#if DEBUG
            email = "ghorbani.s@Teramgrouup.com";
#endif
             
            var task = Task.Run(() =>
              {
                    BackgroundJob.Enqueue(() => SendMailAsync(email, subject, from, from, htmlMessage,emailType));                
              });
            return task;
        }
         
    }
}
