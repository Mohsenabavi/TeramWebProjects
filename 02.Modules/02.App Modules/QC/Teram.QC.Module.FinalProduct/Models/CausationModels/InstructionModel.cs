using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models.CausationModels
{
    public class InstructionModel:ModelBase<Instruction,int>
    {
        public int InstructionId { get; set; }

        [GridColumn(nameof(Title))]
        public string Title {  get; set; }

        [GridColumn(nameof(Number))]
        public string? Number {  get; set; }
        public List<CausationModel>? Causations { get; set; }
    }
}
