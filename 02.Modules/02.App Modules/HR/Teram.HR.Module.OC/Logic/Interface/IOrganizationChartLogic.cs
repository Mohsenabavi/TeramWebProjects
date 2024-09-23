using Teram.Framework.Core.Logic;
using Teram.HR.Module.OC.Entities;
using Teram.HR.Module.OC.Models;

namespace Teram.HR.Module.OC.Logic.Interface
{ 
    public interface IOrganizationChartLogic : IBusinessOperations<OrganizationChartModel, OrganizationChart, int>
    {
        public List<JsTreeNode> GetOrganizationChartData();
        public List<JsTreeNode> ConvertToJsTreeFormat(List<OrganizationChartModel> organizationChartData);
    }

}
