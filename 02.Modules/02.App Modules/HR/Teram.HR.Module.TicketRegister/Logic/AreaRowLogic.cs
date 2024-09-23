using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.TicketRegister.Entities;
using Teram.HR.Module.TicketRegister.Logic.Interfaces;
using Teram.HR.Module.TicketRegister.Models;

namespace Teram.HR.Module.TicketRegister.Logic
{
    public class AreaRowLogic : BusinessOperations<AreaRowModel, AreaRow, int>, IAreaRowLogic
    {
        public AreaRowLogic(IPersistenceService<AreaRow> service) : base(service)
        {

        }
    }

}
