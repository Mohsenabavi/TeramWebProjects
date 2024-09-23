using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.HR.Module.Recruitment.Entities.WorkWthUs
{

    [Table(nameof(EmergencyContact) + "s", Schema = "HR")]
    public class EmergencyContact:EntityBase
    {

        [Key]
        public int EmergencyContactId { get; set; }

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

        private string? _name;

        [Description("نام")]
        public string? Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        private string? _lastName;

        [Description("نام خانوادگی")]
        public string? LastName
        {
            get => _lastName;
            set
            {
                if (_lastName == value) return;
                _lastName = value;
                OnPropertyChanged();
            }
        }

        private string? _relation;
        [Description("نسبت")]
        public string? Relation
        {
            get => _relation;
            set
            {
                if (_relation == value) return;
                _relation = value;
                OnPropertyChanged();
            }
        }

        private string? _mobile;

        [Description("شماره تماس")]

        public string? Mobile
        {
            get { return _mobile; }
            set
            {
                if (_mobile == value) return;
                _mobile = value;
                OnPropertyChanged();
            }
        }


        [ForeignKey(nameof(BaseInformationId))]

        public BaseInformation? BaseInformation { get; set; }
    }
}
