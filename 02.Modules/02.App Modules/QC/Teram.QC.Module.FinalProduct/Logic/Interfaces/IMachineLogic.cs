using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Models.CausationModels;

namespace Teram.QC.Module.FinalProduct.Logic.Interfaces
{  
    public interface IMachineLogic : IBusinessOperations<MachineModel, Machine, int>
    {
        BusinessOperationResult<List<MachineModel>> GetActives();
    }
}
