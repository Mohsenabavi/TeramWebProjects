using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.HR.Module.Recruitment.Enums;

namespace Teram.HR.Module.Recruitment.Entities.JobApplicants
{

    [Table(nameof(JobApplicantFile) + "s", Schema = "HR")]
    public class JobApplicantFile : EntityBase
    {
        public int JobApplicantFileId { get; set; }

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

        private int _attachmentTypeId;
        public int AttachmentTypeId
        {
            get { return _attachmentTypeId; }
            set
            {
                if (_attachmentTypeId == value) return;
                _attachmentTypeId = value;
                OnPropertyChanged();
            }
        }

        private Guid _attachmentId;
        public Guid AttachmentId
        {
            get { return _attachmentId; }
            set
            {
                if (_attachmentId == value) return;
                _attachmentId = value;
                OnPropertyChanged();
            }
        }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName == value) return;
                _fileName = value;
                OnPropertyChanged();
            }
        }

        private string _fileExtension;

        public string FileExtension
        {
            get { return _fileExtension; }
            set
            {
                if (_fileExtension == value) return;
                _fileExtension = value;
                OnPropertyChanged();
            }
        }

        [Description("تاریخ ایجاد")]
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

        [Description("تاریخ حذف")]
        private DateTime? _deletedOn;

        public DateTime? DeletedOn
        {
            get { return _deletedOn; }
            set
            {
                if (_deletedOn == value) return;
                _deletedOn = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _approveDateTime;
        public DateTime? ApproveDateTime
        {
            get { return _approveDateTime; }
            set
            {
                if (_approveDateTime == value) return;
                _approveDateTime = value;
                OnPropertyChanged();
            }
        }


        private Guid? _approvedBy;
        public Guid? ApprovedBy
        {
            get { return _approvedBy; }
            set
            {
                if (_approvedBy == value) return;
                _approvedBy = value;
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


        [ForeignKey(nameof(JobApplicantId))]
        public virtual JobApplicant JobApplicant { get; set; }


    }
}
