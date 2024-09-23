using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.QC.Module.IncomingGoods.Enums;

namespace Teram.QC.Module.IncomingGoods.Entities
{
    [Table(nameof(IncomingGoodsInspectionItem)+"s", Schema = "QC")]
    public class IncomingGoodsInspectionItem:EntityBase
    {
        public int IncomingGoodsInspectionItemId { get; set; }

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

        [ForeignKey(nameof(IncomingGoodsInspectionId))]
        public virtual IncomingGoodsInspection IncomingGoodsInspection { get; set; }

        private int _controlPlanId;
        public int ControlPlanId
        {
            get { return _controlPlanId; }
            set
            {
                if (_controlPlanId == value) return;
                _controlPlanId = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(ControlPlanId))]
        public virtual ControlPlan ControlPlan { get; set; }

        private string? _inspectionResult;
        public string? InspectionResult
        {
            get { return _inspectionResult; }
            set
            {
                if (_inspectionResult == value) return;
                _inspectionResult = value;
                OnPropertyChanged();
            }
        }

        private bool _isMatch;
        public bool IsMatch
        {
            get { return _isMatch; }
            set
            {
                if (_isMatch == value) return;
                _isMatch = value;
                OnPropertyChanged();
            }
        }

        private string? _inspectionResultRemarks;
        public string? InspectionResultRemarks
        {
            get { return _inspectionResultRemarks; }
            set
            {
                if (_inspectionResultRemarks == value) return;
                _inspectionResultRemarks = value;
                OnPropertyChanged();
            }
        }

        private int? _amountOfDefects;
        public int? AmountOfDefects
        {
            get { return _amountOfDefects; }
            set
            {
                _amountOfDefects = value;
                OnPropertyChanged();
            }
        }       
    }
}
