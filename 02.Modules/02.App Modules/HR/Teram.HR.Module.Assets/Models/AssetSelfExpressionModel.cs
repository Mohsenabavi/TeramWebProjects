using Teram.Framework.Core.Logic;
using Teram.HR.Module.Assets.Entities;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.Assets.Models
{
    public class AssetSelfExpressionModel :ModelBase<AssetSelfExpression,int>
    {
        public int AssetSelfExpressionId { get; set; }

        [GridColumn(nameof(AssetID))]
        public string AssetID {  get; set; }

        [GridColumn(nameof(PlaqueNumber))]
        public string PlaqueNumber { get; set; }


        [GridColumn(nameof(Code))]
        public string Code { get; set; }


        [GridColumn(nameof(Title))]
        public string Title { get; set; }

        public Guid CreatedBy {  get; set; }

        public DateTime CreateDate { get; set; }

        [GridColumn(nameof(UserName))]
        public string UserName { get; set; }

        public string? Remarks {  get; set; }
    }
}
