using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.HR.Module.Recruitment.Enums;

namespace Teram.HR.Module.Recruitment.Entities.JobApplicants
{
    [Table("HSEApproveHistories", Schema = "HR")]
    public class HSEApproveHistory:EntityBase
    {
        public int HSEApproveHistoryId { get; set; }

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

        private Guid? _referralAtachmentId;
        public Guid? ReferralAtachmentId
        {
            get { return _referralAtachmentId; }
            set
            {
                if (_referralAtachmentId == value) return;
                _referralAtachmentId = value;
                OnPropertyChanged();
            }
        }

        private Guid? _fileSummaryAttchmanetId;
        public Guid? FileSummaryAttchmanetId
        {
            get { return _fileSummaryAttchmanetId; }
            set
            {
                if (_fileSummaryAttchmanetId == value) return;
                _fileSummaryAttchmanetId = value;
                OnPropertyChanged();
            }
        }

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
