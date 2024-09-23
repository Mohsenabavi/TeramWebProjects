using Teram.Framework.Core.Logic;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Models;

namespace Teram.QC.Module.IncomingGoods.Logic.Interfaces
{

    public interface IControlPlanLogic : IBusinessOperations<ControlPlanModel, ControlPlan, int>
    {
        BusinessOperationResult<List<ControlPlanModel>> GetByCategoryId(int categoryId);
        BusinessOperationResult<List<ControlPlanModel>> GetByFilter(int? controlPlanCategoryId, int? start = null, int? length = null);
    }

}
