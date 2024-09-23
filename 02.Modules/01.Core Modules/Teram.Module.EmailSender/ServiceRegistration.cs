
using Teram.Module.EmailSender.Services;
using Teram.Module.EmailSender.Services.Interfaces;

namespace Teram.Module.EmailSender

{
    public class ServiceRegistration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
