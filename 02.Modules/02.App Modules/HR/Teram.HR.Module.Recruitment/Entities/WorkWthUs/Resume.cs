using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.HR.Module.Recruitment.Entities.WorkWthUs
{

    [Table(nameof(Resume) + "s", Schema = "HR")]
    public class Resume : EntityBase
    {     
        public int ResumeId { get; set; }

        private int _BaseInformationId;
        public int BaseInformationId
        {
            get => _BaseInformationId;
            set
            {
                if (_BaseInformationId == value) return;
                _BaseInformationId = value;
                OnPropertyChanged();
            }
        }

        private string? _companyName;
        [Description("نام موسسه")]
        public string? CompanyName
        {
            get => _companyName;
            set
            {
                if (_companyName == value) return;
                _companyName = value;
                OnPropertyChanged();
            }
        }

        private string? _endCooperationReason;
        [Description("علت خاتمه همکاری")]
        public string? EndCooperationReason
        {
            get => _endCooperationReason;
            set
            {
                if (_endCooperationReason == value) return;
                _endCooperationReason = value;
                OnPropertyChanged();
            }
        }

        private string? _activity;
        [Description("حوزه فعالیت")]
        public string? Activity
        {
            get => _activity;
            set
            {
                if (_activity == value) return;
                _activity = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _startDate;
        [Description("تاریخ شروع")]
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                OnPropertyChanged();
            }
        }


        private DateTime? _finishDate;
        [Description("تاریخ اتمام")]
        public DateTime? FinishDate
        {
            get => _finishDate;
            set
            {
                if (_finishDate == value) return;
                _finishDate = value;
                OnPropertyChanged();
            }
        }
       

        private string? _organizationalPosition;
        [Description("موقعیت سازمانی")]
        public string? OrganizationalPosition
        {
            get => _organizationalPosition;
            set
            {
                if (_organizationalPosition == value) return;
                _organizationalPosition = value;
                OnPropertyChanged();
            }
        }

        private string? _directSupervisorName;
        public string? DirectSupervisorName
        {
            get { return _directSupervisorName; }
            set
            {
                if (_directSupervisorName == value) return;
                _directSupervisorName = value;
                OnPropertyChanged();
            }
        }

        private string? _directSupervisorPhone;
        public string? DirectSupervisorPhone
        {
            get { return _directSupervisorPhone; }
            set
            {
                if (_directSupervisorPhone == value) return;
                _directSupervisorPhone = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(BaseInformationId))]
        public BaseInformation? BaseInformation { get; set; }
    }
}
