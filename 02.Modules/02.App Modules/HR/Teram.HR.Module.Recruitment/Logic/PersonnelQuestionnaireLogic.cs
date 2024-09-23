using MongoDB.Driver;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.HR.Module.Recruitment.Entities.Questionaires;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.Questionaire;
using Teram.ServiceContracts;

namespace Teram.HR.Module.Recruitment.Logic
{
    public class PersonnelQuestionnaireLogic : BusinessOperations<PersonnelQuestionnaireModel, PersonnelQuestionnaire, int>, IPersonnelQuestionnaireLogic
    {
        private readonly IUserSharedService userSharedService;
        private readonly IPersonQuestionnaireQuestionLogic personQuestionnaireQuestionLogic;
        private readonly IQuestionnaireQuestionLogic questionnaireQuestionLogic;

        public PersonnelQuestionnaireLogic(IPersistenceService<PersonnelQuestionnaire> service,
            IUserSharedService userSharedService, IPersonQuestionnaireQuestionLogic personQuestionnaireQuestionLogic,
            IQuestionnaireQuestionLogic questionnaireQuestionLogic) : base(service)
        {
            AfterAdd += PersonnelQuestionnaireLogic_AfterAdd;
            BeforeAdd += PersonnelQuestionnaireLogic_BeforeAdd;
            this.userSharedService = userSharedService ?? throw new ArgumentNullException(nameof(userSharedService));
            this.personQuestionnaireQuestionLogic = personQuestionnaireQuestionLogic ?? throw new ArgumentNullException(nameof(personQuestionnaireQuestionLogic));
            this.questionnaireQuestionLogic = questionnaireQuestionLogic ?? throw new ArgumentNullException(nameof(questionnaireQuestionLogic));
        }

        private void PersonnelQuestionnaireLogic_BeforeAdd(TeramEntityEventArgs<PersonnelQuestionnaire, PersonnelQuestionnaireModel, int> entity)
        {
            entity.NewEntity.CreateDate = DateTime.Now;
            var nationalCode = entity.NewEntity.NationalCode;
            var userInfo = userSharedService.GetUserInfo(nationalCode);
            entity.NewEntity.UserId = userInfo.FirstOrDefault().UserId;
        }

        private void PersonnelQuestionnaireLogic_AfterAdd(TeramEntityEventArgs<PersonnelQuestionnaire, PersonnelQuestionnaireModel, int> entity)
        {
           
           
            var questionnaireQuestions = questionnaireQuestionLogic.GetByQuestionnaireId(entity.NewEntity.QuestionnaireId);
            var PersonnelquestionnaireQuestionListModel = new List<PersonQuestionnaireQuestionModel>();

            foreach (var item in questionnaireQuestions.ResultEntity)
            {
                var PersonnelquestionnaireQuestionModel = new PersonQuestionnaireQuestionModel
                {

                    Answer = Enums.Answer.NoAnswer,
                    QuestionnaireQuestionId = item.QuestionnaireQuestionId,
                    PersonnelQuestionnaireId = entity.NewEntity.PersonnelQuestionnaireId,                    
                };

                PersonnelquestionnaireQuestionListModel.Add(PersonnelquestionnaireQuestionModel);
            }

            var bulkInsertResult = personQuestionnaireQuestionLogic.BulkInsertAsync(PersonnelquestionnaireQuestionListModel).Result;


        }
    }

}
