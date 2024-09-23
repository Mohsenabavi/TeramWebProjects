using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models.CausationModels
{
    public class RootCauseModel:ModelBase<RootCause,int>
    {
        public int RootCauseId { get; set; }

        [GridColumn(nameof(Title))]
        public string Title {  get; set; }
        public bool IsActive { get; set; }
        public List<CausationModel>? Causations { get; set; }

        [GridColumn(nameof(IsActiveText))]
        public string IsActiveText => IsActive ? "فعال" : "غیرفعال";
    }
}
