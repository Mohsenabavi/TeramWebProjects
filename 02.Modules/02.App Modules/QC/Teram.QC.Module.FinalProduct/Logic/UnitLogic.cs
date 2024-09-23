using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models.CausationModels;

namespace Teram.QC.Module.FinalProduct.Logic
{
    public class UnitLogic : BusinessOperations<UnitModel, Unit, int>, IUnitLogic
    {
        public UnitLogic(IPersistenceService<Unit> service) : base(service)
        {

        }

        public BusinessOperationResult<List<UnitModel>> GetActives()
        {
            return GetData<UnitModel>(x => x.IsActive);
        }
    }

}
