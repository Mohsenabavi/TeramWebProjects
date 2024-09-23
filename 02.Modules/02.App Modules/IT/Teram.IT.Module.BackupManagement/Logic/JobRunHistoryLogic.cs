using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.IT.Module.BackupManagement.Entities;
using Teram.IT.Module.BackupManagement.Logic.Interfaces;
using Teram.IT.Module.BackupManagement.Models;

namespace Teram.IT.Module.BackupManagement.Logic
{
  
    public class JobRunHistoryLogic : BusinessOperations<JobRunHistoryModel, JobRunHistory, int>, IJobRunHistoryLogic
    {
        public JobRunHistoryLogic(IPersistenceService<JobRunHistory> service) : base(service)
        {

        }

        public BusinessOperationResult<JobRunHistoryModel> CreateJobRunHistory(JobRunHistoryModel jobRunHistoryModel)
        {
            return AddNew(jobRunHistoryModel);
        }
    }

}
