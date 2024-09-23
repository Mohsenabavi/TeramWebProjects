using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.HR.Module.TicketRegister.Entities
{

    [Table(nameof(Seat)+"s", Schema = "Ticket")]
    public class Seat : EntityBase
    {
        public int SeatId { get; set; }

        private byte[] _rowVersion;

        [ConcurrencyCheck]
        public byte[] RowVersion
        {
            get { return _rowVersion; }
            set
            {
                if (_rowVersion == value) return;
                _rowVersion = value;
                OnPropertyChanged();
            }
        }        

        private int _seatNumber;
        public int SeatNumber
        {
            get { return _seatNumber; }
            set
            {
                if (_seatNumber == value) return;
                _seatNumber = value;
                OnPropertyChanged();
            }
        }

        private bool _isReserved;
        public bool IsReserved
        {
            get { return _isReserved; }
            set
            {
                if (_isReserved == value) return;
                _isReserved = value;
                OnPropertyChanged();
            }
        }

        private Guid? _reservedBy;
        public Guid? ReservedBy
        {
            get { return _reservedBy; }
            set
            {
                if (_reservedBy == value) return;
                _reservedBy = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _reservationDate;
        public DateTime? ReservationDate
        {
            get { return _reservationDate; }
            set
            {
                if (_reservationDate == value) return;
                _reservationDate = value;
                OnPropertyChanged();
            }
        }

        private string? _reservedFor;
        public string? ReservedFor
        {
            get { return _reservedFor; }
            set
            {
                if (_reservedFor == value) return;
                _reservedFor = value;
                OnPropertyChanged();
            }
        }

        private int _areaRowId;
        public int AreaRowId
        {
            get { return _areaRowId; }
            set
            {
                if (_areaRowId == value) return;
                _areaRowId = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(AreaRowId))]
        public virtual AreaRow? AreaRow { get; set; }
    }
}
