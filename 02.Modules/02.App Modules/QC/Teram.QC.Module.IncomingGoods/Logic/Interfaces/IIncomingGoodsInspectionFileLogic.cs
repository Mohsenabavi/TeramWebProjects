using Teram.Framework.Core.Logic;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Models;

namespace Teram.QC.Module.IncomingGoods.Logic.Interfaces
{
    public interface IIncomingGoodsInspectionFileLogic : IBusinessOperations<IncomingGoodsInspectionFileModel, IncomingGoodsInspectionFile, int>
    {
        BusinessOperationResult<IncomingGoodsInspectionFileModel> SaveToDataBase(IFormFile file, int incomingGoodsInspectionId);
        BusinessOperationResult<IncomingGoodsInspectionFileModel> AddFile(IncomingGoodsInspectionFileModel newModel, IFormFile file);
    }
}
