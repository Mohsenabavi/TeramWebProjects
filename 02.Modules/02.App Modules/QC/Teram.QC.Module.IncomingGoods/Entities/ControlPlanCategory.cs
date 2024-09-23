using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.IncomingGoods.Entities
{
    [Table("ControlPlanCategories", Schema = "QC")]
    public class ControlPlanCategory : EntityBase
    {
        public int ControlPlanCategoryId { get; set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
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

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
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
        public virtual ICollection<ControlPlan> ControlPlans { get; set; }
    }
}
