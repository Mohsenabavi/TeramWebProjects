using Teram.Framework.Core.Logic;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Models;

namespace Teram.QC.Module.IncomingGoods.Logic.Interfaces
{
  
    public interface IIncomingGoodsInspectionItemLogic : IBusinessOperations<IncomingGoodsInspectionItemModel, IncomingGoodsInspectionItem, int>
    {
        BusinessOperationResult<List<IncomingGoodsInspectionItemModel>> GetbyIncomingGoodsInspectionId(int incomingGoodsInspectionId);
    }
}
