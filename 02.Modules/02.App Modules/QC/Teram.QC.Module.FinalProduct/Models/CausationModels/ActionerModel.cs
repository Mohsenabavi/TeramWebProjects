using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models.CausationModels
{
    public class ActionerModel : ModelBase<Actioner, int>
    {
        public int ActionerId { get; set; }

        [GridColumn(nameof(FirstName))]
        public string FirstName { get; set; }

        [GridColumn(nameof(LastName))]
        public string LastName { get; set; }

        [GridColumn(nameof(PersonnelCode))]
        public string? PersonnelCode { get; set; }


        [GridColumn(nameof(PostTitle))]
        public string? PostTitle { get; set; }

        public bool IsActive { get; set; }

        public Guid? UserId {  get; set; }
    }
}
