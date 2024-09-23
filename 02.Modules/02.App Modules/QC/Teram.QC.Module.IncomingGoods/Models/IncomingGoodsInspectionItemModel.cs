using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Enums;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.IncomingGoods.Models
{
    public class IncomingGoodsInspectionItemModel : ModelBase<IncomingGoodsInspectionItem, int>
    {
        public int IncomingGoodsInspectionItemId { get; set; }

        public int IncomingGoodsInspectionId { get; set; }

        public int ControlPlanId { get; set; }

        public string? InspectionResult { get; set; }

        [Required]
        public bool? IsMatch { get; set; }

        public string IsMatchText =>(IsMatch.HasValue && IsMatch.Value) ? "منطبق است" : "نا منطبق است";

        public string? InspectionResultRemarks { get; set; }

        public int? AmountOfDefects { get; set; }


        #region ControlPlanFields

        public string ControlPlanCategoryTitle { get; set; }

        public string ControlPlanParameter { get; set; }

        public string QuantityDescription { get; set; }

        public string AcceptanceCriteria { get; set; }


        #endregion
        public string ControlPlanControlPlanParameter { get; set; }

        public string ControlPlanQuantityDescription { get; set; }

        public string ControlPlanAcceptanceCriteria { get; set; }
    }
}
