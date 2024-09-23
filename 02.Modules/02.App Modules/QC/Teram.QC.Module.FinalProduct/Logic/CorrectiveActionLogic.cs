using Microsoft.EntityFrameworkCore;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Entities.WorkFlow;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;

namespace Teram.QC.Module.FinalProduct.Logic
{

    public class CorrectiveActionLogic : BusinessOperations<CorrectiveActionModel, CorrectiveAction, int>, ICorrectiveActionLogic
    {
        public CorrectiveActionLogic(IPersistenceService<CorrectiveAction> service) : base(service)
        {
           
        }
        private void CorrectiveActionLogic_BeforeUpdate(TeramEntityEventArgs<CorrectiveAction, CorrectiveActionModel, int> entity)
        {            
        }
    }

}
