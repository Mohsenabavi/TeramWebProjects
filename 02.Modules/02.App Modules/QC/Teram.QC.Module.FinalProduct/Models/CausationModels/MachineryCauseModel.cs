using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models.CausationModels
{
    public class MachineryCauseModel:ModelBase<MachineryCause,int>
    {
        public int MachineryCauseId { get; set; }

        [GridColumn(nameof(Title))]
        public string Title { get; set; }
    }
}
