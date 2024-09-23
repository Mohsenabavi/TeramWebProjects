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
    public class QuestionnaireQuestionController : ControlPanelBaseController<QuestionnaireQuestionModel, QuestionnaireQuestion, int>
    {
        private readonly IQuestionnaireLogic questionnaireLogic;
        private readonly IQuestionLogic questionLogic;

        public QuestionnaireQuestionController(ILogger<QuestionnaireQuestionController> logger
            , IStringLocalizer<QuestionnaireQuestionController> localizer, IQuestionnaireLogic questionnaireLogic, IQuestionLogic questionLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;


            Model = new ViewInformation<QuestionnaireQuestionModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["QuestionnaireQuestion"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["QuestionnaireQuestion"],
                OperationColumns = true,
                HomePage = nameof(QuestionnaireQuestionController).Replace("Controller", "") + "/index",
            };
            this.questionnaireLogic = questionnaireLogic ?? throw new ArgumentNullException(nameof(questionnaireLogic));
            this.questionLogic = questionLogic ?? throw new ArgumentNullException(nameof(questionLogic));
        }

        [ControlPanelMenu("QuestionnaireQuestion", ParentName = "QuestionnaireParent", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            ViewBag.Questionaires = GetActiveQuestionaires();
            ViewBag.Questions = GetActiveQuestions();
            return View(Model);
        }
        protected override void ModifyItem(ILogic<QuestionnaireQuestionModel> service, int id)
        {
            ViewBag.Questionaires = GetActiveQuestionaires();
            ViewBag.Questions = GetActiveQuestions();
            base.ModifyItem(service, id);
        }

        private List<SelectListItem> GetActiveQuestionaires()
        {

            var result = new List<SelectListItem>();

            var data = questionnaireLogic.GetActives();

            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }
            return data.ResultEntity.ToSelectList(nameof(QuestionnaireModel.Title), nameof(QuestionnaireModel.QuestionnaireId));
        }

        private List<SelectListItem> GetActiveQuestions()
        {

            var result = new List<SelectListItem>();

            var data = questionLogic.GetActives();

            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }

            return data.ResultEntity.ToSelectList(nameof(QuestionModel.Title), nameof(QuestionModel.QuestionId));

        }
    }

}
