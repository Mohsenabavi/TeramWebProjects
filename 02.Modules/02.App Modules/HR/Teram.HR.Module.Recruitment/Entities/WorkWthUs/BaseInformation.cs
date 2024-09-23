using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using Teram.ServiceContracts;
using Teram.HR.Module.Recruitment.Enums;
using Teram.Framework.Core.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;

namespace Teram.HR.Module.Recruitment.Entities.WorkWthUs
{

    [Table(nameof(BaseInformation), Schema = "HR")]
    public class BaseInformation : EntityBase
    {

        [Key]
        public int BaseInformationId { get; set; }

        private string _name;
        [Description("نام")]
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }


        private string _lastname;
        [Description("نام خانوادگی")]
        public string Lastname
        {
            get => _lastname;
            set
            {
                if (_lastname == value) return;
                _lastname = value;
                OnPropertyChanged();
            }
        }

        private string _fatherName;
        [Description("نام پدر")]
        public string FatherName
        {
            get => _fatherName;
            set
            {
                if (_fatherName == value) return;
                _fatherName = value;
                OnPropertyChanged();
            }
        }


        private string? _identityNumber;

        [Description("شماره شناسنامه")]
        public string? IdentityNumber
        {
            get => _identityNumber;
            set
            {
                if (_identityNumber == value) return;
                _identityNumber = value;
                OnPropertyChanged();
            }
        }

        private string? _identitySerialNumber;
        [MaxLength(50)]
        public string? IdentitySerialNumber
        {
            get { return _identitySerialNumber; }
            set
            {
                if (_identitySerialNumber == value) return;
                _identitySerialNumber = value;
                OnPropertyChanged();
            }
        }

        private string _nationalCode;

        [Description("کد ملی")]
        public string NationalCode
        {
            get => _nationalCode;
            set
            {
                if (_nationalCode == value) return;
                _nationalCode = value;
                OnPropertyChanged();
            }
        }

        private GenderType _gender;

        [Description("جنسیت")]
        public GenderType Gender
        {
            get => _gender;
            set
            {
                if (_gender == value) return;
                _gender = value;
                OnPropertyChanged();
            }
        }

        private string? _phone;
        [Description("تلفن")]
        public string? Phone
        {
            get => _phone;
            set
            {
                if (_phone == value) return;
                _phone = value;
                OnPropertyChanged();
            }
        }


        private string? _postalCode;
        public string? PostalCode
        {
            get { return _postalCode; }
            set
            {
                if (_postalCode == value) return;
                _postalCode = value;
                OnPropertyChanged();
            }
        }


        private string _mobile;
        [Description("موبایل")]
        public string Mobile
        {
            get => _mobile;
            set
            {
                if (_mobile == value) return;
                _mobile = value;
                OnPropertyChanged();
            }
        }

        private int? _birthLocationId;
        [Description("محل تولد")]
        public int? BirthLocationId
        {
            get => _birthLocationId;
            set
            {
                if (_birthLocationId == value) return;
                _birthLocationId = value;
                OnPropertyChanged();
            }
        }

        private string? _address;
        [Description("آدرس")]
        public string? Address
        {
            get => _address;
            set
            {
                if (_address == value) return;
                _address = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _birthDate;

        [Description("تاریخ تولد")]
        public DateTime? BirthDate
        {
            get => _birthDate;
            set
            {
                if (_birthDate == value) return;
                _birthDate = value;
                OnPropertyChanged();
            }
        }
        private DateTime? _startDateMilitaryService;
        [Description("تاریخ شروع خدمت")]
        public DateTime? StartDateMilitaryService
        {
            get => _startDateMilitaryService;
            set
            {
                if (_startDateMilitaryService == value) return;
                _startDateMilitaryService = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _endDateMilitaryService;
        [Description("تاریخ پایان خدمت")]
        public DateTime? EndDateMilitaryService
        {
            get => _endDateMilitaryService;
            set
            {
                if (_endDateMilitaryService == value) return;
                _endDateMilitaryService = value;
                OnPropertyChanged();
            }
        }


        private MilitaryServiceStatus? _militaryServiceStatus;
        [Description("وضعیت نظام وظبفه")]
        public MilitaryServiceStatus? MilitaryServiceStatus
        {
            get => _militaryServiceStatus;
            set
            {
                if (_militaryServiceStatus == value) return;
                _militaryServiceStatus = value;
                OnPropertyChanged();
            }
        }

        private string? _medicalExemptionReason;
        public string? MedicalExemptionReason
        {
            get { return _medicalExemptionReason; }
            set
            {
                if (_medicalExemptionReason == value) return;
                _medicalExemptionReason = value;
                OnPropertyChanged();
            }
        }     

        private string? _insuranceMonths;
        [Description("مدت بیمه به ماه")]
        public string? InsuranceMonths
        {
            get => _insuranceMonths;
            set
            {
                if (_insuranceMonths == value) return;
                _insuranceMonths = value;
                OnPropertyChanged();
            }
        }

        private BloodGroup? _bloodGroup;
        [Description("گروه خونی")]
        public BloodGroup? BloodGroup
        {
            get => _bloodGroup;
            set
            {
                if (_bloodGroup == value) return;
                _bloodGroup = value;
                OnPropertyChanged();
            }
        }      

        private MaritalStatus _marriageStatus;
        [Description("وضعیت تاهل")]
        public MaritalStatus MarriageStatus
        {
            get => _marriageStatus;
            set
            {
                if (_marriageStatus == value) return;
                _marriageStatus = value;
                OnPropertyChanged();
            }
        }

        private string? _email;
        [Description("ایمیل")]
        public string? Email
        {
            get => _email;
            set
            {
                if (_email == value) return;
                _email = value;
                OnPropertyChanged();
            }
        }

        private string? _insuranceNumber;
        [Description("شماره بیمه")]
        public string? InsuranceNumber
        {
            get => _insuranceNumber;
            set
            {
                if (_insuranceNumber == value) return;
                _insuranceNumber = value;
                OnPropertyChanged();
            }
        }

        private int? _homeCityId;
        [Description("شهر")]
        public int? HomeCityId
        {
            get => _homeCityId;
            set
            {
                if (_homeCityId == value) return;
                _homeCityId = value;
                OnPropertyChanged();
            }
        }

        [Description("تاریخ ثبت نام")]

        private DateTime _createdOn;
        public DateTime CreatedOn
        {
            get { return _createdOn; }
            set
            {
                if (_createdOn == value) return;
                _createdOn = value;
                OnPropertyChanged();
            }
        }


        private string _nationality;
        [MaxLength(20)]
        public string Nationality
        {
            get { return _nationality; }
            set
            {
                if (_nationality == value) return;
                _nationality = value;
                OnPropertyChanged();
            }
        }

        private string _citizenship;
        [MaxLength(20)]
        public string Citizenship
        {
            get { return _citizenship; }
            set
            {
                if (_citizenship == value) return;
                _citizenship = value;
                OnPropertyChanged();
            }
        }

        private decimal? _latitude;
        [Column(TypeName = "decimal(18,15)")]
        public decimal? Latitude
        {
            get => _latitude;
            set
            {
                if (_latitude == value) return;
                _latitude = value;
                OnPropertyChanged();
            }
        }

        private decimal? _longitude;
        [Column(TypeName = "decimal(18,15)")]
        public decimal? Longitude
        {
            get => _longitude;
            set
            {
                if (_longitude == value) return;
                _longitude = value;
                OnPropertyChanged();
            }
        }

        private EducationGrade? _partnerEducationLevel;
        public EducationGrade? PartnerEducationLevel
        {
            get { return _partnerEducationLevel; }
            set
            {
                if (_partnerEducationLevel == value) return;
                _partnerEducationLevel = value;
                OnPropertyChanged();
            }
        }

        private string? _partnerJob;
        public string? PartnerJob
        {
            get { return _partnerJob; }
            set
            {
                if (_partnerJob == value) return;
                _partnerJob = value;
                OnPropertyChanged();
            }
        }

        private int _childCount;
        public int ChildCount
        {
            get { return _childCount; }
            set
            {
                if (_childCount == value) return;
                _childCount = value;
                OnPropertyChanged();
            }
        }

        private bool _hasWorkingRelatives;
        public bool HasWorkingRelatives
        {
            get { return _hasWorkingRelatives; }
            set
            {
                if (_hasWorkingRelatives == value) return;
                _hasWorkingRelatives = value;
                OnPropertyChanged();
            }
        }

        private string? _workingRelatives;
        public string? WorkingRelatives
        {
            get { return _workingRelatives; }
            set
            {
                if (_workingRelatives == value) return;
                _workingRelatives = value;
                OnPropertyChanged();
            }
        }

        private string? _thesisTitle;
        public string? ThesisTitle
        {
            get { return _thesisTitle; }
            set
            {
                if (_thesisTitle == value) return;
                _thesisTitle = value;
                OnPropertyChanged();
            }
        }

        private string? _specialAcademicAchievements;
        public string? SpecialAcademicAchievements
        {
            get { return _specialAcademicAchievements; }
            set
            {
                if (_specialAcademicAchievements == value) return;
                _specialAcademicAchievements = value;
                OnPropertyChanged();
            }
        }

        private string? _specialWorkSuccesses;
        public string? SpecialWorkSuccesses
        {
            get { return _specialWorkSuccesses; }
            set
            {
                if (_specialWorkSuccesses == value) return;
                _specialWorkSuccesses = value;
                OnPropertyChanged();
            }
        }

        private string? _freeTimeActivities;
        public string? FreeTimeActivities
        {
            get { return _freeTimeActivities; }
            set
            {
                if (_freeTimeActivities == value) return;
                _freeTimeActivities = value;
                OnPropertyChanged();
            }
        }

        private bool _hasSpecialDisease;
        public bool HasSpecialDisease
        {
            get { return _hasSpecialDisease; }
            set
            {
                _hasSpecialDisease = value;
                OnPropertyChanged();
            }
        }

        private string? _specialDisease;
        public string? SpecialDisease
        {
            get { return _specialDisease; }
            set
            {
                _specialDisease = value;
                OnPropertyChanged();
            }
        }

        private bool _hasCriminalRecord;
        public bool HasCriminalRecord
        {
            get { return _hasCriminalRecord; }
            set
            {
                _hasCriminalRecord = value;
                OnPropertyChanged();
            }
        }

        private string? _criminalRecord;
        public string? CriminalRecord
        {
            get { return _criminalRecord; }
            set
            {
                if (_criminalRecord == value) return;
                _criminalRecord = value;
                OnPropertyChanged();
            }
        }

        private bool _canWorkInShifts;
        public bool CanWorkInShifts
        {
            get { return _canWorkInShifts; }
            set
            {
                _canWorkInShifts = value;
                OnPropertyChanged();
            }
        }

        private bool _businessMissionAbility;
        public bool BusinessMissionAbility
        {
            get { return _businessMissionAbility; }
            set
            {
                _businessMissionAbility = value;
                OnPropertyChanged();
            }
        }

        private bool _hasIntentionToMigrate;
        public bool HasIntentionToMigrate
        {
            get { return _hasIntentionToMigrate; }
            set
            {
                if (_hasIntentionToMigrate == value) return;
                _hasIntentionToMigrate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _readyToWorkDate;
        public DateTime? ReadyToWorkDate
        {
            get { return _readyToWorkDate; }
            set
            {
                if (_readyToWorkDate == value) return;
                _readyToWorkDate = value;
                OnPropertyChanged();
            }
        }

        private string? _currentJobActivity;
        public string? CurrentJobActivity
        {
            get { return _currentJobActivity; }
            set
            {
                if (_currentJobActivity == value) return;
                _currentJobActivity = value;
                OnPropertyChanged();
            }
        }

        private decimal? _currntSalary;
        public decimal? CurrntSalary
        {
            get { return _currntSalary; }
            set
            {
                if (_currntSalary == value) return;
                _currntSalary = value;
                OnPropertyChanged();
            }
        }

        private bool _hasWorkingRelativeInPackingCompanies;
        public bool HasWorkingRelativeInPackingCompanies
        {
            get { return _hasWorkingRelativeInPackingCompanies; }
            set
            {
                _hasWorkingRelativeInPackingCompanies = value;
                OnPropertyChanged();
            }
        }

        private string? _workingRelativeInPackingCompanyName;
        public string? WorkingRelativeInPackingCompanyName
        {
            get { return _workingRelativeInPackingCompanyName; }
            set
            {
                if (_workingRelativeInPackingCompanyName == value) return;
                _workingRelativeInPackingCompanyName = value;
                OnPropertyChanged();
            }
        }
        private int _jobApplicantId;
        public int JobApplicantId
        {
            get { return _jobApplicantId; }
            set
            {
                if (_jobApplicantId == value) return;
                _jobApplicantId = value;
                OnPropertyChanged();
            }
        }
        public virtual JobApplicant JobApplicant { get; set; }
        public virtual ICollection<Resume>? Resumes { get; set; }
        public virtual ICollection<Education>? Educations { get; set; }
        public virtual ICollection<TrainingRecord>? TrainingRecords { get; set; }
        public virtual ICollection<EmergencyContact>? EmergencyContacts { get; set; }
        public virtual ICollection<PersonnelLanguage>? PersonnelLanguages { get; set; }
        public virtual ICollection<PersonnelComputerSkill>? PersonnelComputerSkills { get; set; }

        



    }
}
