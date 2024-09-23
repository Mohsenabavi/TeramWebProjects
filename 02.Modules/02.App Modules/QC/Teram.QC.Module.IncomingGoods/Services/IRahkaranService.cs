using Teram.QC.Module.IncomingGoods.Models.ServiceModels;

namespace Teram.QC.Module.IncomingGoods.Services
{
    public interface IRahkaranService
    {
        Task<List<QualityInspectionResultModel>> GetQualityInspectionData(string Number);
    }
}
