using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.HR.Module.OC.Entities
{

    [Table(nameof(OrganizationChart), Schema = "OC")]
    public class OrganizationChart : EntityBase
    {
        public int OrganizationChartId { get; set; }

        private int? _parentOrganizationChartId;
        public int? ParentOrganizationChartId
        {
            get { return _parentOrganizationChartId; }
            set
            {
                if (_parentOrganizationChartId == value) return;
                _parentOrganizationChartId = value;
                OnPropertyChanged();
            }
        }

        private string _personnelCode;
        public string PersonnelCode
        {
            get { return _personnelCode; }
            set
            {
                if (_personnelCode == value) return;
                _personnelCode = value;
                OnPropertyChanged();
            }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName == value) return;
                _firstName = value;
                OnPropertyChanged();
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName == value) return;
                _lastName = value;
                OnPropertyChanged();
            }
        }

        private int _positionId;
        public int PositionId
        {
            get { return _positionId; }
            set
            {
                if (_positionId == value) return;
                _positionId = value;
                OnPropertyChanged();
            }
        }


        private Guid _userId;
        public Guid UserId
        {
            get { return _userId; }
            set
            {
                if (_userId == value) return;
                _userId = value;
                OnPropertyChanged();
            }
        }


        [ForeignKey(nameof(PositionId))]

        public virtual Position Position { get; set; }


        [ForeignKey(nameof(ParentOrganizationChartId))]
        public virtual OrganizationChart Parent { get; set; }
        public virtual ICollection<OrganizationChart> Children { get; set; }

    }
}
