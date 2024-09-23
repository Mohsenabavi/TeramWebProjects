using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Logic.Interfaces;
using Teram.QC.Module.IncomingGoods.Models;

namespace Teram.QC.Module.IncomingGoods.Logic
{
    public class ControlPlanCategoryLogic : BusinessOperations<ControlPlanCategoryModel, ControlPlanCategory, int>, IControlPlanCategoryLogic
    {
        public ControlPlanCategoryLogic(IPersistenceService<ControlPlanCategory> service) : base(service)
        {

        }

        public BusinessOperationResult<ControlPlanCategoryModel> GetByTitle(string title)
        {
            return GetFirst<ControlPlanCategoryModel>(x => x.Title==title);
        }
    }

}
