using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Models;

namespace Teram.QC.Module.FinalProduct.Logic.Interfaces
{
    public interface IFinalProductInspectionLogic : IBusinessOperations<FinalProductInspectionModel, FinalProductInspection, int>
    {
        BusinessOperationResult<List<FinalProductInspectionModel>> GetByOrderNoAndProductCode(int orderNo, string productCode);
        BusinessOperationResult<FinalProductInspectionModel> GetByFinalProductInspectionId(int finalProductInspectionId);
        BusinessOperationResult<FinalProductInspectionModel> GetByPalletNumber(int palletNumber);
        BusinessOperationResult<List<FinalProductInspectionModel>> GetByIds(List<int> finalProductInspectionIds);
        BusinessOperationResult<List<FinalProductInspectionModel>> GetByFilter(int orderNo, int number, string productCode, string TracingCode, string tracingCode, string orderTitle, int? start = null, int? length = null);
    } 

}
