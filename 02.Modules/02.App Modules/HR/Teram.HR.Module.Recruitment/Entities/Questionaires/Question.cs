using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.HR.Module.Recruitment.Enums;

namespace Teram.HR.Module.Recruitment.Entities.Questionaires
{
    [Table(nameof(Question) + "s", Schema = "HR")]
    public class Question : EntityBase
    {
        public int QuestionId { get; set; }

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
    }
}
