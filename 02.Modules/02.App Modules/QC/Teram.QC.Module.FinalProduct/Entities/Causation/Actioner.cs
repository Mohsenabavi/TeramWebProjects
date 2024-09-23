using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.QC.Module.FinalProduct.Entities.Causation
{
    [Table(nameof(Actioner) + "s", Schema = "QCFP")]
    public class Actioner : EntityBase
    {
        public int ActionerId { get; set; }

        private string _firstName;

        [StringLength(100)]
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName == value) return;
                _firstName = value;
                OnPropertyChanged();
            }
        }

        private string _lastName;

        [StringLength(100)]
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName == value) return;
                _lastName = value;
                OnPropertyChanged();
            }
        }

        private string? _personnelCode;

        [StringLength(20)]
        public string? PersonnelCode
        {
            get { return _personnelCode; }
            set
            {
                if (_personnelCode == value) return;
                _personnelCode = value;
                OnPropertyChanged();
            }
        }

        private string? _postTitle;
        public string? PostTitle
        {
            get { return _postTitle; }
            set
            {
                if (_postTitle == value) return;
                _postTitle = value;
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

        private Guid? _userId;
        public Guid? UserId
        {
            get { return _userId; }
            set
            {
                if (_userId == value) return;
                _userId = value;
                OnPropertyChanged();
            }
        }
        public virtual ICollection<CorrectiveAction>? CorrectiveActions { get; set; }
        public virtual ICollection<Causation>? Causations { get; set; }
    }
}
