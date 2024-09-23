using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.HR.Module.TicketRegister.Entities
{

    [Table(nameof(Area)+"s", Schema = "Ticket")]
    public class Area :EntityBase
    {
        public int AreaId { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged();
            }
        }

        private int _capacity;
        public int Capacity
        {
            get { return _capacity; }
            set
            {
                if (_capacity == value) return;
                _capacity = value;
                OnPropertyChanged();
            }
        }
        public virtual IList<AreaRow>? AreaRows { get; set; }
    }
}
