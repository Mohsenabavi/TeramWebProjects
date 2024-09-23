using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.Questionaires;
using Teram.HR.Module.Recruitment.Models.Questionaire;

namespace Teram.HR.Module.Recruitment.Logic.Interfaces
{

    public interface IQuestionnaireLogic : IBusinessOperations<QuestionnaireModel, Questionnaire, int>
    {
        BusinessOperationResult<List<QuestionnaireModel>> GetActives();
    }

}
