using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.HR.Module.Recruitment.Enums;

namespace Teram.HR.Module.Recruitment.Entities.Questionaires
{
    [Table(nameof(Questionnaire) + "s", Schema = "HR")]
    public class Questionnaire : EntityBase
    {
        public int QuestionnaireId { get; set; }


        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged();
            }
        }


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

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {             
                _isActive = value;
                OnPropertyChanged();
            }
        }

        public virtual ICollection<QuestionnaireQuestion> QuestionnaireQuestions { get; set; }

        public virtual ICollection<PersonnelQuestionnaire> PersonnelQuestionnaires { get; set; }

    }
}
