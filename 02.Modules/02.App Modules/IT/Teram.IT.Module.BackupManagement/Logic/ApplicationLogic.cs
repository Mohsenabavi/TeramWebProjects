using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.IT.Module.BackupManagement.Entities;
using Teram.IT.Module.BackupManagement.Logic.Interfaces;
using Teram.IT.Module.BackupManagement.Models;

namespace Teram.IT.Module.BackupManagement.Logic
{
    public class ApplicationLogic : BusinessOperations<ApplicationModel, Application, int>, IApplicationLogic
    {
        public ApplicationLogic(IPersistenceService<Application> service) : base(service)
        {

        }

        public BusinessOperationResult<List<ApplicationModel>> GetActives()
        {
            return GetData<ApplicationModel>(x => x.IsActive);
        }
    }

}
