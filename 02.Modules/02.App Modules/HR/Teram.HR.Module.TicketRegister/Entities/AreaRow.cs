using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.HR.Module.TicketRegister.Entities
{

    [Table(nameof(AreaRow)+"s", Schema = "Ticket")]
    public class AreaRow : EntityBase
    {
        public int AreaRowId { get; set; }


        private string _rowNumber;
        public string RowNumber
        {
            get { return _rowNumber; }
            set
            {
                if (_rowNumber == value) return;
                _rowNumber = value;
                OnPropertyChanged();
            }
        }

        private int _seatCount;
        public int SeatCount
        {
            get { return _seatCount; }
            set
            {
                if (_seatCount == value) return;
                _seatCount = value;
                OnPropertyChanged();
            }
        }


        private int _areaId;
        public int AreaId
        {
            get { return _areaId; }
            set
            {
                if (_areaId == value) return;
                _areaId = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(AreaId))]
        public virtual Area Area { get; set; }

        public virtual IList<Seat>? Seats { get; set; }
    }
}
