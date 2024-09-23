using System.ComponentModel.DataAnnotations;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.TicketRegister.Entities;

namespace Teram.HR.Module.TicketRegister.Models
{
    public class SeatModel :ModelBase<Seat,int>
    {
        public int SeatId { get; set; }

        [ConcurrencyCheck]
        public byte[] RowVersion {  get; set; }
        public int SeatNumber { get; set; }

        public bool IsReserved {  get; set; }

        public Guid? ReservedBy {  get; set; }

        public DateTime? ReservationDate {  get; set; }

        public string? ReservedFor {  get; set; }
        public int AreaRowId { get; set; }
    }
}
