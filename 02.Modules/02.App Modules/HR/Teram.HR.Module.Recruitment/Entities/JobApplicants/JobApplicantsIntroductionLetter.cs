using System.ComponentModel.DataAnnotations.Schema;
using Teram.Framework.Core.Domain;
using Teram.HR.Module.Recruitment.Enums;

namespace Teram.HR.Module.Recruitment.Entities.JobApplicants
{
    [Table(nameof(JobApplicantsIntroductionLetter) + "s", Schema = "HR")]
    public class JobApplicantsIntroductionLetter:EntityBase
    {
        public int JobApplicantsIntroductionLetterId { get; set; }

        private IntroductionLetterType _introductionLetterType;
        public IntroductionLetterType IntroductionLetterType
        {
            get { return _introductionLetterType; }
            set
            {
                if (_introductionLetterType == value) return;
                _introductionLetterType = value;
                OnPropertyChanged();
            }
        }

        private string _fileUrl;
        public string FileUrl
        {
            get { return _fileUrl; }
            set
            {
                if (_fileUrl == value) return;
                _fileUrl = value;
                OnPropertyChanged();
            }
        }

        private int _jobApplicantId;
        public int JobApplicantId
        {
            get { return _jobApplicantId; }
            set
            {
                if (_jobApplicantId == value) return;
                _jobApplicantId = value;
                OnPropertyChanged();
            }
        }

        private DateTime _createDate;
        public DateTime CreateDate
        {
            get { return _createDate; }
            set
            {
                if (_createDate == value) return;
                _createDate = value;
                OnPropertyChanged();
            }
        }

        private long _letterNumber;
        public long LetterNumber
        {
            get { return _letterNumber; }
            set
            {
                if (_letterNumber == value) return;
                _letterNumber = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey(nameof(JobApplicantId))]
        public virtual JobApplicant JobApplicant { get; set; }
    }
}
