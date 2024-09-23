using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Models.CausationModels;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.QC.Module.FinalProduct.Controllers
{
    public class ActionerController : ControlPanelBaseController<ActionerModel, Actioner, int>
    {
        private readonly IUserSharedService userSharedService;

        public ActionerController(ILogger<ActionerController> logger
            , IStringLocalizer<ActionerController> localizer, IUserSharedService userSharedService,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<ActionerModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["Actioners"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["Actioners"],
                OperationColumns = true,
                HomePage = nameof(ActionerController).Replace("Controller", "") + "/index",
            };
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
        }

        [ControlPanelMenu("Actioners", ParentName = "BaseInfoManagement", Icon = "fa  fa-user-circle", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            ViewBag.UsersList=UsersList();
            return View(Model);         
        }

        protected override void ModifyItem(ILogic<ActionerModel> service, int id)
        {
            ViewBag.UsersList=UsersList();
            base.ModifyItem(service, id);
        }
        public List<SelectListItem> UsersList()
        {
            var result = new List<SelectListItem>();
            var data = userSharedService.GetAllUsers();
            if (data != null)
            {
                return data.Select(x => new SelectListItem
                {

                    Text = $"{x.Name} {x.Family}",
                    Value=x.UserId.ToString()

                }).ToList();
            };
            return result;
        }
    }
}
