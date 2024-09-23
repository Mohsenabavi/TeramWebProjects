using Microsoft.AspNetCore.Mvc.Rendering;

namespace Teram.QC.Module.FinalProduct.Models
{
    public class ChooseNonComplianceModel
    {
        public List<SelectListItem> NonCompliancesList { get; set; }
        public int ControlPlanDefectId { get; set; }
        public int Number {  get; set; }
        public int FinalProductInspectionId {  get; set; }
    }
}
