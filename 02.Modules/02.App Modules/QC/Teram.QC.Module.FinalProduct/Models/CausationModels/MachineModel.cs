using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models.CausationModels
{
    public class MachineModel:ModelBase<Machine,int>
    {
        public int MachineId { get; set; }

        [GridColumn(nameof(Title))]
        public string Title {  get; set; }

        [GridColumn(nameof(Remarks))]
        public string? Remarks { get; set; }
        public bool IsActive {  get; set; }

        [GridColumn(nameof(IsActiveText))]
        public string IsActiveText => IsActive ? "فعال" : "غیر فعال";
    }
}
