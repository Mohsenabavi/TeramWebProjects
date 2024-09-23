using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.IncomingGoods.Entities
{
    [Table(nameof(ControlPlan)+"s", Schema = "QC")]
    public class ControlPlan : EntityBase
    {
        public int ControlPlanId { get; set; }

        private string _controlPlanParameter;
        public string ControlPlanParameter
        {
            get { return _controlPlanParameter; }
            set
            {
                if (_controlPlanParameter == value) return;
                _controlPlanParameter = value;
                OnPropertyChanged();
            }
        }

        private string _quantityDescription;
        public string QuantityDescription
        {
            get { return _quantityDescription; }
            set
            {
                if (_quantityDescription == value) return;
                _quantityDescription = value;
                OnPropertyChanged();
            }
        }

        private string? _acceptanceCriteria;
        public string? AcceptanceCriteria
        {
            get { return _acceptanceCriteria; }
            set
            {
                if (_acceptanceCriteria == value) return;
                _acceptanceCriteria = value;
                OnPropertyChanged();
            }
        }

        private int _controlPlanCategoryId;
        public int ControlPlanCategoryId
        {
            get { return _controlPlanCategoryId; }
            set
            {
                if (_controlPlanCategoryId == value) return;
                _controlPlanCategoryId = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(ControlPlanCategoryId))]
        public virtual ControlPlanCategory ControlPlanCategory { get; set; }
        public virtual ICollection<IncomingGoodsInspectionItem> IncomingGoodsInspectionItem { get; set; }
    }
}
