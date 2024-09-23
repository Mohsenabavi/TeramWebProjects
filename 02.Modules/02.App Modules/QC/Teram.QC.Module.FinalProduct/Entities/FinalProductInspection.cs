using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.FinalProduct.Entities
{
    [Table(nameof(FinalProductInspection) +"s", Schema = "QCFP")]
    public class FinalProductInspection:EntityBase
    {
        public int FinalProductInspectionId { get; set; }

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

       
        private string  _orderTitle;

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

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (_date == value) return;
                _date = value;
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

        private string _tracingCode;

        [StringLength(100)]
        public string TracingCode
        {
            get { return _tracingCode; }
            set
            {
                if (_tracingCode == value) return;
                _tracingCode = value;
                OnPropertyChanged();
            }
        }

        private long _startInterval;
        public long StartInterval
        {
            get { return _startInterval; }
            set
            {
                if (_startInterval == value) return;
                _startInterval = value;
                OnPropertyChanged();
            }
        }

        private long _endInterval;
        public long EndInterval
        {
            get { return _endInterval; }
            set
            {
                if (_endInterval == value) return;
                _endInterval = value;
                OnPropertyChanged();
            }
        }

        private int _sampleCount;
        public int SampleCount
        {
            get { return _sampleCount; }
            set
            {
                if (_sampleCount == value) return;
                _sampleCount = value;
                OnPropertyChanged();
            }
        }

        private long _totalCount;
        public long TotalCount
        {
            get { return _totalCount; }
            set
            {
                if (_totalCount == value) return;
                _totalCount = value;
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
        public virtual ICollection<FinalProductInspectionDefect>? FinalProductInspectionDefects { get; set; }
    }
}
