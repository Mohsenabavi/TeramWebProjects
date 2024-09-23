using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.HR.Module.Recruitment.Enums;

namespace Teram.HR.Module.Recruitment.Entities.WorkWthUs
{

    [Table(nameof(TrainingRecord) + "s", Schema = "HR")]
    public class TrainingRecord:EntityBase
    {
        [Key]
        public int TrainingRecordId { get; set; }

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

        private string? _collegeName;

        [Description("نام محل تحصیل")]
        public string? CollegeName
        {
            get => _collegeName;
            set
            {
                if (_collegeName == value) return;
                _collegeName = value;
                OnPropertyChanged();
            }
        }

        private EducationGrade? _educationGrade;
        [Description("مقطع تحصیلی")]
        public EducationGrade? EducationGrade
        {
            get => _educationGrade;
            set
            {
                if (_educationGrade == value) return;
                _educationGrade = value;
                OnPropertyChanged();
            }
        }

        [Description("معدل")]
        public double? Average
        {
            get; set;
        }

        private string? _field;
        [Description("رشته")]
        public string? Field
        {
            get => _field;
            set
            {
                if (_field == value) return;
                _field = value;
                OnPropertyChanged();
            }
        }

        private int? _educationCityId;
        public int? EducationCityId
        {
            get { return _educationCityId; }
            set
            {
                if (_educationCityId == value) return;
                _educationCityId = value;
                OnPropertyChanged();
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate == value) return;
                _startDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate == value) return;
                _endDate = value;
                OnPropertyChanged();
            }
        }


        [ForeignKey(nameof(BaseInformationId))]
        public BaseInformation? BaseInformation { get; set; }
    }
}
