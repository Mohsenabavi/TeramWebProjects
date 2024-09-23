using Teram.Framework.Core.Logic;
using Teram.HR.Module.TicketRegister.Entities;

namespace Teram.HR.Module.TicketRegister.Models
{
    public class AreaRowModel : ModelBase<AreaRow, int>
    {
        public int AreaRowId { get; set; }

        public string RowNumber { get; set; }

        public int SeatCount { get; set; }

        public int AreaId { get; set; }

        public List<SeatModel>? Seats { get; set; }

    }
}
