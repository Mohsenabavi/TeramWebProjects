using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Models;
namespace Teram.QC.Module.FinalProduct.Logic.Interfaces
{

    public interface IFinalProductNoncomplianceDetailLogic : IBusinessOperations<FinalProductNoncomplianceDetailModel, FinalProductNoncomplianceDetail, int>
    {
        BusinessOperationResult<FinalProductNoncomplianceDetailModel> GetByPalletNumberAndControlPlanDefectId(int palletNumber,int controlPlanDefcetId);
        BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductNoncomplianceIdsAndPalletNumber(List<int> finalProductNoncomplianceIds, int palletNumber);
        BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductNoncomplianceIdsAndFinalProductInspectionId(List<int> finalProductNoncomplianceIds, int finalProductInspectionId);
        BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductInspectionId(int finalProductInspectionId);
        BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductInspectionIds(List<int> finalProductInspectionIds);
        BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductNoncomplianceId(int finalProductNoncomplianceId);
    }

}
