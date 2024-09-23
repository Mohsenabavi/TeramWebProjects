using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.WorkWthUs;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.WorkWithUs
{
    public class EducationModel : ModelBase<Education, int>
    {

        public int EducationId { get; set; }

        public int BaseInformationId { get; set; }


        [GridColumn(nameof(BaseInformationNationalCode))]
        public string BaseInformationNationalCode { get; set; }

        public string BaseInformationName { get; set; }

        public string BaseInformationLastname { get; set; }

        [GridColumn(nameof(FullName))]
        public string FullName => BaseInformationName + " " + BaseInformationLastname;

        [GridColumn(nameof(Subject))]
        public string? Subject { get; set; }

        [GridColumn(nameof(OrganizationName))]
        public string? OrganizationName { get; set; }


        [GridColumn(nameof(EducationDurationHour))]
        public int? EducationDurationHour { get; set; }

        [GridColumn(nameof(EducationYear))]
        public int? EducationYear { get; set; }
        public bool HasCertificate { get; set; }


        [GridColumn(nameof(HasCertificateText))]
        public string HasCertificateText => HasCertificate ? "دارد" : "ندارد";

    }
}
