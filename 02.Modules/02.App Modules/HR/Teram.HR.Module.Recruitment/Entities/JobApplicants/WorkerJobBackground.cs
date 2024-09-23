using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.HR.Module.Recruitment.Enums;

namespace Teram.HR.Module.Recruitment.Entities.JobApplicants
{
    [Table(nameof(WorkerJobBackground) + "s", Schema = "HR")]
    public class WorkerJobBackground:EntityBase
    {
        public int WorkerJobBackgroundId { get; set; }


        private string _jobTitle;
        public string JobTitle
        {
            get { return _jobTitle; }
            set
            {
                if (_jobTitle == value) return;
                _jobTitle = value;
                OnPropertyChanged();
            }
        }

        private bool _addressIMatch;
        public bool AddressIMatch
        {
            get { return _addressIMatch; }
            set
            {              
                _addressIMatch = value;
                OnPropertyChanged();
            }
        }

        private bool _resumeIsMatch;
        public bool ResumeIsMatch
        {
            get { return _resumeIsMatch; }
            set
            {               
                _resumeIsMatch = value;
                OnPropertyChanged();
            }
        }

        private string _statementOfPreviousWorkplace;
        public string StatementOfPreviousWorkplace
        {
            get { return _statementOfPreviousWorkplace; }
            set
            {
                if (_statementOfPreviousWorkplace == value) return;
                _statementOfPreviousWorkplace = value;
                OnPropertyChanged();
            }
        }

        private string? _firstApprovePerson;
        public string? FirstApprovePerson
        {
            get { return _firstApprovePerson; }
            set
            {
                if (_firstApprovePerson == value) return;
                _firstApprovePerson = value;
                OnPropertyChanged();
            }
        }

        private string? _firstApprovePersonRemarks;
        public string? FirstApprovePersonRemarks
        {
            get { return _firstApprovePersonRemarks; }
            set
            {
                if (_firstApprovePersonRemarks == value) return;
                _firstApprovePersonRemarks = value;
                OnPropertyChanged();
            }
        }

        private string? _secondApprovePerson;
        public string? SecondApprovePerson
        {
            get { return _secondApprovePerson; }
            set
            {
                if (_secondApprovePerson == value) return;
                _secondApprovePerson = value;
                OnPropertyChanged();
            }
        }

        private string? _secondtApprovePersonRemarks;
        public string? SecondtApprovePersonRemarks
        {
            get { return _secondtApprovePersonRemarks; }
            set
            {
                if (_secondtApprovePersonRemarks == value) return;
                _secondtApprovePersonRemarks = value;
                OnPropertyChanged();
            }
        }

        private string? _thirdApprovePerson;
        public string? ThirdApprovePerson
        {
            get { return _thirdApprovePerson; }
            set
            {
                if (_thirdApprovePerson == value) return;
                _thirdApprovePerson = value;
                OnPropertyChanged();
            }
        }
        private string? _thirdApprovePersonRemarks;
        public string? ThirdApprovePersonRemarks
        {
            get { return _thirdApprovePersonRemarks; }
            set
            {
                if (_thirdApprovePersonRemarks == value) return;
                _thirdApprovePersonRemarks = value;
                OnPropertyChanged();
            }
        }

        private string _researcherName;
        public string ResearcherName
        {
            get { return _researcherName; }
            set
            {
                if (_researcherName == value) return;
                _researcherName = value;
                OnPropertyChanged();
            }
        }

        private DateTime _approveDate;
        public DateTime ApproveDate
        {
            get { return _approveDate; }
            set
            {
                if (_approveDate == value) return;
                _approveDate = value;
                OnPropertyChanged();
            }
        }

        private Guid _approvedBy;
        public Guid ApprovedBy
        {
            get { return _approvedBy; }
            set
            {
                if (_approvedBy == value) return;
                _approvedBy = value;
                OnPropertyChanged();
            }
        }

        private BackgroundJobApproveStatus _approveStatus;
        public BackgroundJobApproveStatus ApproveStatus
        {
            get { return _approveStatus; }
            set
            {
                if (_approveStatus == value) return;
                _approveStatus = value;
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

        private string _finalStatement;
        public string FinalStatement
        {
            get { return _finalStatement; }
            set
            {
                if (_finalStatement == value) return;
                _finalStatement = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(JobApplicantId))]
        public virtual JobApplicant JobApplicant { get; set; }

    }
}
