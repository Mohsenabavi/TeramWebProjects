using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Models;

namespace Teram.QC.Module.FinalProduct.Logic.Interfaces
{  
    public interface IFinalProductInspectionDefectLogic : IBusinessOperations<FinalProductInspectionDefectModel, FinalProductInspectionDefect, int>
    {
        BusinessOperationResult<FinalProductInspectionDefectModel> GetByFianalProductInspectionIdAndControlPlandefectId(int fianalProductInspectionId,int controlPlandefectId);
        BusinessOperationResult<List<FinalProductInspectionDefectModel>> GetByIdsAndControlPlanDefectId(List<int> finalProductInspectionIds, int controlPlanDefectId);


    }
}
