using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.HR.Module.Recruitment.Entities.Questionaires
{

    [Table(nameof(PersonnelQuestionnaire) + "s", Schema = "HR")]
    public class PersonnelQuestionnaire : EntityBase
    {
        public int PersonnelQuestionnaireId { get; set; }

        private DateTime _createDate;
        public DateTime CreateDate
        {
            get { return _createDate; }
            set
            {
                if (_createDate == value) return;
                _createDate = value;
                OnPropertyChanged();
            }
        }

        private string _nationalCode;
        public string NationalCode
        {
            get { return _nationalCode; }
            set
            {
                if (_nationalCode == value) return;
                _nationalCode = value;
                OnPropertyChanged();
            }
        }

        private Guid _userId;
        public Guid UserId
        {
            get { return _userId; }
            set
            {
                if (_userId == value) return;
                _userId = value;
                OnPropertyChanged();
            }
        }

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
        public virtual ICollection<PersonQuestionnaireQuestion> PersonQuestionnaireQuestions { get; set; }
    }
}
