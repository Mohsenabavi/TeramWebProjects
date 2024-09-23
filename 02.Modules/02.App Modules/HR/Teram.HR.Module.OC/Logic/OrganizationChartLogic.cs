using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.OC.Entities;
using Teram.HR.Module.OC.Logic.Interface;
using Teram.HR.Module.OC.Models;

namespace Teram.HR.Module.OC.Logic
{ 
    public class OrganizationChartLogic : BusinessOperations<OrganizationChartModel, OrganizationChart, int>, IOrganizationChartLogic
    {
        public OrganizationChartLogic(IPersistenceService<OrganizationChart> service) : base(service)
        {

        }

        public List<JsTreeNode> ConvertToJsTreeFormat(List<OrganizationChartModel> organizationChartData)
        {
            var treeData = organizationChartData.Select(node => new JsTreeNode
            {
                id = node.OrganizationChartId.ToString(),
                parent = node.ParentOrganizationChartId != null ? node.ParentOrganizationChartId.ToString() : "#",
                text = $"{node.FirstName} {node.LastName} ( {node.PositionTitle} )"
            }).ToList();

            return treeData;
        }

        public List<JsTreeNode> GetOrganizationChartData()
        {
            var organizationChartData = GetAll();
            return ConvertToJsTreeFormat(organizationChartData.ResultEntity);

        }
    }

}
