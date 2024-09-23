using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.FinalProduct.Entities
{
    [Table(nameof(FinalProductNonComplianceCartableItem) +"s", Schema = "QCFP")]
    public class FinalProductNonComplianceCartableItem : EntityBase
    {
        public int FinalProductNonComplianceCartableItemId { get; set; }


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

        private int _finalProductNoncomplianceId;
        public int FinalProductNoncomplianceId
        {
            get { return _finalProductNoncomplianceId; }
            set
            {
                if (_finalProductNoncomplianceId == value) return;
                _finalProductNoncomplianceId = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(FinalProductNoncomplianceId))]
        public virtual FinalProductNoncompliance FinalProductNoncompliance { get; set; }       
    }
}
