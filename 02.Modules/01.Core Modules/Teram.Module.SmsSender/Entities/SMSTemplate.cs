using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.Module.SmsSender.Entities
{
    [Table(nameof(SMSTemplate) + "s", Schema = "SMS")]
    public class SMSTemplate:EntityBase
    {
        public int SMSTemplateId { get; init; }

        private string _title;

        [MaxLength(50)]
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

        private string _template;
        public string Template
        {
            get { return _template; }
            set
            {
                if (_template == value) return;
                _template = value;
                OnPropertyChanged();
            }
        }
    }
}
