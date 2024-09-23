using Teram.Module.AttachmentsManagement.Logic;
using Teram.Module.AttachmentsManagement.Logic.Interfaces;

namespace Teram.Module.AttachmentsManagement
{
    public class ServiceRegistration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IAttachmentLogic, AttachmentLogic>();
        }
    }
}
