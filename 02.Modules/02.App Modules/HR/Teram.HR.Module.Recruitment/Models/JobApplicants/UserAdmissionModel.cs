using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.JobApplicants;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.JobApplicants
{
    public class UserAdmissionModel : ModelBase<JobApplicant, int>
    {
        public int JobApplicantId { get; set; }

        [GridColumn(nameof(NationalCode))]
        public string NationalCode { get; set; }

        [GridColumn(nameof(FirstName))]
        public string FirstName { get; set; }

        [GridColumn(nameof(LastName))]
        public string LastName { get; set; }

        public bool NoAddictionDone { get; set; }

        [GridColumn(nameof(NoAddictionDoneText))]
        public string NoAddictionDoneText => NoAddictionDone ? "مراجعه کرده" : "عدم مراجعه";
        public DateTime? NoAddictionDate { get; set; }

        [GridColumn(nameof(PersianNoAddictionDate))]
        public string PersianNoAddictionDate => NoAddictionDate != null ? NoAddictionDate.Value.ToPersianDate() : "";

        public Guid? NoAddictionApprovedBy { get; set; }

        [GridColumn(nameof(NoAddictionApprovedByName))]
        public string? NoAddictionApprovedByName { get; set; }

        public bool NoBadBackgroundDone { get; set; }


        [GridColumn(nameof(NoBadBackgroundDoneText))]
        public string NoBadBackgroundDoneText => NoBadBackgroundDone ? "دریافت کرده" : "عدم دریافت";

        public DateTime? NoBadBackgroundDate { get; set; }


        [GridColumn(nameof(PersianNoBadBackgroundDate))]
        public string PersianNoBadBackgroundDate => NoBadBackgroundDate != null ? NoBadBackgroundDate.Value.ToPersianDate() : "";

        public Guid? NoBadBackgroundApprovedBy { get; set; }

        [GridColumn(nameof(NoBadBackgroundApprovedByName))]
        public string? NoBadBackgroundApprovedByName { get; set; }

        public DateTime? OccupationalMedicineDate { get; set; }

        [GridColumn(nameof(PersianOccupationalMedicineDate))]
        public string PersianOccupationalMedicineDate => OccupationalMedicineDate != null ? OccupationalMedicineDate.Value.ToPersianDate() : "";
        public bool OccupationalMedicineDone { get; set; }

        [GridColumn(nameof(OccupationalMedicineDone))]
        public string OccupationalMedicineDoneText => OccupationalMedicineDone ? "مراجعه کرده" : "عدم مراجعه";

        public bool IsAdmin {  get; set; }

        public bool IsShow {  get; set; }
    }
}
