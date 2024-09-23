using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.FinalProduct.Entities
{

    [Table(nameof(FinalProductNoncomplianceDetail) +"s", Schema = "QCFP")]
    public class FinalProductNoncomplianceDetail :EntityBase
    {
        public int FinalProductNoncomplianceDetailId { get; set; }       

        private int _number;
        public int Number
        {
            get { return _number; }
            set
            {
                if (_number == value) return;
                _number = value;
                OnPropertyChanged();
            }
        }

        private Guid _createdBy;
        public Guid CreatedBy
        {
            get { return _createdBy; }
            set
            {
                if (_createdBy == value) return;
                _createdBy = value;
                OnPropertyChanged();
            }
        }

        private DateTime _createdDate;
        public DateTime CreateDate
        {
            get { return _createdDate; }
            set
            {
                if (_createdDate == value) return;
                _createdDate = value;
                OnPropertyChanged();
            }
        }

        private int? _finalProductInspectionId;
        public int? FinalProductInspectionId
        {
            get { return _finalProductInspectionId; }
            set
            {
                if (_finalProductInspectionId == value) return;
                _finalProductInspectionId = value;
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
        public virtual ICollection<FinalProductNoncomplianceDetailSample> FinalProductNoncomplianceDetailSamples { get; set; }

    }
}
