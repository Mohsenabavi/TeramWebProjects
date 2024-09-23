using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models.CausationModels
{
    public class WorkStationModel:ModelBase<WorkStation,int>
    {
        public int WorkStationId { get; set; }


        [GridColumn(nameof(Title))]
        public string Title {  get; set; }
        public bool IsActive {  get; set; }

        [GridColumn(nameof(IsActiveText))]
        public string IsActiveText => IsActive ? "فعال" : "غیرفعال";
    }
}
