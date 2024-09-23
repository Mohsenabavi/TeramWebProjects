using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.HR.Module.Recruitment.Enums;

namespace Teram.HR.Module.Recruitment.Entities.JobApplicants
{
    public class EmployeeJobBackground : EntityBase
    {
        public int EmployeeJobBackgroundId { get; set; }

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

        private bool _performanceIsApproved;
        public bool PerformanceIsApproved
        {
            get { return _performanceIsApproved; }
            set
            {
                _performanceIsApproved = value;
                OnPropertyChanged();
            }
        }

        private bool _disciplineIsApproved;
        public bool DisciplineIsApproved
        {
            get { return _disciplineIsApproved; }
            set
            {
                _disciplineIsApproved = value;
                OnPropertyChanged();
            }
        }

        private bool _moralityIsApproved;
        public bool MoralityIsApproved
        {
            get { return _moralityIsApproved; }
            set
            {
                _moralityIsApproved = value;
                OnPropertyChanged();
            }
        }


        private string? _approvePerson;
        public string? ApprovePerson
        {
            get { return _approvePerson; }
            set
            {
                if (_approvePerson == value) return;
                _approvePerson = value;
                OnPropertyChanged();
            }
        }

        private string? _approvePersonRemarks;
        public string? ApprovePersonRemarks
        {
            get { return _approvePersonRemarks; }
            set
            {
                if (_approvePersonRemarks == value) return;
                _approvePersonRemarks = value;
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

        private DateTime? _approveDate;
        public DateTime? ApproveDate
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

        [ForeignKey(nameof(JobApplicantId))]
        public virtual JobApplicant JobApplicant { get; set; }

    }
}
