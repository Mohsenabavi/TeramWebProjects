using Teram.Framework.Core.Logic;
using Teram.IT.Module.BackupManagement.Models;

namespace Teram.IT.Module.BackupManagement.Services
{
    public interface IIOService
    {
        BusinessOperationResult<BackupResultModel> CopyFiles(string ApplicationTitle, string sourcePath, string destinationPath, string? fileName = null);
        BusinessOperationResult<BackupResultModel> Esko_Backup();
        BusinessOperationResult<BackupResultModel> EdariWeb_Backup();
        BusinessOperationResult<BackupResultModel> MizitoBackup();

    }
}
