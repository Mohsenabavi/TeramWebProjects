using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.WorkWthUs;
using Teram.HR.Module.Recruitment.Enums;

namespace Teram.HR.Module.Recruitment.Models.WorkWithUs
{
    public class PersonnelComputerSkillModel :ModelBase<PersonnelComputerSkill,int>
    {
        public int PersonnelComputerSkillId { get; set; }

        public ComputerSkill ComputerSkill { get; set; }

        public string ComputerSkillTitle => (ComputerSkill > 0) ? ComputerSkill.GetDescription() : "-";

        public SkillLevel SkillLevel { get; set; }

        public string SkillLevelTitle => (SkillLevel > 0) ? SkillLevel.GetDescription() : "-";

        public int BaseInformationId {  get; set; }
    }
}
