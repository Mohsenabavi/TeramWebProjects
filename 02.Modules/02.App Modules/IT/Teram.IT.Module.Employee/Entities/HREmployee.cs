using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.IT.Module.Employee.Enums;

namespace Teram.IT.Module.Employee.Entities
{

    [Table(nameof(HREmployee) + "s", Schema = "Emp")]
    public class HREmployee:EntityBase
    {
        public int HREmployeeId { get; set; }

        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                if (_code == value) return;
                _code = value;
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

        private string? _employmentNumber;
        public string? EmploymentNumber
        {
            get { return _employmentNumber; }
            set
            {
                if (_employmentNumber == value) return;
                _employmentNumber = value;
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

        private GenderType _gender;
        public GenderType Gender
        {
            get { return _gender; }
            set
            {
                if (_gender == value) return;
                _gender = value;
                OnPropertyChanged();
            }
        }

        private string? _nationality;
        public string? Nationality
        {
            get { return _nationality; }
            set
            {
                if (_nationality == value) return;
                _nationality = value;
                OnPropertyChanged();
            }
        }


        private string? _allegiance;
        public string? Allegiance
        {
            get { return _allegiance; }
            set
            {
                if (_allegiance == value) return;
                _allegiance = value;
                OnPropertyChanged();
            }
        }

        private string? _religion;
        public string? Religion
        {
            get { return _religion; }
            set
            {
                if (_religion == value) return;
                _religion = value;
                OnPropertyChanged();
            }
        }

        private string? _mobile;
        public string? Mobile
        {
            get { return _mobile; }
            set
            {
                if (_mobile == value) return;
                _mobile = value;
                OnPropertyChanged();
            }
        }

        private string? _fatherName;
        public string? FatherName
        {
            get { return _fatherName; }
            set
            {
                if (_fatherName == value) return;
                _fatherName = value;
                OnPropertyChanged();
            }
        }

        private DateTime _birthDate;
        public DateTime BirthDate
        {
            get { return _birthDate; }
            set
            {
                if (_birthDate == value) return;
                _birthDate = value;
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

        private int _status;
        public int Status
        {
            get { return _status; }
            set
            {
                if (_status == value) return;
                _status = value;
                OnPropertyChanged();
            }
        }
    }
}
