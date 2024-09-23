using Azure;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.Web.Core.Attributes;

namespace Teram.QC.Module.FinalProduct.Models.CausationModels
{
    public class OperatorModel:ModelBase<Operator,int>
    {
        public int OperatorId { get; set; }

        [GridColumn(nameof(FirstName))]
        public string FirstName { get; set; }

        [GridColumn(nameof(LastName))]
        public string LastName { get; set; }

        [GridColumn(nameof(PersonnelCode))]
        public string? PersonnelCode {  get; set; }

        [GridColumn(nameof(MobileNumber))]
        public string? MobileNumber { get; set; }

        [GridColumn(nameof(Remarks))]
        public string? Remarks {  get; set; }
        public bool IsActive { get; set; }
        public List<CausationModel>? Causations { get; set; }
       
       [GridColumn(nameof(IsActiveText))]
        public string IsActiveText => IsActive ? "فعال" : "غیرفعال";
        public int EmployeeID {  get; set; }

        public Guid? UserId {  get; set; }

        public string? NationalID {  get; set; }
    }
}
