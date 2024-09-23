using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.Module.SmsSender.Entities
{
    [Table("SMSHistories", Schema = "SMS")]
    public class SMSHistory :EntityBase
    {
        public int SMSHistoryId { get; set; }

        private string _senderId;
        public string SenderId
        {
            get { return _senderId; }
            set
            {
                if (_senderId == value) return;
                _senderId = value;
                OnPropertyChanged();
            }
        }

        private string _recieverNumber;
        public string RecieverNumber
        {
            get { return _recieverNumber; }
            set
            {
                if (_recieverNumber == value) return;
                _recieverNumber = value;
                OnPropertyChanged();
            }
        }

        private DateTime _sendDate;
        public DateTime SendDate
        {
            get { return _sendDate; }
            set
            {
                if (_sendDate == value) return;
                _sendDate = value;
                OnPropertyChanged();
            }
        }


        private string _messageContext;
        public string MessageContext
        {
            get { return _messageContext; }
            set
            {
                if (_messageContext == value) return;
                _messageContext = value;
                OnPropertyChanged();
            }
        }


        private Guid _batchId;
        public Guid BatchId
        {
            get { return _batchId; }
            set
            {
                if (_batchId == value) return;
                _batchId = value;
                OnPropertyChanged();
            }
        }

        private bool _isSuccessful;
        public bool IsSuccessful
        {
            get { return _isSuccessful; }
            set
            {
                if (_isSuccessful == value) return;
                _isSuccessful = value;
                OnPropertyChanged();
            }
        }

        private int _statusCode;
        public int StatusCode
        {
            get { return _statusCode; }
            set
            {
                if (_statusCode == value) return;
                _statusCode = value;
                OnPropertyChanged();
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message == value) return;
                _message = value;
                OnPropertyChanged();
            }
        }       
    }
}
