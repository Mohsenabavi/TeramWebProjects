using Teram.Framework.Core.Logic;
using Teram.HR.Module.OC.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.OC.Models
{
    public class OrganizationChartModel : ModelBase<OrganizationChart, int>
    {
        [GridColumn("ID")]
        public int OrganizationChartId { get; set; }

        public int? ParentOrganizationChartId { get; set; }

        [GridColumn(nameof(PersonnelCode))]

        public string PersonnelCode { get; set; } = string.Empty;


        [GridColumn(nameof(FirstName))]
        public string FirstName { get; set; } = string.Empty;


        [GridColumn(nameof(LastName))]
        public string LastName { get; set; } = string.Empty;

        public int PositionId {  get; set; }

        public Guid UserId {  get; set; }


        public string PositionTitle {  get; set; }

    }
}
