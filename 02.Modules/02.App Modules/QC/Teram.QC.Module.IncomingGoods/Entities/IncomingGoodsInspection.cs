using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.QC.Module.IncomingGoods.Enums;

namespace Teram.QC.Module.IncomingGoods.Entities
{

    [Table(nameof(IncomingGoodsInspection)+"s", Schema = "QC")]
    public class IncomingGoodsInspection : EntityBase
    {
        public int IncomingGoodsInspectionId { get; set; }

        private string _qualityInspectionNumber;
        public string QualityInspectionNumber
        {
            get { return _qualityInspectionNumber; }
            set
            {
                if (_qualityInspectionNumber == value) return;
                _qualityInspectionNumber = value;
                OnPropertyChanged();
            }
        }

        private string _goodsTitle;
        public string GoodsTitle
        {
            get { return _goodsTitle; }
            set
            {
                if (_goodsTitle == value) return;
                _goodsTitle = value;
                OnPropertyChanged();
            }
        }

        private decimal? _quantity;
        public decimal? Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity == value) return;
                _quantity = value;
                OnPropertyChanged();
            }
        }

        private string _goodsCode;
        public string GoodsCode
        {
            get { return _goodsCode; }
            set
            {
                if (_goodsCode == value) return;
                _goodsCode = value;
                OnPropertyChanged();
            }
        }


        private string _vendorName;
        public string VendorName
        {
            get { return _vendorName; }
            set
            {
                if (_vendorName == value) return;
                _vendorName = value;
                OnPropertyChanged();
            }
        }

        private bool _isConsignment;
        public bool IsConsignment
        {
            get { return _isConsignment; }
            set
            {
                _isConsignment = value;
                OnPropertyChanged();
            }
        }

        private bool _isSampleGoods;
        public bool IsSampleGoods
        {
            get { return _isSampleGoods; }
            set
            {
                _isSampleGoods = value;
                OnPropertyChanged();
            }
        }

        private bool _goodsInfoIsApproved;
        public bool GoodsInfoIsApproved
        {
            get { return _goodsInfoIsApproved; }
            set
            {
                _goodsInfoIsApproved = value;
                OnPropertyChanged();
            }
        }

        private bool _hasFunctionalTest;
        public bool HasFunctionalTest
        {
            get { return _hasFunctionalTest; }
            set
            {
                _hasFunctionalTest = value;
                OnPropertyChanged();
            }
        }

        private DateTime _createDate;
        public DateTime CreateDate
        {
            get { return _createDate; }
            set
            {
                if (_createDate == value) return;
                _createDate = value;
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

        private InspectionFormStatus _inspectionFormStatus;
        public InspectionFormStatus InspectionFormStatus
        {
            get { return _inspectionFormStatus; }
            set
            {
                if (_inspectionFormStatus == value) return;
                _inspectionFormStatus = value;
                OnPropertyChanged();
            }
        }

        private int _totalDefectiveCount;
        public int TotalDefectiveCount
        {
            get { return _totalDefectiveCount; }
            set
            {
                if (_totalDefectiveCount == value) return;
                _totalDefectiveCount = value;
                OnPropertyChanged();
            }
        }

        private bool _needToRefferal;
        public bool NeedToRefferal
        {
            get { return _needToRefferal; }
            set
            {                
                _needToRefferal = value;
                OnPropertyChanged();
            }
        }

        private bool? _finalApprove;
        public bool? FinalApprove
        {
            get { return _finalApprove; }
            set
            {
                if (_finalApprove == value) return;
                _finalApprove = value;
                OnPropertyChanged();
            }
        }

        public virtual ICollection<IncomingGoodsInspectionItem>? IncomingGoodsInspectionItems { get; set; }
        public virtual ICollection<IncomingGoodsInspectionFile>? IncomingGoodsInspectionFiles { get; set; }
        public virtual ICollection<IncomingGoodsInspectionCartableItem>? IncomingGoodsInspectionCartableItems { get; set; }
 
    }
}
