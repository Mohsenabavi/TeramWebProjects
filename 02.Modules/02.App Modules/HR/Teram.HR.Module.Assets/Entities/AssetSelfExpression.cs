using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.HR.Module.Assets.Entities
{

    [Table(nameof(AssetSelfExpression) + "s", Schema = "Asset")]
    public class AssetSelfExpression :EntityBase
    {
        public int AssetSelfExpressionId { get; set; }


        private string _assetID;
        public string AssetID
        {
            get { return _assetID; }
            set
            {
                if (_assetID == value) return;
                _assetID = value;
                OnPropertyChanged();
            }
        }

        private string _plaqueNumber;
        public string PlaqueNumber
        {
            get { return _plaqueNumber; }
            set
            {
                if (_plaqueNumber == value) return;
                _plaqueNumber = value;
                OnPropertyChanged();
            }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                if (_code == value) return;
                _code = value;
                OnPropertyChanged();
            }
        }

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

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName == value) return;
                _userName = value;
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

    }
}
