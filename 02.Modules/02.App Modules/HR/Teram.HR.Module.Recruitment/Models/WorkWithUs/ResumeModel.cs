using System.ComponentModel.DataAnnotations;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.WorkWthUs;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.WorkWithUs
{
    public class ResumeModel : ModelBase<Resume, int>
    {
        public int ResumeId { get; set; }

        public int BaseInformationId { get; set; }      

        [GridColumn(nameof(CompanyName))]       
        public string? CompanyName { get; set; }

        public string? EndCooperationReason { get; set; }

        [GridColumn(nameof(Activity))]
        public string? Activity { get; set; }


        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        [GridColumn(nameof(OrganizationalPosition))]       
        public string? OrganizationalPosition { get; set; }


        public string? DirectSupervisorName {  get; set; }

        public string? DirectSupervisorPhone {  get; set; }

       

        

       

       


        #region Helper Fields

        [GridColumn(nameof(BaseInformationNationalCode))]
        public string? BaseInformationNationalCode { get; set; }


        public string? BaseInformationName { get; set; }

        public string? BaseInformationLastname { get; set; }


        [GridColumn(nameof(FullName))]
        public string? FullName => BaseInformationName + " " + BaseInformationLastname;


        [GridColumn(nameof(StartPersianDate))]
        public string StartPersianDate => (StartDate!=null) ? StartDate.Value.ToPersianDate() : "-";

       

        [GridColumn(nameof(FinishPersianDate))]
        public string FinishPersianDate => (FinishDate!=null) ? FinishDate.Value.ToPersianDate() : "-";

        #endregion

    }
}
