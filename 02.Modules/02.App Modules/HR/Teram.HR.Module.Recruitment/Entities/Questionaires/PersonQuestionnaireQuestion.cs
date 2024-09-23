using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.HR.Module.Recruitment.Enums;

namespace Teram.HR.Module.Recruitment.Entities.Questionaires
{
    [Table(nameof(PersonQuestionnaireQuestion) + "s", Schema = "HR")]
    public class PersonQuestionnaireQuestion : EntityBase
    {
        public int PersonQuestionnaireQuestionId { get; set; }

       
        private int _questionnaireQuestionId;
        public int QuestionnaireQuestionId
        {
            get { return _questionnaireQuestionId; }
            set
            {
                if (_questionnaireQuestionId == value) return;
                _questionnaireQuestionId = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(QuestionnaireQuestionId))]
        public virtual QuestionnaireQuestion QuestionnaireQuestion { get; set; }



        private int _personnelQuestionnaireId;
        public int PersonnelQuestionnaireId
        {
            get { return _personnelQuestionnaireId; }
            set
            {
                if (_personnelQuestionnaireId == value) return;
                _personnelQuestionnaireId = value;
                OnPropertyChanged();
            }
        }


        [ForeignKey(nameof(PersonnelQuestionnaireId))]
        public virtual PersonnelQuestionnaire PersonnelQuestionnaire { get; set; }


        private Answer _answer;
        public Answer Answer
        {
            get { return _answer; }
            set
            {
                if (_answer == value) return;
                _answer = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _answerDate;
        public DateTime? AnswerDate
        {
            get { return _answerDate; }
            set
            {
                if (_answerDate == value) return;
                _answerDate = value;
                OnPropertyChanged();
            }
        }
    }
}
