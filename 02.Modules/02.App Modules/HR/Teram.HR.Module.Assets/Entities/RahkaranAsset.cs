using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;

namespace Teram.HR.Module.Assets.Entities
{

    [Table(nameof(RahkaranAsset) + "s", Schema = "Rahkaran")]
    public class RahkaranAsset : EntityBase
    {
        public int RahkaranAssetId { get; set; }

        private long _assetID;
        public long AssetID
        {
            get { return _assetID; }
            set
            {
                if (_assetID == value) return;
                _assetID = value;
                OnPropertyChanged();
            }
        }

        private string? _PlaqueNumber;
        public string? PlaqueNumber
        {
            get { return _PlaqueNumber; }
            set
            {
                if (_PlaqueNumber == value) return;
                _PlaqueNumber = value;
                OnPropertyChanged();
            }
        }

        private string? _title;
        public string? Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _utilizeDate;
        public DateTime? UtilizeDate
        {
            get { return _utilizeDate; }
            set
            {
                if (_utilizeDate == value) return;
                _utilizeDate = value;
                OnPropertyChanged();
            }
        }

        private string? _utilizationDate;
        public string? UtilizationDate
        {
            get { return _utilizationDate; }
            set
            {
                if (_utilizationDate == value) return;
                _utilizationDate = value;
                OnPropertyChanged();
            }
        }

        private string? _groupTitle;
        public string? GroupTitle
        {
            get { return _groupTitle; }
            set
            {
                if (_groupTitle == value) return;
                _groupTitle = value;
                OnPropertyChanged();
            }
        }

        private string? _depreciatedMethodTitle;
        public string? DepreciatedMethodTitle
        {
            get { return _depreciatedMethodTitle; }
            set
            {
                if (_depreciatedMethodTitle == value) return;
                _depreciatedMethodTitle = value;
                OnPropertyChanged();
            }
        }

        private string? _costCenter;
        public string? CostCenter
        {
            get { return _costCenter; }
            set
            {
                if (_costCenter == value) return;
                _costCenter = value;
                OnPropertyChanged();
            }
        }

        private string? _settlementPlace;
        public string? SettlementPlace
        {
            get { return _settlementPlace; }
            set
            {
                if (_settlementPlace == value) return;
                _settlementPlace = value;
                OnPropertyChanged();
            }
        }

        private string? _collector;
        public string? Collector
        {
            get { return _collector; }
            set
            {
                if (_collector == value) return;
                _collector = value;
                OnPropertyChanged();
            }
        }

        private string? _fullName;
        public string? FullName
        {
            get { return _fullName; }
            set
            {
                if (_fullName == value) return;
                _fullName = value;
                OnPropertyChanged();
            }
        }

        private decimal? _cost;
        public decimal? Cost
        {
            get { return _cost; }
            set
            {
                if (_cost == value) return;
                _cost = value;
                OnPropertyChanged();
            }
        }

        private string? _code;
        public string? Code
        {
            get { return _code; }
            set
            {
                if (_code == value) return;
                _code = value;
                OnPropertyChanged();
            }
        }

        private int? _personnelCode;
        public int? PersonnelCode
        {
            get { return _personnelCode; }
            set
            {
                if (_personnelCode == value) return;
                _personnelCode = value;
                OnPropertyChanged();
            }
        }


        private Guid? _approvedBy;
        public Guid? ApprovedBy
        {
            get { return _approvedBy; }
            set
            {
                if (_approvedBy == value) return;
                _approvedBy = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _approveDate;
        public DateTime? ApproveDate
        {
            get { return _approveDate; }
            set
            {
                if (_approveDate == value) return;
                _approveDate = value;
                OnPropertyChanged();
            }
        }

        private bool? _approveStatus;
        public bool? ApproveStatus
        {
            get { return _approveStatus; }
            set
            {
                if (_approveStatus == value) return;
                _approveStatus = value;
                OnPropertyChanged();
            }
        }

        private string? _approverRemarks;
        public string? ApproverRemarks
        {
            get { return _approverRemarks; }
            set
            {
                if (_approverRemarks == value) return;
                _approverRemarks = value;
                OnPropertyChanged();
            }
        }

        private string? _nationalId;
        public string? NationalID
        {
            get { return _nationalId; }
            set
            {
                if (_nationalId == value) return;
                _nationalId = value;
                OnPropertyChanged();
            }
        }

    }
}
