using Teram.Framework.Core.Extensions;
using Teram.QC.Module.FinalProduct.Enums;

namespace Teram.QC.Module.FinalProduct.Models
{
    public class NoncomplianceDetailSampleSummaryModel
    {

        public int FinalProductNonComplianceId { get; set; }

        public bool IsSeparated {  get; set; }

        public int FirstSamplePalletCount { get; set; }
        public int FirstSampleSum { get; set; }
        public OpinionType? FirstSampleOpinion { get; set; }   
        public OpinionType? FirstSampleOpinionCEO { get; set; }
        public OpinionType? FirstSampleOpinionCEOFinal { get; set; }
        public int? FirstSampleSeparatedCount { get; set; }
        public int FirstSampleWasteCount { get; set; }
        public long FirstSampleTotalCount {  get; set; }
        public double FirstSamplePercent {  get; set; }



        public string FirstSampleOpinionText => (FirstSampleOpinion>0) ? FirstSampleOpinion.GetDescription() : "-";
        public string FirstSampleOpinionCEOText => (FirstSampleOpinionCEO>0) ? FirstSampleOpinionCEO.GetDescription() : "-";
        public string FirstSampleOpinionCEOFinalText => (FirstSampleOpinionCEOFinal>0) ? FirstSampleOpinionCEOFinal.GetDescription() : "-";


        public int SecondSamplePalletCount { get; set; }
        public int SecondSampleSum { get; set; }
        public OpinionType? SecondSampleOpinion { get; set; }
        public OpinionType? SecondSampleOpinionCEO { get; set; }
        public OpinionType? SecondSampleOpinionCEOFinal { get; set; }
        public int? SecondSampleSeparatedCount { get; set; }
        public int SecondSampleWasteCount { get; set; }
        public long SecondSampleTotalCount { get; set; }
        public double SecondSamplePercent { get; set; }


        public string SecondSampleOpinionText => (SecondSampleOpinion>0) ? SecondSampleOpinion.GetDescription() : "-";
        public string SecondSampleOpinionCEOText => (SecondSampleOpinionCEO>0) ? SecondSampleOpinionCEO.GetDescription() : "-";
        public string SecondSampleOpinionCEOFinalText => (SecondSampleOpinionCEOFinal>0) ? SecondSampleOpinionCEOFinal.GetDescription() : "-";


        public int ThirdSamplePalletCount { get; set; }
        public int ThirdSampleSum { get; set; }
        public OpinionType? ThirdSampleOpinion { get; set; }
        public OpinionType? ThirdSampleOpinionCEO { get; set; }
        public OpinionType? ThirdSampleOpinionCEOFinal { get; set; }
        public int? ThirdSampleSeparatedCount { get; set; }
        public int ThirdSampleWasteCount { get; set; }
        public long ThirdSampleTotalCount { get; set; }
        public double ThirdSamplePercent { get; set; }

        public string ThirdSampleOpinionText => (ThirdSampleOpinion>0) ? ThirdSampleOpinion.GetDescription() : "-";
        public string ThirdSampleOpinionCEOText => (ThirdSampleOpinionCEO>0) ? ThirdSampleOpinionCEO.GetDescription() : "-";
        public string ThirdSampleOpinionCEOFinalText => (ThirdSampleOpinionCEOFinal>0) ? ThirdSampleOpinionCEOFinal.GetDescription() : "-";

        public int ForthSamplePalletCount { get; set; }
        public int ForthSampleSum { get; set; }
        public OpinionType? ForthSampleOpinion { get; set; }
        public OpinionType? ForthSampleOpinionCEO { get; set; }
        public OpinionType? ForthSampleOpinionCEOFinal { get; set; }
        public int? ForthSampleSeparatedCount { get; set; }
        public int ForthSampleWasteCount { get; set; }
        public long ForthSampleTotalCount { get; set; }
        public double ForthSamplePercent { get; set; }
        public FormStatus FormStatus { get; set; }

        public string ForthSampleOpinionText => (ForthSampleOpinion>0) ? ForthSampleOpinion.GetDescription() : "-";
        public string ForthSampleOpinionCEOText => (ForthSampleOpinionCEO>0) ? ForthSampleOpinionCEO.GetDescription() : "-";
        public string ForthSampleOpinionCEOFinalText => (ForthSampleOpinionCEOFinal>0) ? ForthSampleOpinionCEOFinal.GetDescription() : "-";


    }
}
