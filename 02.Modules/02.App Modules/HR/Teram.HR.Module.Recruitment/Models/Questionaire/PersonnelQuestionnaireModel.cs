using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.Questionaires;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.Questionaire
{
    public class PersonnelQuestionnaireModel : ModelBase<PersonnelQuestionnaire, int>
    {
        public int PersonnelQuestionnaireId { get; set; }
        public DateTime CreateDate { get; set; }

        [GridColumn(nameof(PersianCreateDate))]
        public string PersianCreateDate => CreateDate.ToPersianDate();

        [GridColumn(nameof(NationalCode))]
        public string NationalCode { get; set; }
        public Guid UserId { get; set; }
        public int QuestionnaireId { get; set; }

    }
}
