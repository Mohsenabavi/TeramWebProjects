using OfficeOpenXml.Style;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Teram.Framework.Core.Domain;
using Teram.HR.Module.Recruitment.Entities.BaseInfo;
using Teram.HR.Module.Recruitment.Entities.WorkWthUs;
using Teram.HR.Module.Recruitment.Enums;

namespace Teram.HR.Module.Recruitment.Entities.JobApplicants
{
    [Table(nameof(JobApplicant) + "s", Schema = "HR")]
    public class JobApplicant : EntityBase
    {
        public int JobApplicantId { get; set; }

        private string _nationalCode;

        [MaxLength(11)]
        public string NationalCode
        {
            get { return _nationalCode; }
            set
            {
                if (_nationalCode == value) return;
                _nationalCode = value;
                OnPropertyChanged();
            }
        }       

        private DateTime? _expreminetDeadline;
        public DateTime? ExpreminetDeadline
        {
            get { return _expreminetDeadline; }
            set
            {
                if (_expreminetDeadline == value) return;
                _expreminetDeadline = value;
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

        private string? _personnelCode;
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

        private ApproveStatus _baseInformationApproveStatus;
        public ApproveStatus BaseInformationApproveStatus
        {
            get { return _baseInformationApproveStatus; }
            set
            {              
                _baseInformationApproveStatus = value;
                OnPropertyChanged();
            }
        }

        private Guid? _baseInformationApprovedBy;
        public Guid? BaseInformationApprovedBy
        {
            get { return _baseInformationApprovedBy; }
            set
            {
                if (_baseInformationApprovedBy == value) return;
                _baseInformationApprovedBy = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _baseInformationApproveDate;
        public DateTime? BaseInformationApproveDate
        {
            get { return _baseInformationApproveDate; }
            set
            {
                if (_baseInformationApproveDate == value) return;
                _baseInformationApproveDate = value;
                OnPropertyChanged();
            }
        }

        private string? _baseInformationErrors;
        public string? BaseInformationErrors
        {
            get { return _baseInformationErrors; }
            set
            {
                if (_baseInformationErrors == value) return;
                _baseInformationErrors = value;
                OnPropertyChanged();
            }
        }

        private string? _address;
     
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


        private ProcessStatus _processStatus;
        public ProcessStatus ProcessStatus
        {
            get { return _processStatus; }
            set
            {
                if (_processStatus == value) return;
                _processStatus = value;
                OnPropertyChanged();
            }
        }

        private DateTime _createDate;
        public DateTime CreateDate
        {
            get { return _createDate; }
            set
            {
                if (_createDate == value) return;
                _createDate = value;
                OnPropertyChanged();
            }
        }

        private Guid _createdBy;
        public Guid CreatedBy
        {
            get { return _createdBy; }
            set
            {
                if (_createdBy == value) return;
                _createdBy = value;
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

        private int _jobPositionId;
        public int JobPositionId
        {
            get { return _jobPositionId; }
            set
            {
                if (_jobPositionId == value) return;
                _jobPositionId = value;
                OnPropertyChanged();
            }
        }


        private string _mobileNumber;
        public string MobileNumber
        {
            get { return _mobileNumber; }
            set
            {
                if (_mobileNumber == value) return;
                _mobileNumber = value;
                OnPropertyChanged();
            }
        }

        private JobCategory _jobCategory;
        public JobCategory JobCategory
        {
            get { return _jobCategory; }
            set
            {
                if (_jobCategory == value) return;
                _jobCategory = value;
                OnPropertyChanged();
            }
        }

        private OccupationalMedicineApproveStatus _occupationalMedicineApproveStatus;
        public OccupationalMedicineApproveStatus OccupationalMedicineApproveStatus
        {
            get { return _occupationalMedicineApproveStatus; }
            set
            {
                if (_occupationalMedicineApproveStatus == value) return;
                _occupationalMedicineApproveStatus = value;
                OnPropertyChanged();
            }
        }

        private bool _occupationalMedicineDone;
        public bool OccupationalMedicineDone
        {
            get { return _occupationalMedicineDone; }
            set
            {               
                _occupationalMedicineDone = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _occupationalMedicineDate;
        public DateTime? OccupationalMedicineDate
        {
            get { return _occupationalMedicineDate; }
            set
            {
                if (_occupationalMedicineDate == value) return;
                _occupationalMedicineDate = value;
                OnPropertyChanged();
            }
        }

        private Guid? _occupationalMedicineApprovedBy;
        public Guid? OccupationalMedicineApprovedBy
        {
            get { return _occupationalMedicineApprovedBy; }
            set
            {
                if (_occupationalMedicineApprovedBy == value) return;
                _occupationalMedicineApprovedBy = value;
                OnPropertyChanged();
            }
        }

        private string? _occupationalMedicineRemarks;
        public string? OccupationalMedicineRemarks
        {
            get { return _occupationalMedicineRemarks; }
            set
            {
                if (_occupationalMedicineRemarks == value) return;
                _occupationalMedicineRemarks = value;
                OnPropertyChanged();
            }
        }

        private bool _noAddictionDone;
        public bool NoAddictionDone
        {
            get { return _noAddictionDone; }
            set
            {               
                _noAddictionDone = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _noAddictionDate;
        public DateTime? NoAddictionDate
        {
            get { return _noAddictionDate; }
            set
            {
                if (_noAddictionDate == value) return;
                _noAddictionDate = value;
                OnPropertyChanged();
            }
        }

        private Guid? _noAddictionApprovedBy;
        public Guid? NoAddictionApprovedBy
        {
            get { return _noAddictionApprovedBy; }
            set
            {
                if (_noAddictionApprovedBy == value) return;
                _noAddictionApprovedBy = value;
                OnPropertyChanged();
            }
        }

        private bool _noBadBackgroundDone;
        public bool NoBadBackgroundDone
        {
            get { return _noBadBackgroundDone; }
            set
            {              
                _noBadBackgroundDone = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _noBadBackgroundDate;
        public DateTime? NoBadBackgroundDate
        {
            get { return _noBadBackgroundDate; }
            set
            {
                if (_noBadBackgroundDate == value) return;
                _noBadBackgroundDate = value;
                OnPropertyChanged();
            }
        }

        private Guid? _noBadBackgroundApprovedBy;
        public Guid? NoBadBackgroundApprovedBy
        {
            get { return _noBadBackgroundApprovedBy; }
            set
            {
                if (_noBadBackgroundApprovedBy == value) return;
                _noBadBackgroundApprovedBy = value;
                OnPropertyChanged();
            }
        }


        private bool _neededForBackgroundCheck;
        public bool NeededForBackgroundCheck
        {
            get { return _neededForBackgroundCheck; }
            set
            {                
                _neededForBackgroundCheck = value;
                OnPropertyChanged();
            }
        }

        private string _promissoryNoteAmount;
        public string PromissoryNoteAmount
        {
            get { return _promissoryNoteAmount; }
            set
            {
                if (_promissoryNoteAmount == value) return;
                _promissoryNoteAmount = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _invitationToWorkDate;
        public DateTime? InvitationToWorkDate
        {
            get { return _invitationToWorkDate; }
            set
            {
                if (_invitationToWorkDate == value) return;
                _invitationToWorkDate = value;
                OnPropertyChanged();
            }
        }

        private Guid? _invitedBy;
        public Guid? InvitedBy
        {
            get { return _invitedBy; }
            set
            {
                if (_invitedBy == value) return;
                _invitedBy = value;
                OnPropertyChanged();
            }
        }

        private FlowType _flowType;
        public FlowType FlowType
        {
            get { return _flowType; }
            set
            {
                if (_flowType == value) return;
                _flowType = value;
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

        private string? _fatherName;
        [Description("نام پدر")]
        public string? FatherName
        {
            get => _fatherName;
            set
            {
                if (_fatherName == value) return;
                _fatherName = value;
                OnPropertyChanged();
            }
        }



        public virtual BaseInformation? BaseInformation {  get; set; }
        public virtual ICollection<JobApplicantFile> JobApplicantFiles { get; set; }
        public virtual ICollection<JobApplicantsIntroductionLetter> JobApplicantsIntroductionLetters { get; set; }
        public virtual ICollection<JobApplicantApproveHistory> JobApplicantsApproveHistory { get; set; }
        public virtual ICollection<WorkerJobBackground>? WorkerJobBackgrounds { get; set; }
        public virtual ICollection<EmployeeJobBackground>? EmployeeJobBackgrounds { get; set; }

        public virtual ICollection<HSEApproveHistory> HSEApproveHistories { get; set; }

        [ForeignKey(nameof(JobPositionId))]
        public virtual JobPosition? JobPosition { get; set; }
    }
}
