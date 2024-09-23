using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Enums;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.IncomingGoods.Models
{
    public class IncomingGoodsInspectionCartableItemModel : ModelBase<IncomingGoodsInspectionCartableItem, int>
    {

        #region Database Fields

        public int IncomingGoodsInspectionCartableItemId { get; set; }

        public int IncomingGoodsInspectionId { get; set; }

        public Guid UserId { get; set; }

        public bool IsApproved { get; set; }

        public string Comments { get; set; }

        public DateTime InputDate { get; set; }

        public DateTime? OutputDate { get; set; }

        public Guid ReferredBy { get; set; }

        #endregion


        public string ReferredToName { get; set; }

        public string ReferredByName {  get; set; }

        public string PersianInputDate => InputDate.ToPersianDateTime();

        public string PersianOutputDate => (OutputDate.HasValue) ? OutputDate.Value.ToPersianDateTime() : "-";



    }
}
