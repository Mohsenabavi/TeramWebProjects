using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Models.CausationModels;

namespace Teram.QC.Module.FinalProduct.Logic.Interfaces
{  
    public interface IRootCauseLogic : IBusinessOperations<RootCauseModel, RootCause, int>
    {
        BusinessOperationResult<List<RootCauseModel>> GetActives();
    }
}
