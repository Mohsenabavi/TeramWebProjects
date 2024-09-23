using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.HR.Module.Recruitment.Entities.Questionaires;
using Teram.HR.Module.Recruitment.Models.Questionaire;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.HR.Module.Recruitment.Controllers
{
    [ControlPanelMenu("QuestionnaireParent", Name = "QuestionnaireParent", Icon = "fa fa-tasks", PanelType = PanelType.User, Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar)]
    public class QuestionController : ControlPanelBaseController<QuestionModel, Question, int>
    {

        public QuestionController(ILogger<QuestionController> logger
            , IStringLocalizer<QuestionController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;


            Model = new ViewInformation<QuestionModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["Questions"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["Questions"],
                OperationColumns = true,
                HomePage = nameof(QuestionController).Replace("Controller", "") + "/index",
            };
        }

        [ControlPanelMenu("Questions", ParentName = "QuestionnaireParent", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
    }

}
