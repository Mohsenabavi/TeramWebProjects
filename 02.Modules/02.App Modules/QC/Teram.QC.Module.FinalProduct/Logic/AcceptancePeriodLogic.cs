using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;

namespace Teram.QC.Module.FinalProduct.Logic
{   
    public class AcceptancePeriodLogic : BusinessOperations<AcceptancePeriodModel, AcceptancePeriod, int>, IAcceptancePeriodLogic
    {
        public AcceptancePeriodLogic(IPersistenceService<AcceptancePeriod> service) : base(service)
        {

        }

        public BusinessOperationResult<AcceptancePeriodModel> GetByContrplPlanIdAndPeriod(int conplPlanId, long palletCount)
        {
            return GetFirst<AcceptancePeriodModel>(x => conplPlanId.Equals(conplPlanId) && (palletCount>=x.StartInterval && palletCount<=x.EndInterval));
        }
    }

}
