using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models.CausationModels;

namespace Teram.QC.Module.FinalProduct.Logic
{  
    public class RootCauseLogic : BusinessOperations<RootCauseModel, RootCause, int>, IRootCauseLogic
    {
        public RootCauseLogic(IPersistenceService<RootCause> service) : base(service)
        {

        }

        public BusinessOperationResult<List<RootCauseModel>> GetActives()
        {
            return GetData<RootCauseModel>(x => x.IsActive);
        }
    }

}
