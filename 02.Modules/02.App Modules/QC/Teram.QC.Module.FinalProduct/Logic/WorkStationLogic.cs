using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models.CausationModels;

namespace Teram.QC.Module.FinalProduct.Logic
{
    public class WorkStationLogic : BusinessOperations<WorkStationModel, WorkStation, int>, IWorkStationLogic
    {
        public WorkStationLogic(IPersistenceService<WorkStation> service) : base(service)
        {

        }

        public BusinessOperationResult<List<WorkStationModel>> GetActives()
        {
            return GetData<WorkStationModel>(x => x.IsActive);
        }
    }

}
