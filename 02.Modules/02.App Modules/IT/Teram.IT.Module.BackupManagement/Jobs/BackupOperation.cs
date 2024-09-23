using DocumentFormat.OpenXml.Bibliography;
using Hangfire;
using Renci.SshNet;
using Teram.Framework.Core.Extensions;
using Teram.IT.Module.BackupManagement.Logic.Interfaces;
using Teram.IT.Module.BackupManagement.Models;
using Teram.IT.Module.BackupManagement.Services;
using Teram.Module.EmailSender.Services.Interfaces;

namespace Teram.IT.Module.BackupManagement.Jobs
{
    public class BackupOperation
    {
        private readonly ILogger<BackupOperation> logger;
        private readonly IIOService service;
        private readonly IApplicationLogic applicationLogic;
        private readonly IBackupHistoryLogic backupHistoryLogic;
        private readonly IJobRunHistoryLogic jobRunHistoryLogic;
        private readonly IEmailService emailService;
        private readonly IRecurringJobManager recurringJobManager;

        public BackupOperation(ILogger<BackupOperation> logger, IIOService service,
            IApplicationLogic applicationLogic, IBackupHistoryLogic backupHistoryLogic,
            IJobRunHistoryLogic jobRunHistoryLogic, IEmailService emailService,
            IRecurringJobManager recurringJobManager)
        {
            this.logger=logger??throw new ArgumentNullException(nameof(logger));
            this.service=service??throw new ArgumentNullException(nameof(service));
            this.applicationLogic=applicationLogic??throw new ArgumentNullException(nameof(applicationLogic));
            this.backupHistoryLogic=backupHistoryLogic??throw new ArgumentNullException(nameof(backupHistoryLogic));
            this.jobRunHistoryLogic=jobRunHistoryLogic??throw new ArgumentNullException(nameof(jobRunHistoryLogic));
            this.emailService=emailService??throw new ArgumentNullException(nameof(emailService));
            this.recurringJobManager=recurringJobManager??throw new ArgumentNullException(nameof(recurringJobManager));
        }
        public void Initilize()
        {
            recurringJobManager.AddOrUpdate("RunBackupOperation", () => RunBackupOperation(), Cron.Daily(20, 00), TimeZoneInfo.Local);
        }

        public void RunBackupOperation()
        {
            try
            {
                string emailContext = string.Empty;

                var jobRunHistory = new JobRunHistoryModel
                {
                    IsSucess=true,
                    Message="Job Ran Successfully",
                    RunDate=DateTime.Now,
                    Title="Backup Operation",
                };

                var applicationsResult = applicationLogic.GetActives();
                emailContext+="<b>" + "Backup copy report on server 210" + "</b>" + "<br/><hr/>";

                foreach (var application in applicationsResult.ResultEntity)
                {

                    foreach (var serverPath in application.ServerPaths)
                    {

                        if (!string.IsNullOrEmpty(serverPath.SourcePath) && !string.IsNullOrEmpty(serverPath.DestinationPath))
                        {
                            var fullSourcePath = $"{application.SourcePath}{serverPath.SourcePath}";
                            var fullDestinationPath = $"{application.DestinationPath}{serverPath.DestinationPath}";
                            var result = service.CopyFiles(application.Title, fullSourcePath, fullDestinationPath, serverPath.FileName);

                            var historyModel = new BackupHistoryModel
                            {
                                ApplicationTitle = application.Title,
                                BackupDate=DateTime.Now,
                                DestinationPath = fullDestinationPath,
                                IsSuccess=result.ResultEntity.IsSuccess,
                                SourcePath = fullSourcePath,
                                Message=string.Join("-", result.Messages)
                            };
                            backupHistoryLogic.CreateHistory(historyModel);
                            emailContext+=$"{result.ResultEntity.EmailContext}<br/>";
                        }
                    }
                }

                var mizitoResult = service.MizitoBackup();

                emailContext+=mizitoResult.ResultEntity.EmailContext;

                var eskoResult = service.Esko_Backup();

                emailContext+=eskoResult.ResultEntity.EmailContext;


                var edariWebResult = service.EdariWeb_Backup();

                emailContext+=edariWebResult.ResultEntity.EmailContext;

                jobRunHistory.RunFinishDate = DateTime.Now;
                jobRunHistoryLogic.CreateJobRunHistory(jobRunHistory);
                emailService.SendEmail($"Backup Report - {DateTime.Now.ToPersianDate()}", emailContext, DateTime.Now, "Mohsen.Nabavi@teram-group.com");
                emailService.SendEmail($"Backup Report - {DateTime.Now.ToPersianDate()}", emailContext, DateTime.Now, "ict@teram-group.com");
            }
            catch (Exception ex)
            {
                var jobRunHistory = new JobRunHistoryModel
                {
                    IsSucess=false,
                    Message=string.Join("-", ex.Message, ex.InnerException),
                    RunDate=DateTime.Now,
                    Title="Backup Operation",
                };
                jobRunHistoryLogic.CreateJobRunHistory(jobRunHistory);
                logger.LogError(1007, ex, $"Exception Error in Job Update Assets Info Service : {Environment.NewLine} {ex.Message} ");
            }
        }
    }
}
