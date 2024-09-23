using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.HR.Module.OC.Entities
{
    [Table(nameof(Position), Schema = "OC")]
    public class Position:EntityBase
    {
        public int PositionId { get; set; }

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
        public virtual ICollection<OrganizationChart> OrganizationCharts { get; set; }
    }
}
