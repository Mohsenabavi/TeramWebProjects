using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Logic.Interfaces;
using Teram.QC.Module.IncomingGoods.Models;

namespace Teram.QC.Module.IncomingGoods.Logic
{
    public class IncomingGoodsInspectionItemLogic : BusinessOperations<IncomingGoodsInspectionItemModel, IncomingGoodsInspectionItem, int>, IIncomingGoodsInspectionItemLogic
    {
        public IncomingGoodsInspectionItemLogic(IPersistenceService<IncomingGoodsInspectionItem> service) : base(service)
        {

        }

        public BusinessOperationResult<List<IncomingGoodsInspectionItemModel>> GetbyIncomingGoodsInspectionId(int incomingGoodsInspectionId)
        {
            return GetData<IncomingGoodsInspectionItemModel>(x => x.IncomingGoodsInspectionId==incomingGoodsInspectionId);
        }
    }

}
