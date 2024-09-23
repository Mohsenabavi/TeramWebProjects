using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.Questionaires;
using Teram.Web.Core.Attributes;

namespace Teram.HR.Module.Recruitment.Models.Questionaire
{
    public class QuestionModel : ModelBase<Question, int>
    {
        public int QuestionId { get; set; }

        [GridColumn(nameof(Title))]
        public string Title { get; set; }
        public bool IsActive {  get; set; }

       
    }
}
