using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Models;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.QC.Module.IncomingGoods.Controllers
{
    
    [ControlPanelMenu("IncomingGoods", Name = "IncomingGoods", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar, Order = 100)]
    public class ControlPlanCategoryController : ControlPanelBaseController<ControlPlanCategoryModel, ControlPlanCategory, int>
    {
        public ControlPlanCategoryController(ILogger<ControlPlanCategoryController> logger
            , IStringLocalizer<ControlPlanCategoryController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<ControlPlanCategoryModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["ControlPlanCategory"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["ControlPlanCategory"],
                OperationColumns = true,
                HomePage = nameof(ControlPlanCategoryController).Replace("Controller", "") + "/index",
            };
        }

        [ControlPanelMenu("ControlPlanCategory", ParentName = "BaseInfoManagement", Icon = "fa fa-list-ul", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }
    }

}
