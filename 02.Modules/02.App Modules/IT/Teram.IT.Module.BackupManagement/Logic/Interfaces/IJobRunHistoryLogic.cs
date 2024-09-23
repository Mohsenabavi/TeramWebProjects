using Teram.Framework.Core.Logic;
using Teram.IT.Module.BackupManagement.Entities;
using Teram.IT.Module.BackupManagement.Models;

namespace Teram.IT.Module.BackupManagement.Logic.Interfaces
{ 
    public interface IJobRunHistoryLogic : IBusinessOperations<JobRunHistoryModel, JobRunHistory, int>
    {
        BusinessOperationResult<JobRunHistoryModel> CreateJobRunHistory(JobRunHistoryModel jobRunHistoryModel);
    }

}
