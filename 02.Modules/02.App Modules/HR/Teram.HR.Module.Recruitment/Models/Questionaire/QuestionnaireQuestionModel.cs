using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.Questionaires;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.Questionaire
{
    public class QuestionnaireQuestionModel : ModelBase<QuestionnaireQuestion, int>
    {
        public int QuestionnaireQuestionId { get; set; }
        public int QuestionnaireId { get; set; }
        public int QuestionId { get; set; }

        [GridColumn(nameof(QuestionTitle))]
        public string QuestionTitle { get; set; }
        public QuestionnaireModel Questionnaire { get; set; }

        [GridColumn(nameof(QuestionnaireTitle))]
        public string QuestionnaireTitle => (Questionnaire != null) ? Questionnaire.Title : "";


    }
}
