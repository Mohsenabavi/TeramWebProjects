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
    public class QuestionnaireController : ControlPanelBaseController<QuestionnaireModel, Questionnaire, int>
    {

        public QuestionnaireController(ILogger<QuestionnaireController> logger
            , IStringLocalizer<QuestionnaireController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<QuestionnaireModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["Questionnaire"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["Questionnaire"],
                OperationColumns = true,
                HomePage = nameof(QuestionnaireController).Replace("Controller", "") + "/index",
            };
        }

        [ControlPanelMenu("Questionnaire", ParentName = "QuestionnaireParent", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
    }

}
