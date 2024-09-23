using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Recruitment.Entities.Questionaires;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.Questionaire;

namespace Teram.HR.Module.Recruitment.Logic
{
 
    public class QuestionnaireQuestionLogic : BusinessOperations<QuestionnaireQuestionModel, QuestionnaireQuestion, int>, IQuestionnaireQuestionLogic
    {
        public QuestionnaireQuestionLogic(IPersistenceService<QuestionnaireQuestion> service) : base(service)
        {
           
        }

        public BusinessOperationResult<List<QuestionnaireQuestionModel>> GetByQuestionnaireId(int questionnaireId)
        {
           return GetData<QuestionnaireQuestionModel>(x=>x.QuestionnaireId == questionnaireId);
        }
    }

}
