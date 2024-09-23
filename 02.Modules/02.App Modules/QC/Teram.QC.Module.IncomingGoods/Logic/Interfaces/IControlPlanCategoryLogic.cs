using Teram.Framework.Core.Logic;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Models;

namespace Teram.QC.Module.IncomingGoods.Logic.Interfaces
{  
    public interface IControlPlanCategoryLogic : IBusinessOperations<ControlPlanCategoryModel, ControlPlanCategory, int>
    {
        BusinessOperationResult<ControlPlanCategoryModel> GetByTitle(string titleL);
    }

}
