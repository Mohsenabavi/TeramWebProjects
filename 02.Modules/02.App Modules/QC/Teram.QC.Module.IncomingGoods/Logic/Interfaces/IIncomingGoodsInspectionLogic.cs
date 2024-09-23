using Microsoft.AspNetCore.Mvc;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Enums;
using Teram.QC.Module.IncomingGoods.Models;
namespace Teram.QC.Module.IncomingGoods.Logic.Interfaces
{
  
    public interface IIncomingGoodsInspectionLogic : IBusinessOperations<IncomingGoodsInspectionModel, IncomingGoodsInspection, int>
    {
        BusinessOperationResult<IncomingGoodsInspectionModel> GetById(int id);
        BusinessOperationResult<List<IncomingGoodsInspectionModel>> GetByFilter(List<InspectionFormStatus> inspectionFormStatuses, bool isProductionManager, bool isAdmin, string? qualityInspectionNumber, string? goodsTitle, string vendorName, InspectionFormStatus? inspectionFormStatus, int? start = null, int? length = null);
        public CartableItemsReturnModel GetStepCartableItems(IncomingGoodsInspectionModel model, string comments);
    }
}
