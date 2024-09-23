using Microsoft.CodeAnalysis.Operations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.HR.Module.Recruitment.Entities.Questionaires
{
    [Table(nameof(QuestionnaireQuestion) + "s", Schema = "HR")]
    public class QuestionnaireQuestion : EntityBase
    {
        public int QuestionnaireQuestionId { get; set; }

        private int _questionnaireId;
        public int QuestionnaireId
        {
            get { return _questionnaireId; }
            set
            {
                if (_questionnaireId == value) return;
                _questionnaireId = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(QuestionnaireId))]
        public virtual Questionnaire Questionnaire { get; set; }

        private int _questionId;
        public int QuestionId
        {
            get { return _questionId; }
            set
            {
                if (_questionId == value) return;
                _questionId = value;
                OnPropertyChanged();
            }
        }


        [ForeignKey(nameof(QuestionId))]
        public virtual Question Question { get; set; }

    }
}
