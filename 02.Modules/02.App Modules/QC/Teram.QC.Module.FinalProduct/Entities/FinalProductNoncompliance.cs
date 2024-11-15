using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.QC.Module.FinalProduct.Attributes;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Enums;

namespace Teram.QC.Module.FinalProduct.Entities
{
    [Table(nameof(FinalProductNoncompliance) + "s", Schema = "QCFP")]
    public class FinalProductNoncompliance : EntityBase
    {
        public int FinalProductNoncomplianceId { get; set; }

        private string _finalProductNoncomplianceNumber;

        [StringLength(6)]
        public string FinalProductNoncomplianceNumber
        {
            get { return _finalProductNoncomplianceNumber; }
            set
            {
                if (_finalProductNoncomplianceNumber == value) return;
                _finalProductNoncomplianceNumber = value;
                OnPropertyChanged();
            }
        }

        private int _orderNo;
        public int OrderNo
        {
            get { return _orderNo; }
            set
            {
                if (_orderNo == value) return;
                _orderNo = value;
                OnPropertyChanged();
            }
        }

        private string _orderTitle;

        [StringLength(300)]
        public string OrderTitle
        {
            get { return _orderTitle; }
            set
            {
                if (_orderTitle == value) return;
                _orderTitle = value;
                OnPropertyChanged();
            }
        }

        private string _productCode;

        [StringLength(50)]
        public string ProductCode
        {
            get { return _productCode; }
            set
            {
                if (_productCode == value) return;
                _productCode = value;
                OnPropertyChanged();
            }
        }

        private string _productName;


        [StringLength(300)]

        public string ProductName
        {
            get { return _productName; }
            set
            {
                if (_productName == value) return;
                _productName = value;
                OnPropertyChanged();
            }
        }

        private string _controlPlan;

        [StringLength(50)]
        public string ControlPlan
        {
            get { return _controlPlan; }
            set
            {
                if (_controlPlan == value) return;
                _controlPlan = value;
                OnPropertyChanged();
            }
        }

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


        private string? _remarks;
        public string? Remarks
        {
            get { return _remarks; }
            set
            {
                if (_remarks == value) return;
                _remarks = value;
                OnPropertyChanged();
            }
        }

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

        private FinalProductNoncomplianceType _finalProductNoncomplianceType;
        public FinalProductNoncomplianceType FinalProductNoncomplianceType
        {
            get { return _finalProductNoncomplianceType; }
            set
            {
                if (_finalProductNoncomplianceType == value) return;
                _finalProductNoncomplianceType = value;
                OnPropertyChanged();
            }
        }

        private bool? _isApproved;

        [CompareField]
        [Description("تاییده شده")]
        public bool? IsApproved
        {
            get { return _isApproved; }
            set
            {
                _isApproved = value;
                OnPropertyChanged();
            }
        }

        private bool? _isVoided;
        [CompareField]
        [Description("باطل شده")]
        public bool? IsVoided
        {
            get { return _isVoided; }
            set
            {
                _isVoided = value;
                OnPropertyChanged();
            }
        }

        private bool? _needToAdvisoryOpinion;

        [CompareField]
        [Description("نیاز به نظر مشورتی")]
        public bool? NeedToAdvisoryOpinion
        {
            get { return _needToAdvisoryOpinion; }
            set
            {
                _needToAdvisoryOpinion = value;
                OnPropertyChanged();
            }
        }

        private bool? _needToRefferToCEO;

        [CompareField]

        [Description("نیاز به نظر مدیرعامل")]
        public bool? NeedToRefferToCEO
        {
            get { return _needToRefferToCEO; }
            set
            {
                _needToRefferToCEO = value;
                OnPropertyChanged();
            }
        }

        private bool? _hasSeperationOrder;

        [CompareField]
        [Description("دستور جداسازی")]
        public bool? HasSeperationOrder
        {
            get { return _hasSeperationOrder; }
            set
            {
                _hasSeperationOrder = value;
                OnPropertyChanged();
            }
        }

        private bool? _isSeperated;

        [CompareField]

        [Description("جداسازی شده")]
        public bool? IsSeperated
        {
            get { return _isSeperated; }
            set
            {
                _isSeperated = value;
                OnPropertyChanged();
            }
        }

        private bool? _needToCkeckByOther;

        [CompareField]
        [Description("نیاز به بررسی اشخاص دیگر")]
        public bool? NeedToCkeckByOther
        {
            get { return _needToCkeckByOther; }
            set
            {
                _needToCkeckByOther = value;
                OnPropertyChanged();
            }
        }

        private bool? _hasFinalResult;

        [CompareField]

        [Description("تعیین تکلیف نهایی شده")]
        public bool? HasFinalResult
        {
            get { return _hasFinalResult; }
            set
            {
                _hasFinalResult = value;
                OnPropertyChanged();
            }
        }

        private bool? _hasWasteOrder;

        [CompareField]

        [Description("دستور ضایعات دارد")]
        public bool? HasWasteOrder
        {
            get { return _hasWasteOrder; }
            set
            {
                _hasWasteOrder = value;
                OnPropertyChanged();
            }
        }

        private QualityControlManagerOpinion _qualityControlManagerOpinion;
        public QualityControlManagerOpinion QualityControlManagerOpinion
        {
            get { return _qualityControlManagerOpinion; }
            set
            {
                if (_qualityControlManagerOpinion == value) return;
                _qualityControlManagerOpinion = value;
                OnPropertyChanged();
            }
        }

        private ReferralStatus _referralStatus;
        public ReferralStatus ReferralStatus
        {
            get { return _referralStatus; }
            set
            {
                _referralStatus = value;
                OnPropertyChanged();
            }
        }

        private FormStatus _formStatus;
        public FormStatus FormStatus
        {
            get { return _formStatus; }
            set
            {
                _formStatus = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(ControlPlanDefectId))]

        public virtual ControlPlanDefect ControlPlanDefect { get; set; }


        private DateTime? _lastModifyDate;
        public DateTime? LastModifyDate
        {
            get { return _lastModifyDate; }
            set
            {
                if (_lastModifyDate == value) return;
                _lastModifyDate = value;
                OnPropertyChanged();
            }
        }

        private string? _lastComment;
        public string? LastComment
        {
            get { return _lastComment; }
            set
            {
                if (_lastComment == value) return;
                _lastComment = value;
                OnPropertyChanged();
            }
        }

        private bool? _hasLeniency;

        [CompareField]

        [Description("مجوز ارفاقی دارد")]
        public bool? HasLeniency
        {
            get { return _hasLeniency; }
            set
            {
                _hasLeniency = value;
                OnPropertyChanged();
            }
        }


        private bool _hasCausation;

        [CompareField]

        [Description("علت یابی شده")]
        public bool HasCausation
        {
            get { return _hasCausation; }
            set
            {
                _hasCausation = value;
                OnPropertyChanged();
            }
        }

        private bool _isTriggeredByUserAction;
        public bool IsTriggeredByUserAction
        {
            get { return _isTriggeredByUserAction; }
            set
            {
                _isTriggeredByUserAction = value;
                OnPropertyChanged();
            }
        }

        private Guid? _destinationUser;
        public Guid? DestinationUser
        {
            get { return _destinationUser; }
            set
            {
                _destinationUser = value;
                OnPropertyChanged();
            }
        }

        private bool _finalApproveByQA;
        public bool FinalApproveByQA
        {
            get { return _finalApproveByQA; }
            set
            {
                _finalApproveByQA = value;
                OnPropertyChanged();
            }
        }

        private DateTime _finalApproveByQADate;
        public DateTime FinalApproveByQADate
        {
            get { return _finalApproveByQADate; }
            set
            {
                if (_finalApproveByQADate == value) return;
                _finalApproveByQADate = value;
                OnPropertyChanged();
            }
        }
        public virtual Entities.Causation.Causation Causation { get; set; }
        public virtual ICollection<FinalProductNoncomplianceDetail>? FinalProductNoncomplianceDetails { get; set; }
        public virtual ICollection<FinalProductNoncomplianceFile>? FinalProductNoncomplianceFiles { get; set; }
        public virtual ICollection<FinalProductNonComplianceCartableItem>? FinalProductNonComplianceCartableItems { get; set; }

    }
}
