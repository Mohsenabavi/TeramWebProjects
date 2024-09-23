using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models.CausationModels;

namespace Teram.QC.Module.FinalProduct.Logic
{  
    public class ActionerLogic : BusinessOperations<ActionerModel, Actioner, int>, IActionerLogic
    {
        public ActionerLogic(IPersistenceService<Actioner> service) : base(service)
        {

        }
    }
}
