using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.HR.Module.Recruitment.Entities.WorkWthUs
{

    [Table(nameof(Education) + "s", Schema = "HR")]
    public class Education : EntityBase
    {
        [Key]
        public int EducationId { get; set; }

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

        private string? _subject;
        [Description("موضوع")]
        public string? Subject
        {
            get => _subject;
            set
            {
                if (_subject == value) return;
                _subject = value;
                OnPropertyChanged();
            }
        }


        private string? _organizationName;
        [Description("سازمان آموزش دهنده")]
        public string? OrganizationName
        {
            get => _organizationName;
            set
            {
                if (_organizationName == value) return;
                _organizationName = value;
                OnPropertyChanged();
            }
        }

        private int? _educationDurationHour;

        [Description("میزان ساعت آموزشی")]
        public int? EducationDurationHour
        {
            get { return _educationDurationHour; }
            set
            {
                if (_educationDurationHour == value) return;
                _educationDurationHour = value;
                OnPropertyChanged();
            }
        }

        private int? _educationYear;
        public int? EducationYear
        {
            get { return _educationYear; }
            set
            {
                if (_educationYear == value) return;
                _educationYear = value;
                OnPropertyChanged();
            }
        }

        private bool _hasCertificate;
        public bool HasCertificate
        {
            get { return _hasCertificate; }
            set
            {           
                _hasCertificate = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(BaseInformationId))]
        public BaseInformation? BaseInformation { get; set; }
    }
}
