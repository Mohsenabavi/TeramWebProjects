using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.FinalProduct.Entities
{
    [Table(nameof(FinalProductInspectionDefect) +"s", Schema = "QCFP")]
    public class FinalProductInspectionDefect : EntityBase
    {
        public int FinalProductInspectionDefectId { get; set; }

        private int _finalProductInspectionId;
        public int FinalProductInspectionId
        {
            get { return _finalProductInspectionId; }
            set
            {
                if (_finalProductInspectionId == value) return;
                _finalProductInspectionId = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(FinalProductInspectionId))]
        public virtual FinalProductInspection? FinalProductInspection { get; set; }


        private int _controlPlanDefectId;
        public int ControlPlanDefectId
        {
            get { return _controlPlanDefectId; }
            set
            {
                if (_controlPlanDefectId == value) return;
                _controlPlanDefectId = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(ControlPlanDefectId))]
        public virtual ControlPlanDefect ControlPlanDefect { get; set; }


        private int _firstSample;
        public int FirstSample
        {
            get { return _firstSample; }
            set
            {
                if (_firstSample == value) return;
                _firstSample = value;
                OnPropertyChanged();
            }
        }

        private int _secondSample;
        public int SecondSample
        {
            get { return _secondSample; }
            set
            {
                if (_secondSample == value) return;
                _secondSample = value;
                OnPropertyChanged();
            }
        }

        private int _thirdSample;
        public int ThirdSample
        {
            get { return _thirdSample; }
            set
            {
                if (_thirdSample == value) return;
                _thirdSample = value;
                OnPropertyChanged();
            }
        }

        private int _forthSample;
        public int ForthSample
        {
            get { return _forthSample; }
            set
            {
                if (_forthSample == value) return;
                _forthSample = value;
                OnPropertyChanged();
            }
        }

    }
}
