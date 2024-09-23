using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.IncomingGoods.Entities
{

    [Table(nameof(IncomingGoodsInspectionCartableItem)+"s", Schema = "QC")]
    public class IncomingGoodsInspectionCartableItem:EntityBase
    {
        public int IncomingGoodsInspectionCartableItemId { get; set; }

        private int _incomingGoodsInspectionId;
        public int IncomingGoodsInspectionId
        {
            get { return _incomingGoodsInspectionId; }
            set
            {
                if (_incomingGoodsInspectionId == value) return;
                _incomingGoodsInspectionId = value;
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

        private bool _isApproved;
        public bool IsApproved
        {
            get { return _isApproved; }
            set
            {                
                _isApproved = value;
                OnPropertyChanged();
            }
        }

        private string _comments;
        public string Comments
        {
            get { return _comments; }
            set
            {
                if (_comments == value) return;
                _comments = value;
                OnPropertyChanged();
            }
        }

        private DateTime _inputDate;
        public DateTime InputDate
        {
            get { return _inputDate; }
            set
            {
                if (_inputDate == value) return;
                _inputDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _outputDate;
        public DateTime? OutputDate
        {
            get { return _outputDate; }
            set
            {
                if (_outputDate == value) return;
                _outputDate = value;
                OnPropertyChanged();
            }
        }

        private Guid _referredBy;
        public Guid ReferredBy
        {
            get { return _referredBy; }
            set
            {
                if (_referredBy == value) return;
                _referredBy = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(IncomingGoodsInspectionId))]
        public virtual IncomingGoodsInspection? IncomingGoodsInspection { get; set;  }
    }
}
