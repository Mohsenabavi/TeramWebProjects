using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Recruitment.Entities.Questionaires;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.Questionaire;

namespace Teram.HR.Module.Recruitment.Logic
{
  
    public class PersonQuestionnaireQuestionLogic : BusinessOperations<PersonQuestionnaireQuestionModel, PersonQuestionnaireQuestion, int>, IPersonQuestionnaireQuestionLogic
    {
        public PersonQuestionnaireQuestionLogic(IPersistenceService<PersonQuestionnaireQuestion> service) : base(service)
        {

        }
     
    }

}
