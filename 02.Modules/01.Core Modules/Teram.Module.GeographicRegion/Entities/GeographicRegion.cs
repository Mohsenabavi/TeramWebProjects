using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Teram.Framework.Core.Domain;
using Teram.Module.GeographicRegion.Enums;

namespace Teram.Module.GeographicRegion.Entities
{
    [Table(nameof(GeographicRegion) + "s", Schema = "Geo")]
    public class GeographicRegion :EntityBase
    {
        public int GeographicRegionId { get; set; }

        private int? _parentGeographicRegionId;
        public int? ParentGeographicRegionId
        {
            get { return _parentGeographicRegionId; }
            set
            {
                if (_parentGeographicRegionId == value) return;
                _parentGeographicRegionId = value;
                OnPropertyChanged();
            }
        }
        private GeographicType _geographicType;
        [Column("GeographicRegionTypeId")]
        public GeographicType GeographicType
        {
            get { return _geographicType; }
            set
            {
                if (_geographicType == value) return;
                _geographicType = value;
                OnPropertyChanged();
            }
        }
        private int? _code;
        public int? Code
        {
            get { return _code; }
            set
            {
                if (_code == value) return;
                _code = value;
                OnPropertyChanged();
            }
        }
        private string _name;
        [StringLength(50)]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }
        private string _latinName;
        [StringLength(50)]
        public string LatinName
        {
            get { return _latinName; }
            set
            {
                if (_latinName == value) return;
                _latinName = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(ParentGeographicRegionId))]
        public virtual GeographicRegion Parent { get; set; }
        public virtual ICollection<GeographicRegion> Children { get; set; }
    }
}
