using Teram.Framework.Core.Logic;
using Teram.HR.Module.FileUploader.Models;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Models;

namespace Teram.QC.Module.FinalProduct.Logic.Interfaces
{

    public interface IFinalProductNoncomplianceFileLogic : IBusinessOperations<FinalProductNoncomplianceFileModel, FinalProductNoncomplianceFile, int>
    {
        BusinessOperationResult<FinalProductNoncomplianceFileModel> SaveToDataBase(IFormFile file, int finalProductNoncomplianceId);

        BusinessOperationResult<List<FinalProductNoncomplianceFileModel>> GetByFinalProductNoncomplianceId(int finalProductNoncomplianceId);

        BusinessOperationResult<ShowAttachmentModel> GetAttachmentsByFinalProductNoncomplianceFileId(int finalProductNoncomplianceFileId);
    }

}
