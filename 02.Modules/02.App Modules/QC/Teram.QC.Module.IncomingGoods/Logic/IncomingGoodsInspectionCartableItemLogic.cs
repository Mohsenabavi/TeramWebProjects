using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Enums;
using Teram.QC.Module.IncomingGoods.Logic.Interfaces;
using Teram.QC.Module.IncomingGoods.Models;
using Teram.ServiceContracts;
namespace Teram.QC.Module.IncomingGoods.Logic
{

    public class IncomingGoodsInspectionCartableItemLogic : BusinessOperations<IncomingGoodsInspectionCartableItemModel, IncomingGoodsInspectionCartableItem, int>, IIncomingGoodsInspectionCartableItemLogic
    {
        private readonly IUserSharedService userSharedService;

        public IncomingGoodsInspectionCartableItemLogic(IPersistenceService<IncomingGoodsInspectionCartableItem> service, IUserSharedService userSharedService) : base(service)
        {
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
        }   

        public BusinessOperationResult<List<IncomingGoodsInspectionCartableItemModel>> GetbyIncomingGoodsInspectionId(int incomingGoodsInspectionId)
        {
           return GetData<IncomingGoodsInspectionCartableItemModel>(x=>x.IncomingGoodsInspectionId==incomingGoodsInspectionId);
        }

        public BusinessOperationResult<List<IncomingGoodsInspectionCartableItemModel>> GetByUserId(Guid userId)
        {
            return GetData<IncomingGoodsInspectionCartableItemModel>(x => x.UserId==userId);
        }
    }
}
