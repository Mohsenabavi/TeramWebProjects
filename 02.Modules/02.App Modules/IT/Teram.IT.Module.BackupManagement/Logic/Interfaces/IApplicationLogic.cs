using Teram.Framework.Core.Logic;
using Teram.IT.Module.BackupManagement.Entities;
using Teram.IT.Module.BackupManagement.Models;

namespace Teram.IT.Module.BackupManagement.Logic.Interfaces
{
  
    public interface IApplicationLogic : IBusinessOperations<ApplicationModel, Application, int>
    {
        BusinessOperationResult<List<ApplicationModel>> GetActives();
    }

}
