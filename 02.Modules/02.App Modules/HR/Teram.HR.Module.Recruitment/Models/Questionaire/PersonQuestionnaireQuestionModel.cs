using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.Questionaires;
using Teram.HR.Module.Recruitment.Enums;

namespace Teram.HR.Module.Recruitment.Models.Questionaire
{
    public class PersonQuestionnaireQuestionModel : ModelBase<PersonQuestionnaireQuestion, int>
    {
        public int PersonQuestionnaireQuestionId { get; set; }
        public int QuestionnaireQuestionId { get; set; }
        public int PersonnelQuestionnaireId { get; set; }
        public Answer Answer { get; set; }
        public DateTime? AnswerDate { get; set; }
        public string QuestionnaireQuestionQuestionTitle {  get; set; }
    }
}
