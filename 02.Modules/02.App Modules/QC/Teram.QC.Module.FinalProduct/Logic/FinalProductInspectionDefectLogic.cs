using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;

namespace Teram.QC.Module.FinalProduct.Logic
{   
    public class FinalProductInspectionDefectLogic : BusinessOperations<FinalProductInspectionDefectModel, FinalProductInspectionDefect, int>, IFinalProductInspectionDefectLogic
    {
        public FinalProductInspectionDefectLogic(IPersistenceService<FinalProductInspectionDefect> service) : base(service)
        {

        }

        public BusinessOperationResult<FinalProductInspectionDefectModel> GetByFianalProductInspectionIdAndControlPlandefectId(int fianalProductInspectionId, int controlPlandefectId)
        {
            return GetFirst<FinalProductInspectionDefectModel>(x => x.FinalProductInspectionId==fianalProductInspectionId && x.ControlPlanDefectId==controlPlandefectId);
        }      

        public BusinessOperationResult<List<FinalProductInspectionDefectModel>> GetByIdsAndControlPlanDefectId(List<int> finalProductInspectionIds, int controlPlanDefectId)
        {
            return GetData<FinalProductInspectionDefectModel>(x => finalProductInspectionIds.Contains(x.FinalProductInspectionId) && x.ControlPlanDefectId==controlPlanDefectId);
        }
    }

}
