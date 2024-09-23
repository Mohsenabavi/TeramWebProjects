using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.FinalProduct.Entities
{
    [Table(nameof(ControlPlanDefect) +"s", Schema = "QCFP")]
    public class ControlPlanDefect:EntityBase
    {
        public int ControlPlanDefectId { get; set; }

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


        private int _qCDefectId;
        public int QCDefectId
        {
            get { return _qCDefectId; }
            set
            {
                if (_qCDefectId == value) return;
                _qCDefectId = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(QCDefectId))]
        public virtual QCDefect QCDefect { get; set; }


        private string _controlPlanDefectVal;

        [StringLength(20)]
        public string ControlPlanDefectVal
        {
            get { return _controlPlanDefectVal; }
            set
            {
                if (_controlPlanDefectVal == value) return;
                _controlPlanDefectVal = value;
                OnPropertyChanged();
            }
        }
        public virtual ICollection<FinalProductInspectionDefect>? FinalProductInspectionDefects { get; set; }
        public virtual ICollection<FinalProductNoncompliance>? FinalProductNoncompliances { get; set; }
    }
}
