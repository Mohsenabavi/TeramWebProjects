using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.HR.Module.Recruitment.Enums;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.JobApplicants
{
    public class JobApplicantsIntroductionLetterModel:ModelBase<JobApplicantsIntroductionLetter,int>
    {
        public int JobApplicantsIntroductionLetterId { get; set; }        
        public IntroductionLetterType IntroductionLetterType { get; set; }

        [GridColumn(nameof(IntroductionLetterTypeDescription))]
        public string IntroductionLetterTypeDescription => IntroductionLetterType.GetDescription();
        public string FileUrl { get; set; }
        public int JobApplicantId { get; set; }
        public DateTime CreateDate {  get; set; }
        public long LetterNumber {  get; set; }
    }
}
