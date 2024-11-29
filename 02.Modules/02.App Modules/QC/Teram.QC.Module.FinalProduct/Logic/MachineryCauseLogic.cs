using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models.CausationModels;

namespace Teram.QC.Module.FinalProduct.Logic
{   
    public class MachineryCauseLogic : BusinessOperations<MachineryCauseModel, MachineryCause, int>, IMachineryCauseLogic
    {
        public MachineryCauseLogic(IPersistenceService<MachineryCause> service) : base(service)
        {

        }
    }

}
