using Teram.Framework.Core.Logic;
using Teram.HR.Module.TicketRegister.Entities;
using Teram.HR.Module.TicketRegister.Models;

namespace Teram.HR.Module.TicketRegister.Logic.Interfaces
{
    public interface IAreaLogic : IBusinessOperations<AreaModel, Area, int>
    {
        BusinessOperationResult<AreaModel> GetByAreaId(int areaId); 
    }

}
