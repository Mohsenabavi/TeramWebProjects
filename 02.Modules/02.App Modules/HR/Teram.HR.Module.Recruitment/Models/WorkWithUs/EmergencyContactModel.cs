using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.WorkWthUs;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.WorkWithUs
{
    public class EmergencyContactModel:ModelBase<EmergencyContact,int>
    {
        public int EmergencyContactId { get; set; }

        public int BaseInformationId {  get; set; }

        [GridColumn(nameof(BaseInformationNationalCode))]
        public string BaseInformationNationalCode { get; set; }

        public string BaseInformationName { get; set; }

        public string BaseInformationLastname { get; set; }

        [GridColumn(nameof(FullName))]
        public string FullName => BaseInformationName + " " + BaseInformationLastname;

        [GridColumn(nameof(Name))]
        public string? Name {  get; set; }


        [GridColumn(nameof(LastName))]
        public string? LastName { get; set; }


        [GridColumn(nameof(Relation))]
        public string? Relation {  get; set; }


        [GridColumn(nameof(Mobile))]
        public string? Mobile {  get; set; }
    }
}
