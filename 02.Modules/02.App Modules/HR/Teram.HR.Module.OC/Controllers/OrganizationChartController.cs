using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.HR.Module.OC.Entities;
using Teram.HR.Module.OC.Logic;
using Teram.HR.Module.OC.Logic.Interface;
using Teram.HR.Module.OC.Models;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using Teram.Framework.Core.Logic;
using Teram.Web.Core.Extensions;
using Teram.ServiceContracts;
using System.ComponentModel.DataAnnotations;
using MongoDB.Driver;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Teram.HR.Module.OC.Controllers
{
    [ControlPanelMenu("OrganizationChartManagement", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
    public class OrganizationChartController : ControlPanelBaseController<OrganizationChartModel, OrganizationChart, int>
    {
        private readonly IOrganizationChartLogic organizationChartLogic;
        private readonly IPositionLogic positionLogic;
        private readonly IUserSharedService userSharedService;

        public OrganizationChartController(ILogger<OrganizationChartController> logger
            , IStringLocalizer<OrganizationChartController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer, IOrganizationChartLogic organizationChartLogic, IPositionLogic positionLogic, IUserSharedService userSharedService)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<OrganizationChartModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["OrganizationChart"],
                HasGrid = false,
                HasManagmentForm = true,
                Title = localizer["OrganizationChart"],
                OperationColumns = true,
                HomePage = nameof(OrganizationChartController).Replace("Controller", "") + "/index",
                ExtraScripts = "/ExternalModule/HR/Module/OC/Scripts/OC.js",
            };
            this.organizationChartLogic=organizationChartLogic??throw new ArgumentNullException(nameof(organizationChartLogic));
            this.positionLogic=positionLogic??throw new ArgumentNullException(nameof(positionLogic));
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
        }

        [ControlPanelMenu("OrganizationChart", ParentName = "OrganizationChartManagement", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            ViewBag.Parents=FillParents();
            ViewBag.Positions=FillPositions();
            ViewBag.Users=FillUsers();
            return View(Model);
        }

        public override IActionResult Save([FromServices] ILogic<OrganizationChartModel> service, OrganizationChartModel model)
        {

            if (model.ParentOrganizationChartId==model.OrganizationChartId)
            {
                return Json(new { result = "fail", message = localizer["Node Can Not Be Parent Of itself"] });

            }

            return base.Save(service, model);
        }

        public IActionResult GetChartData()
        {

            var data = organizationChartLogic.GetOrganizationChartData();
            return new JsonResult(data);
        } 

        public IActionResult RemoveNode(int id)
        {

            var deleteResult = organizationChartLogic.Delete(id);

            if (deleteResult.ResultStatus != OperationResultStatus.Successful || !deleteResult.ResultEntity)
            {
                return Json(new { result = "fail", data = deleteResult.ResultEntity, message = localizer["Error In Delete Item"] });
            }

            return Json(new { result = "ok", data = deleteResult.ResultEntity, message = localizer["Node Deleted Successfully"] });
        }

        [Display(Description = "Edit")]
        [ParentalAuthorize(nameof(Index))]
        public IActionResult EditPartialAsync(int id)
        {
            if (id == 0)
                return PartialView("Add", new OrganizationChartModel());

            var data = organizationChartLogic.GetRow(id);

            Model.ModelData=data.ResultEntity;
            return PartialView("Add", data.ResultEntity);

        }   
               

        #region load Select Options

        public List<SelectListItem> FillPositions()
        {

            var result = new List<SelectListItem>();

            var data = positionLogic.GetAll();

            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }
            return data.ResultEntity.ToSelectList(nameof(PositionModel.Title), nameof(PositionModel.PositionId));
        }

        public List<SelectListItem> FillParents()
        {

            var result = new List<SelectListItem>();

            var data = organizationChartLogic.GetAll();

            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }
            return data.ResultEntity.Select(x => new SelectListItem
            {
                Text=$"{x.FirstName} {x.LastName}",
                Value=x.OrganizationChartId.ToString()

            }).ToList();
        }


        public List<SelectListItem> FillUsers()
        {
            var result = new List<SelectListItem>();

            var data = userSharedService.GetAllUsers();

            var x = data.Select(x => new SelectListItem
            {
                Text=$"{x.Name} {x.Family}",
                Value=x.UserId.ToString()

            }).ToList();

            return x;
        }
        #endregion
    }
}
