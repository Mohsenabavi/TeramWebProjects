using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Models;

namespace Teram.QC.Module.FinalProduct.Logic.Interfaces
{
    public interface IAcceptancePeriodLogic : IBusinessOperations<AcceptancePeriodModel, AcceptancePeriod, int>
    {
        BusinessOperationResult<AcceptancePeriodModel> GetByContrplPlanIdAndPeriod(int conplPlanId,long palletCount);
    }

}
