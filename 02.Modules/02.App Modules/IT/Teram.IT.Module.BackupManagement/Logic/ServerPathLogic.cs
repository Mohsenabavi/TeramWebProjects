using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.IT.Module.BackupManagement.Entities;
using Teram.IT.Module.BackupManagement.Logic.Interfaces;
using Teram.IT.Module.BackupManagement.Models;

namespace Teram.IT.Module.BackupManagement.Logic
{
 
    public class ServerPathLogic : BusinessOperations<ServerPathModel, ServerPath, int>, IServerPathLogic
    {
        public ServerPathLogic(IPersistenceService<ServerPath> service) : base(service)
        {

        }
    }

}
