using Teram.Framework.Core.Logic;
using Teram.HR.Module.TicketRegister.Entities;
using Teram.HR.Module.TicketRegister.Models;

namespace Teram.HR.Module.TicketRegister.Logic.Interfaces
{
    public interface ISeatLogic : IBusinessOperations<SeatModel, Seat, int>
    {
        BusinessOperationResult<List<SeatModel>> CreateSeats(int AreaId);

        BusinessOperationResult<List<SeatModel>> GetAreaSeats(int id);
    }

}
