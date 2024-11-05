using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Models;
namespace Teram.QC.Module.FinalProduct.Logic.Interfaces
{

    public interface IFinalProductNoncomplianceDetailLogic : IBusinessOperations<FinalProductNoncomplianceDetailModel, FinalProductNoncomplianceDetail, int>
    {
        BusinessOperationResult<FinalProductNoncomplianceDetailModel> GetByPalletNumberAndControlPlanDefectId(int palletNumber, int controlPlanDefcetId);
        BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductNoncomplianceIdsAndPalletNumber(List<int> finalProductNoncomplianceIds, int palletNumber);
        BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductNoncomplianceIdsAndFinalProductInspectionId(List<int> finalProductNoncomplianceIds, int finalProductInspectionId);
        BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductInspectionId(int finalProductInspectionId);
        BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductInspectionIds(List<int> finalProductInspectionIds);
        BusinessOperationResult<List<FinalProductNoncomplianceDetailModel>> GetByFinalProductNoncomplianceId(int finalProductNoncomplianceId);
        Task<(List<PalletsStatusModel> Items, int TotalCount)> GetPalletsStatusAsync(
        string number = null,
        string orderNo = null,
        string productName = null,
        int? sampleCount = null,
        string tracingCode = null,
        string productCode = null,
        int skip = 0,
        int take = 10);
    }

}
