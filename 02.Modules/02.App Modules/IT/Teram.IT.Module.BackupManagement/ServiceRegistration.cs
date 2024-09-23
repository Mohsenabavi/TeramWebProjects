

using Teram.Framework.Core.Logic;
using Teram.IT.Module.BackupManagement.Jobs;
using Teram.IT.Module.BackupManagement.Logic;
using Teram.IT.Module.BackupManagement.Logic.Interfaces;
using Teram.IT.Module.BackupManagement.Models;
using Teram.IT.Module.BackupManagement.Services;

namespace Teram.IT.Module.BackupManagement
{
    public class ServiceRegistration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IApplicationLogic, ApplicationLogic>();
            services.AddScoped<ILogic<ApplicationModel>, ApplicationLogic>();

            services.AddScoped<IServerPathLogic, ServerPathLogic>();
            services.AddScoped<ILogic<ServerPathModel>, ServerPathLogic>();


            services.AddScoped<IBackupHistoryLogic, BackupHistoryLogic>();
            services.AddScoped<ILogic<BackupHistoryModel>, BackupHistoryLogic>();

            services.AddScoped<IJobRunHistoryLogic, JobRunHistoryLogic>();
            services.AddScoped<ILogic<JobRunHistoryModel>, JobRunHistoryLogic>();

            services.AddScoped<BackupOperation>();

            services.AddScoped<IIOService, IOService>();
        }
        public static void JobRegister(IServiceProvider provider)
        {
            var runBackupOperation = provider.GetService<BackupOperation>();
            if (runBackupOperation != null)
            {
                runBackupOperation.Initilize();
            }
        }
    }
}
