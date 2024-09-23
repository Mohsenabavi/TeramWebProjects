using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.HR.Module.Recruitment.Enums;

namespace Teram.HR.Module.Recruitment.Entities.WorkWthUs
{
    [Table(nameof(PersonnelComputerSkill) + "s", Schema = "HR")]
    public class PersonnelComputerSkill:EntityBase
    {
        public int PersonnelComputerSkillId { get; set; }

        private ComputerSkill _computerSkill;
        public ComputerSkill ComputerSkill
        {
            get { return _computerSkill; }
            set
            {
                if (_computerSkill == value) return;
                _computerSkill = value;
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
