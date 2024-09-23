using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.Framework.Core.Extensions;
using Teram.HR.Module.Recruitment.Enums;

namespace Teram.HR.Module.Recruitment.Entities.WorkWthUs
{

    [Table(nameof(PersonnelLanguage) + "s", Schema = "HR")]
    public class PersonnelLanguage:EntityBase
    {
        public int PersonnelLanguageId { get; set; }

        private Enums.Language _language;
        public Enums.Language Language
        {
            get { return _language; }
            set
            {
                if (_language == value) return;
                _language = value;
                OnPropertyChanged();
            }
        }

        private SkillLevel _skillLevel;
        public SkillLevel SkillLevel
        {
            get { return _skillLevel; }
            set
            {
                if (_skillLevel == value) return;
                _skillLevel = value;
                OnPropertyChanged();
            }
        }

        private string? _description;
        public string? Description
        {
            get { return _description; }
            set
            {
                if (_description == value) return;
                _description = value;
                OnPropertyChanged();
            }
        }

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

        [ForeignKey(nameof(BaseInformationId))]
        public virtual BaseInformation? BaseInformation { get; set; }
    }
}
