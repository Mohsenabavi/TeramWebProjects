using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.QC.Module.FinalProduct.Enums;

namespace Teram.QC.Module.FinalProduct.Entities
{
    [Table(nameof(FinalProductNoncomplianceDetailSample) +"s", Schema = "QCFP")]
    public class FinalProductNoncomplianceDetailSample :EntityBase
    {
        public int FinalProductNoncomplianceDetailSampleId { get; set; }

        private SampleType _sampleType;
        public SampleType SampleType
        {
            get { return _sampleType; }
            set
            {
                if (_sampleType == value) return;
                _sampleType = value;
                OnPropertyChanged();
            }
        }

        private int _amount;
        public int Amount
        {
            get { return _amount; }
            set
            {               
                _amount = value;
                OnPropertyChanged();
            }
        }

        private int _finalProductNoncomplianceDetailId;
        public int FinalProductNoncomplianceDetailId
        {
            get { return _finalProductNoncomplianceDetailId; }
            set
            {
                if (_finalProductNoncomplianceDetailId == value) return;
                _finalProductNoncomplianceDetailId = value;
                OnPropertyChanged();
            }
        }

        private OpinionType _opinionTypeQCManager;
        public OpinionType OpinionTypeQCManager
        {
            get { return _opinionTypeQCManager; }
            set
            {
                if (_opinionTypeQCManager == value) return;
                _opinionTypeQCManager = value;
                OnPropertyChanged();
            }
        }

        private OpinionType _opinionTypeCEO;
        public OpinionType OpinionTypeCEO
        {
            get { return _opinionTypeCEO; }
            set
            {
                if (_opinionTypeCEO == value) return;
                _opinionTypeCEO = value;
                OnPropertyChanged();
            }
        }

        private OpinionType _opinionTypeCEOFinal;
        public OpinionType OpinionTypeCEOFinal
        {
            get { return _opinionTypeCEOFinal; }
            set
            {
                if (_opinionTypeCEOFinal == value) return;
                _opinionTypeCEOFinal = value;
                OnPropertyChanged();
            }
        }



        private int _separatedCount;
        public int SeparatedCount
        {
            get { return _separatedCount; }
            set
            {
                if (_separatedCount == value) return;
                _separatedCount = value;
                OnPropertyChanged();
            }
        }

        private int _wasteCount;
        public int WasteCount
        {
            get { return _wasteCount; }
            set
            {
                if (_wasteCount == value) return;
                _wasteCount = value;
                OnPropertyChanged();
            }
        }

        private string? _wasteDocumentNumber;
        public string? WasteDocumentNumber
        {
            get { return _wasteDocumentNumber; }
            set
            {
                if (_wasteDocumentNumber == value) return;
                _wasteDocumentNumber = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(FinalProductNoncomplianceDetailId))]
        public virtual FinalProductNoncomplianceDetail FinalProductNoncomplianceDetail { get; set; }        
    }
}
