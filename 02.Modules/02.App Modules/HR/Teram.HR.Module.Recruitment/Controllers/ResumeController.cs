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
    public class ResumeController : ControlPanelBaseController<ResumeModel, Resume, int>
    {
        private readonly IBaseInformationLogic baseInformationLogic;

        public ResumeController(ILogger<ResumeController> logger
            , IStringLocalizer<ResumeController> localizer, IBaseInformationLogic baseInformationLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<ResumeModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["EditResume"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["EditResume"],
                OperationColumns = true,
                HomePage = nameof(ResumeController).Replace("Controller", "") + "/index",
            };
            this.baseInformationLogic = baseInformationLogic ?? throw new ArgumentNullException(nameof(baseInformationLogic));
        }

        [ControlPanelMenu("EditResume", ParentName = "EditBaseInformation", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            ViewBag.BaseInformationList = GetBaseInformationList();
            return View(Model);
        }

        protected override void ModifyItem(ILogic<ResumeModel> service, int id)
        {
            ViewBag.BaseInformationList = GetBaseInformationList();
            base.ModifyItem(service, id);
        }     
        private List<SelectListItem> GetBaseInformationList()
        {
            var result=new List<SelectListItem>();
            var data = baseInformationLogic.GetAll();
            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }
            return data.ResultEntity.Select(x=> new SelectListItem {                 
                Text=string.Concat(x.Name," ",x.Lastname,"  ","کد ملی :",x.NationalCode),
                Value=x.BaseInformationId.ToString()
                       
            }).ToList();
        }
    }
}
