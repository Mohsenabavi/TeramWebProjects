using Teram.Framework.Core.Logic;
using Teram.HR.Module.TicketRegister.Entities;

namespace Teram.HR.Module.TicketRegister.Models
{
    public class AreaModel :ModelBase<Area,int>
    {
        public int AreaId { get; set; }

        public string Title { get; set; }

        public int Capacity { get; set; }
        public List<AreaRowModel>? AreaRows { get; set; }
    }
}
