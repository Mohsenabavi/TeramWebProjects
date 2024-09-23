using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.Questionaires;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.Questionaire
{
    public class QuestionnaireModel : ModelBase<Questionnaire, int>
    {
        public int QuestionnaireId { get; set; }

        [GridColumn(nameof(Title))]
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }

        [GridColumn(nameof(PersianCreateDate))]
        public string PersianCreateDate => CreateDate.ToPersianDate();
        public bool IsActive { get; set; }

        [GridColumn(nameof(IsActiveText))]
        public string IsActiveText => IsActive ? "فعال" : "غیر فعال";
    }
}
