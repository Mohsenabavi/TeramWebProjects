using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.TicketRegister.Entities;
using Teram.HR.Module.TicketRegister.Logic.Interfaces;
using Teram.HR.Module.TicketRegister.Models;

namespace Teram.HR.Module.TicketRegister.Logic
{

    public class AreaLogic : BusinessOperations<AreaModel, Area, int>, IAreaLogic
    {
        public AreaLogic(IPersistenceService<Area> service) : base(service)
        {

        }

        public BusinessOperationResult<AreaModel> GetByAreaId(int areaId)
        {
            return GetFirst<AreaModel>(x => x.AreaId == areaId);
        }
    }

}
