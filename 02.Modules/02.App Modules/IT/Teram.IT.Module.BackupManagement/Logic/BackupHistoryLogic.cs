using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.IT.Module.BackupManagement.Entities;
using Teram.IT.Module.BackupManagement.Logic.Interfaces;
using Teram.IT.Module.BackupManagement.Models;

namespace Teram.IT.Module.BackupManagement.Logic
{

    public class BackupHistoryLogic : BusinessOperations<BackupHistoryModel, BackupHistory, int>, IBackupHistoryLogic
    {
        public BackupHistoryLogic(IPersistenceService<BackupHistory> service) : base(service)
        {

        }

        public BusinessOperationResult<BackupHistoryModel> CreateHistory(BackupHistoryModel backupHistory)
        {
            return AddNew(backupHistory);
        }
    }

}
