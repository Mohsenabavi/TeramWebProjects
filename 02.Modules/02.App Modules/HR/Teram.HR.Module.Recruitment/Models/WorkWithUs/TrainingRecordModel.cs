using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.WorkWthUs;
using Teram.HR.Module.Recruitment.Enums;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.WorkWithUs
{
    public class TrainingRecordModel:ModelBase<TrainingRecord,int>
    {
        public int TrainingRecordId { get; set; }
        public int BaseInformationId {  get; set; }

        [GridColumn(nameof(BaseInformationNationalCode))]
        public string BaseInformationNationalCode { get; set; }


        public string BaseInformationName { get; set; }

        public string BaseInformationLastname { get; set; }


        [GridColumn(nameof(FullName))]
        public string FullName => BaseInformationName + " " + BaseInformationLastname;

        [GridColumn(nameof(CollegeName))]
        public string? CollegeName { get; set; }

        public EducationGrade? EducationGrade { get; set; }

        [GridColumn(nameof(Average))]
        public double? Average {  get; set; }

        [GridColumn(nameof(Field))]
        public string? Field { get; set; }

        public int? EducationCityId { get; set; }

        public DateTime StartDate { get; set; }

        [GridColumn(nameof(PersianStartDate))]
        public string PersianStartDate => StartDate.ToPersianDate();

        public DateTime EndDate { get; set; }

        [GridColumn(nameof(PersianEndDate))]
        public string PersianEndDate => EndDate.ToPersianDate();

        [GridColumn(nameof(EducationGradeTitle))]
        public string EducationGradeTitle { get { return EducationGrade != null ? (string)EducationGrade.GetDescription() : string.Empty; } }

    }
}
