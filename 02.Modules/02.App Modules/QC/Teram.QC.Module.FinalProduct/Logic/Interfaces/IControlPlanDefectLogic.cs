using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Models;

namespace Teram.QC.Module.FinalProduct.Logic.Interfaces
{
    public interface IControlPlanDefectLogic : IBusinessOperations<ControlPlanDefectModel, ControlPlanDefect, int>
    {
        BusinessOperationResult<List<ControlPlanDefectModel>> GetByControlPlanId(int controlPlanId);
        BusinessOperationResult<ControlPlanDefectModel> GetByControlPlanDefectId(int controlPlanDefectId);
        BusinessOperationResult<List<ControlPlanDefectModel>> GetByControlPlanDefectIds(List<int> controlPlanDefectIds);

    }
}
