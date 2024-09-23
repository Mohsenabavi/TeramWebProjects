using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.FinalProduct.Entities
{
    [Table(nameof(AcceptancePeriod) +"s", Schema = "QCFP")]
    public class AcceptancePeriod:EntityBase
    {
        public int AcceptancePeriodId { get; set; }

        private int _qCControlPlanId;
        public int QCControlPlanId
        {
            get { return _qCControlPlanId; }
            set
            {
                if (_qCControlPlanId == value) return;
                _qCControlPlanId = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(QCControlPlanId))]
        public virtual QCControlPlan QCControlPlan { get; set; }

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

        private string _a;
        [StringLength(10)]
        public string A
        {
            get { return _a; }
            set
            {
                if (_a == value) return;
                _a = value;
                OnPropertyChanged();
            }
        }

        private string _total;

        [StringLength(10)]
        public string Total
        {
            get { return _total; }
            set
            {
                if (_total == value) return;
                _total = value;
                OnPropertyChanged();
            }
        }
    }
}
