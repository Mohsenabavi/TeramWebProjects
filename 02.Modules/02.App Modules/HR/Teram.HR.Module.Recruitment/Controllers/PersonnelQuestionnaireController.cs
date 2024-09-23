using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.Questionaires;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.Questionaire;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Teram.Web.Core.Extensions;

namespace Teram.HR.Module.Recruitment.Controllers
{  
    public class PersonnelQuestionnaireController : ControlPanelBaseController<PersonnelQuestionnaireModel, PersonnelQuestionnaire, int>
    {
        private readonly IQuestionnaireLogic questionnaireLogic;

        public PersonnelQuestionnaireController(ILogger<PersonnelQuestionnaireController> logger
            , IStringLocalizer<PersonnelQuestionnaireController> localizer, IQuestionnaireLogic questionnaireLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;


            Model = new ViewInformation<PersonnelQuestionnaireModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["PersonnelQuestionnaire"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["PersonnelQuestionnaire"],
                OperationColumns = true,
                HomePage = nameof(PersonnelQuestionnaireController).Replace("Controller", "") + "/index",
            };
            this.questionnaireLogic = questionnaireLogic ?? throw new ArgumentNullException(nameof(questionnaireLogic));
        }

        [ControlPanelMenu("PersonnelQuestionnaire", ParentName = "QuestionnaireParent", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            ViewBag.Questionnaires = GetActiveQuestionnaires();
            return View(Model);
        }

        protected override void ModifyItem(ILogic<PersonnelQuestionnaireModel> service, int id)
        {
            ViewBag.Questionnaires = GetActiveQuestionnaires();
            base.ModifyItem(service, id);
        }

        private List<SelectListItem> GetActiveQuestionnaires()
        {
            var result=new List<SelectListItem>();

            var data = questionnaireLogic.GetActives();

            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }
            return data.ResultEntity.ToSelectList(nameof(QuestionnaireModel.Title), nameof(QuestionnaireModel.QuestionnaireId));

        }
    }

}
