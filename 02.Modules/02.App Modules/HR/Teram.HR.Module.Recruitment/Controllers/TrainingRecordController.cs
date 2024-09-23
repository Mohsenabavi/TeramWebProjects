using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Logic;
using Teram.HR.Module.Recruitment.Entities.WorkWthUs;
using Teram.HR.Module.Recruitment.Logic.Interfaces;
using Teram.HR.Module.Recruitment.Models.WorkWithUs;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.HR.Module.Recruitment.Controllers
{   
    public class TrainingRecordController : ControlPanelBaseController<TrainingRecordModel, TrainingRecord, int>
    {
        private readonly IBaseInformationLogic baseInformationLogic;

        public TrainingRecordController(ILogger<TrainingRecordController> logger
            , IStringLocalizer<TrainingRecordController> localizer, IBaseInformationLogic baseInformationLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<TrainingRecordModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["EditTrainingRecord"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["EditTrainingRecord"],
                OperationColumns = true,
                HomePage = nameof(TrainingRecordController).Replace("Controller", "") + "/index",
            };
            this.baseInformationLogic = baseInformationLogic ?? throw new ArgumentNullException(nameof(baseInformationLogic));
        }

        [ControlPanelMenu("EditTrainingRecord", ParentName = "EditBaseInformation", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            ViewBag.BaseInformationList = GetBaseInformationList();
            return View(Model);
        }

        protected override void ModifyItem(ILogic<TrainingRecordModel> service, int id)
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
