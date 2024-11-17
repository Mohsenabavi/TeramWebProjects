using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models.CausationModels;

namespace Teram.QC.Module.FinalProduct.Logic
{  
    public class RawMaterialLogic : BusinessOperations<RawMaterialModel, RawMaterial, int>, IRawMaterialLogic
    {
        public RawMaterialLogic(IPersistenceService<RawMaterial> service) : base(service)
        {

        }
    }

}
