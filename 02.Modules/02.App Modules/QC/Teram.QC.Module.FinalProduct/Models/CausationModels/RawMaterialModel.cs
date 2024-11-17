using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models.CausationModels
{
    public class RawMaterialModel : ModelBase<RawMaterial, int>
    {
        public int RawMaterialId { get; set; }

        [GridColumn(nameof(Title))]
        public string Title { get; set; }
    }
}
