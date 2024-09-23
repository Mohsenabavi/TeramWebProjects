using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Enums;

namespace Teram.QC.Module.FinalProduct.Models
{
    public class FinalProductNoncomplianceDetailSampleModel : ModelBase<FinalProductNoncomplianceDetailSample, int>
    {
        public int FinalProductNoncomplianceDetailSampleId { get; set; }
        public SampleType SampleType { get; set; }
        public int Amount { get; set; }
        public int FinalProductNoncomplianceDetailId { get; set; }
        public OpinionType OpinionTypeQCManager {  get; set; }
        public OpinionType OpinionTypeCEO {  get; set; }
        public OpinionType OpinionTypeCEOFinal {  get; set; }
        public int SeparatedCount {  get; set; }
        public int WasteCount {  get; set; }
        public string? WasteDocumentNumber {  get; set; }
    }
}
