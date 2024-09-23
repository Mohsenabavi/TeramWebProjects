using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.HR.Module.Recruitment.Entities.Questionaires;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.Questionaire;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.HR.Module.Recruitment.Controllers
{
    public class PersonQuestionnaireQuestionController : ControlPanelBaseController<PersonQuestionnaireQuestionModel, PersonQuestionnaireQuestion, int>
    {
        private readonly IPersonQuestionnaireQuestionLogic personQuestionnaireQuestionLogic;

        public PersonQuestionnaireQuestionController(ILogger<PersonQuestionnaireQuestionController> logger
            , IStringLocalizer<PersonQuestionnaireQuestionController> localizer, IPersonQuestionnaireQuestionLogic personQuestionnaireQuestionLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<PersonQuestionnaireQuestionModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["PersonQuestionnaireQuestion"],
                HasGrid = false,
                HasManagmentForm = true,
                Title = localizer["PersonQuestionnaireQuestion"],
                OperationColumns = true,
                HomePage = nameof(PersonQuestionnaireQuestionController).Replace("Controller", "") + "/index",
            };
            this.personQuestionnaireQuestionLogic = personQuestionnaireQuestionLogic ?? throw new ArgumentNullException(nameof(personQuestionnaireQuestionLogic));
        }

        [ControlPanelMenu("PersonQuestionnaireQuestion", ParentName = "QuestionnaireParent", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            Model.ModelData = personQuestionnaireQuestionLogic.GetAll().ResultEntity;
            return View(Model);
        }

        [ParentalAuthorize(nameof(Index))]
        public IActionResult SaveAll(List<PersonQuestionnaireQuestionModel> personQuestionnaireQuestions)
        {

            foreach (var item in personQuestionnaireQuestions)
            {
                var updateModel = new PersonQuestionnaireQuestionModel
                {

                    PersonQuestionnaireQuestionId = item.PersonQuestionnaireQuestionId,
                    Answer = item.Answer,
                    AnswerDate=DateTime.Now,
                };

                personQuestionnaireQuestionLogic.Update(updateModel);
            }
            return Json(new
            {
                result = "ok",
                message = "Saved",
                title = sharedLocalizer["SaveTitle"]
            });
        }
    }

}
