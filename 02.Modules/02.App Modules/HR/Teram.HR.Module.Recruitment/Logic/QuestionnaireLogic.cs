using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Recruitment.Entities.Questionaires;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.Questionaire;

namespace Teram.HR.Module.Recruitment.Logic
{
   
    public class QuestionnaireLogic : BusinessOperations<QuestionnaireModel, Questionnaire, int>, IQuestionnaireLogic
    {
        public QuestionnaireLogic(IPersistenceService<Questionnaire> service) : base(service)
        {
            BeforeAdd += QuestionnaireLogic_BeforeAdd;
        }

        public BusinessOperationResult<List<QuestionnaireModel>> GetActives()
        {
            return GetData<QuestionnaireModel>(x => x.IsActive);
        }

        private void QuestionnaireLogic_BeforeAdd(TeramEntityEventArgs<Questionnaire, QuestionnaireModel, int> entity)
        {
           entity.NewEntity.CreateDate = DateTime.Now;
        }
    }

}
