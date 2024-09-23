using Teram.QC.Module.IncomingGoods.Enums;

namespace Teram.QC.Module.IncomingGoods.Models
{
    public class CartableItemsReturnModel
    {
        public bool hasPermission {  get; set; }
        public InspectionFormStatus InspectionFormStatus { get; set; }
        public List<IncomingGoodsInspectionCartableItemModel> IncomingGoodsInspectionCartableItems { get; set; }
    }
}
