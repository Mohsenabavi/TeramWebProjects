using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Models.CausationModels;

namespace Teram.QC.Module.FinalProduct.Logic.Interfaces
{  
    public interface IWorkStationLogic : IBusinessOperations<WorkStationModel, WorkStation, int>
    {
        BusinessOperationResult<List<WorkStationModel>> GetActives();
    }

}
