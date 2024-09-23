using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.HR.Module.Recruitment.Enums;

namespace Teram.HR.Module.Recruitment.Entities.JobApplicants
{
    [Table("JobApplicantApproveHistoris", Schema = "HR")]
    public class JobApplicantApproveHistory:EntityBase
    {
        public int JobApplicantApproveHistoryId { get; set; }

        private Guid _approvedByUserId;
        public Guid ApprovedByUserId
        {
            get { return _approvedByUserId; }
            set
            {
                if (_approvedByUserId == value) return;
                _approvedByUserId = value;
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

        private ApproveStatus _approveStatus;
        public ApproveStatus ApproveStatus
        {
            get { return _approveStatus; }
            set
            {
                if (_approveStatus == value) return;
                _approveStatus = value;
                OnPropertyChanged();
            }
        }

        private string _remarks;
        public string Remarks
        {
            get { return _remarks; }
            set
            {
                if (_remarks == value) return;
                _remarks = value;
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
