using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Models.ServiceModels;

namespace Teram.QC.Module.FinalProduct.Services
{
    public interface IQueryService
    {
        Task<BusinessOperationResult<PalletInfoModel>> GetPalletInfo(int PalletNo);
        Task<BusinessOperationResult<List<OrderProductModel>>> GetOrderProducts(int orderNo);
        Task<BusinessOperationResult<List<EmployeeModel>>> GetActiveEmployees();
    }
}
