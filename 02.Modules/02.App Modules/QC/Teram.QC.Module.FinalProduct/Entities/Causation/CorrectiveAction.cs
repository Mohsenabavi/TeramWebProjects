using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.FinalProduct.Entities.Causation
{
    [Table(nameof(CorrectiveAction) +"s", Schema = "QCFP")]
    public class CorrectiveAction:EntityBase
    {
        public int CorrectiveActionId { get; set; }       

        private string _descriiption;
        public string Descriiption
        {
            get { return _descriiption; }
            set
            {
                if (_descriiption == value) return;
                _descriiption = value;
                OnPropertyChanged();
            }
        }
        private int _causationId;
        public int CausationId
        {
            get { return _causationId; }
            set
            {
                if (_causationId == value) return;
                _causationId = value;
                OnPropertyChanged();
            }
        }

        private int _actionerId;
        public int ActionerId
        {
            get { return _actionerId; }
            set
            {
                if (_actionerId == value) return;
                _actionerId = value;
                OnPropertyChanged();
            }
        }

        private DateTime _actionDate;
        public DateTime ActionDate
        {
            get { return _actionDate; }
            set
            {
                if (_actionDate == value) return;
                _actionDate = value;
                OnPropertyChanged();
            }
        }

        private int _approverId;   
        public int ApproverId
        {
            get { return _approverId; }
            set
            {
                if (_approverId == value) return;
                _approverId = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(ActionerId))]
        public virtual Actioner? Actioner { get; set; }      

        [ForeignKey(nameof(CausationId))]
        public virtual Causation? Causation { get; set; }
    }
}
