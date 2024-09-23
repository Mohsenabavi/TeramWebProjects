using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;

namespace Teram.QC.Module.FinalProduct.Logic
{
    public class ControlPlanDefectLogic : BusinessOperations<ControlPlanDefectModel, ControlPlanDefect, int>, IControlPlanDefectLogic
    {
        public ControlPlanDefectLogic(IPersistenceService<ControlPlanDefect> service) : base(service)
        {

        }      
        public BusinessOperationResult<List<ControlPlanDefectModel>> GetByControlPlanId(int controlPlanId)
        {
            return GetData<ControlPlanDefectModel>(x=>x.QCControlPlanId == controlPlanId);
        }

        public BusinessOperationResult<ControlPlanDefectModel> GetByControlPlanDefectId(int controlPlanDefectId)
        {
            return GetFirst<ControlPlanDefectModel>(x=>x.ControlPlanDefectId == controlPlanDefectId);
        }

        public BusinessOperationResult<List<ControlPlanDefectModel>> GetByControlPlanDefectIds(List<int> controlPlanDefectIds)
        {
            return GetData<ControlPlanDefectModel>(x => controlPlanDefectIds.Contains(x.ControlPlanDefectId));
        }
    }

}
