using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models.CausationModels;

namespace Teram.QC.Module.FinalProduct.Logic
{
    public class MachineLogic : BusinessOperations<MachineModel, Machine, int>, IMachineLogic
    {
        public MachineLogic(IPersistenceService<Machine> service) : base(service)
        {

        }

        public BusinessOperationResult<List<MachineModel>> GetActives()
        {
            return GetData<MachineModel>(x => x.IsActive);
        }
    }
}
