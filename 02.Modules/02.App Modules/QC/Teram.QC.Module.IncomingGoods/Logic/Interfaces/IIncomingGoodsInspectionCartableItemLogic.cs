using Teram.Framework.Core.Logic;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Enums;
using Teram.QC.Module.IncomingGoods.Models;
namespace Teram.QC.Module.IncomingGoods.Logic.Interfaces
{
 
    public interface IIncomingGoodsInspectionCartableItemLogic : IBusinessOperations<IncomingGoodsInspectionCartableItemModel, IncomingGoodsInspectionCartableItem, int>
    {
        BusinessOperationResult<List<IncomingGoodsInspectionCartableItemModel>> GetByUserId(Guid userId);

        BusinessOperationResult<List<IncomingGoodsInspectionCartableItemModel>> GetbyIncomingGoodsInspectionId(int incomingGoodsInspectionId);        
    }

}
