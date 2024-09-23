using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Models;

namespace Teram.QC.Module.FinalProduct.Logic.Interfaces
{   
    public interface IQCControlPlanLogic : IBusinessOperations<QCControlPlanModel, QCControlPlan, int>
    {
        BusinessOperationResult<QCControlPlanModel> GetByTitle(string title);
    }

}
