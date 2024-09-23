using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.FinalProduct.Entities.Causation
{
    [Table(nameof(Operator) +"s", Schema = "QCFP")]
    public class Operator:EntityBase
    {
        public int OperatorId { get; set; }

        private string _firstName;

        [StringLength(100)]
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

        [StringLength(100)]
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

        private string? _personnelCode;

        [StringLength(20)]
        public string? PersonnelCode
        {
            get { return _personnelCode; }
            set
            {
                if (_personnelCode == value) return;
                _personnelCode = value;
                OnPropertyChanged();
            }
        }

        private string? _mobileNumber;
        public string? MobileNumber
        {
            get { return _mobileNumber; }
            set
            {
                if (_mobileNumber == value) return;
                _mobileNumber = value;
                OnPropertyChanged();
            }
        }

        private string? _remarks;
        public string? Remarks
        {
            get { return _remarks; }
            set
            {
                if (_remarks == value) return;
                _remarks = value;
                OnPropertyChanged();
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {              
                _isActive = value;
                OnPropertyChanged();
            }
        }

        private int _employeeID;
        public int EmployeeID
        {
            get { return _employeeID; }
            set
            {
                if (_employeeID == value) return;
                _employeeID = value;
                OnPropertyChanged();
            }
        }

        private Guid? _userId;
        public Guid? UserId
        {
            get { return _userId; }
            set
            {
                if (_userId == value) return;
                _userId = value;
                OnPropertyChanged();
            }
        }

        private string? _nationalID;
        public string? NationalID
        {
            get { return _nationalID; }
            set
            {
                if (_nationalID == value) return;
                _nationalID = value;
                OnPropertyChanged();
            }
        }
        public virtual ICollection<Causation>? Causations { get; set; }
    }
}
