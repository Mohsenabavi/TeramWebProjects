using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.OC.Logic.Interface;
using Teram.HR.Module.OC.Models;

namespace Teram.HR.Module.OC.Logic
{ 
    public class PositionLogic : BusinessOperations<PositionModel, Entities.Position, int>, IPositionLogic
    {
        public PositionLogic(IPersistenceService<Entities.Position> service) : base(service)
        {

        }
    }

}
