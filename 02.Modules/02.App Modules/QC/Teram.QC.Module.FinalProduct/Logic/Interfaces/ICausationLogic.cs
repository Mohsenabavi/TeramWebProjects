using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Models.CausationModels;
using Teram.QC.Module.FinalProduct.Models.ReportsModels;

namespace Teram.QC.Module.FinalProduct.Logic.Interfaces
{
    public interface ICausationLogic : IBusinessOperations<CausationModel, Causation, int>
    {
        BusinessOperationResult<CausationModel> GetByFinalProductNonComplianceId(int finalProductNonComplianceId);
        Task<List<WrongDoerReportModel>> GetWrongDoerReport(int wrongDoerId);
        Task<(List<WrongDoersListReportModel> Items, int TotalCount)> GetWrongDoerReportData(
       int? WrongDoerId = null,
       DateTime? statInputDate = null,
       DateTime? endInputDate = null,
       int skip = 0,
       int take = 10);
    }
}
