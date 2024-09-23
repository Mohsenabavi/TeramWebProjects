using System.Reflection;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.WorkWthUs;
using Teram.HR.Module.Recruitment.Enums;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.WorkWithUs
{
    public class PersonnelLanguageModel : ModelBase<PersonnelLanguage, int>
    {
        public int BaseInformationId { get; set; }

        [GridColumn(nameof(BaseInformationNationalCode))]
        public string BaseInformationNationalCode { get; set; }

        public string BaseInformationName { get; set; }

        public string BaseInformationLastname { get; set; }

        [GridColumn(nameof(FullName))]
        public string FullName => BaseInformationName + " " + BaseInformationLastname;
        public int PersonnelLanguageId { get; set; }
        public Enums.Language Language { get; set; }

        [GridColumn(nameof(LanguageTitle))]
        public string LanguageTitle => (Language > 0) ? Language.GetDescription() : "-";
        public SkillLevel SkillLevel { get; set; }

        [GridColumn(nameof(SkillLevelTitle))]
        public string SkillLevelTitle => (SkillLevel > 0) ? SkillLevel.GetDescription() : "-";
        public string? Description { get; set; }

    }
}
