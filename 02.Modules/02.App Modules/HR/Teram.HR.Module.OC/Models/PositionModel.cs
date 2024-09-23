using Teram.Framework.Core.Logic;
using Teram.Web.Core.Attributes;
using Position = Teram.HR.Module.OC.Entities.Position;

namespace Teram.HR.Module.OC.Models
{
    public class PositionModel:ModelBase<Position,int> 
    {
        public int PositionId { get; set; }

        [GridColumn(nameof(Title))]
        public string Title {  get; set; }
    }
}
