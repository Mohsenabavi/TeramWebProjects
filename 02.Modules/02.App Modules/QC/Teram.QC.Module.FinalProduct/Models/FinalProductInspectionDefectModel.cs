using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;

namespace Teram.QC.Module.FinalProduct.Models
{
    public class FinalProductInspectionDefectModel : ModelBase<FinalProductInspectionDefect, int>
    {
        public int FinalProductInspectionDefectId { get; set; }
        public int FinalProductInspectionId { get; set; }
        public int ControlPlanDefectId { get; set; }
        public int? FirstSample {  get; set; }
        public int? SecondSample {  get; set; }
        public int? ThirdSample {  get; set; }
        public int? ForthSample {  get; set; }
        public string? FinalProductNoncomplianceNumbers {  get; set; }
        public string? ControlPlanTitle {  get; set; }         
        public bool IsLocked {  get; set; }
    }
}
