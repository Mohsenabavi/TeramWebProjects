using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Recruitment.Entities.Questionaires;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.Questionaire;

namespace Teram.HR.Module.Recruitment.Logic
{  
    public class QuestionLogic : BusinessOperations<QuestionModel, Question, int>, IQuestionLogic
    {
        public QuestionLogic(IPersistenceService<Question> service) : base(service)
        {

        }

        public BusinessOperationResult<List<QuestionModel>> GetActives()
        {
            return GetData<QuestionModel>(x => x.IsActive);
        }
    }

}
