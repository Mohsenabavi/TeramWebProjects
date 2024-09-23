using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.WorkWthUs;
using Teram.HR.Module.Recruitment.Logic;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.WorkWithUs;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.HR.Module.Recruitment.Controllers
{
    public class EmergencyContactController : ControlPanelBaseController<EmergencyContactModel, EmergencyContact, int>
    {
        private readonly IBaseInformationLogic baseInformationLogic;

        public EmergencyContactController(ILogger<EmergencyContactController> logger
            , IStringLocalizer<EmergencyContactController> localizer, IBaseInformationLogic baseInformationLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<EmergencyContactModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["EditEmergencyContact"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["EditEmergencyContact"],
                OperationColumns = true,
                HomePage = nameof(EmergencyContactController).Replace("Controller", "") + "/index",
            };
            this.baseInformationLogic = baseInformationLogic ?? throw new ArgumentNullException(nameof(baseInformationLogic));
        }

        [ControlPanelMenu("EditEmergencyContact", ParentName = "EditBaseInformation", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            ViewBag.BaseInformationList = GetBaseInformationList();
            return View(Model);
        }
        protected override void ModifyItem(ILogic<EmergencyContactModel> service, int id)
        {
            ViewBag.BaseInformationList = GetBaseInformationList();
            base.ModifyItem(service, id);
        }

        private List<SelectListItem> GetBaseInformationList()
        {
            var result = new List<SelectListItem>();
            var data = baseInformationLogic.GetAll();
            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }
            return data.ResultEntity.Select(x => new SelectListItem
            {
                Text = string.Concat(x.Name, " ", x.Lastname, "  ", "کد ملی :", x.NationalCode),
                Value = x.BaseInformationId.ToString()

            }).ToList();
        }
    }

}
