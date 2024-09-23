using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Attributes;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Entities.WorkFlow;
using Teram.QC.Module.FinalProduct.Enums;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.QC.Module.FinalProduct.Models.WorkFlow;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Teram.Web.Core.Extensions;
using Teram.Web.Core.Model;

namespace Teram.QC.Module.FinalProduct.Controllers
{
    [ControlPanelMenu("FinalProductWorkFlow", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar, Order = 4)]
    public class FlowInstructionController : ControlPanelBaseController<FlowInstructionModel, FlowInstruction, int>
    {
        private readonly IRoleSharedService roleSharedService;
        private readonly IFlowInstructionLogic flowInstructionLogic;

        public FlowInstructionController(ILogger<FlowInstructionController> logger, IRoleSharedService roleSharedService
            , IStringLocalizer<FlowInstructionController> localizer, IFlowInstructionLogic flowInstructionLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<FlowInstructionModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["FlowInstruction"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["FlowInstruction"],
                OperationColumns = true,
                GetDataUrl = "",
                LoadAjaxData = false,
                HomePage = nameof(FlowInstructionController).Replace("Controller", "") + "/index",
                ExtraScripts="/ExternalModule/QC/Module/FinalProduct/Scripts/FlowInstruction.js",
            };
            this.roleSharedService=roleSharedService??throw new ArgumentNullException(nameof(roleSharedService));
            this.flowInstructionLogic=flowInstructionLogic??throw new ArgumentNullException(nameof(flowInstructionLogic));
        }

        [ControlPanelMenu("FlowInstruction", ParentName = "FinalProductWorkFlow", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            ViewBag.Roles=GetRoles();
            ViewData["FieldName"]=GetFieldNames();
            ViewData["FieldValue"]=GetBoolValues();
            return View(Model);
        }

        protected override void ModifyItem(ILogic<FlowInstructionModel> service, int id)
        {
            Model.ModelData=new FinalProductInspectionModel();
            var relatedRecord = flowInstructionLogic.GetById(id);
            ViewBag.Roles=GetRoles();
            ViewData["FieldName"]=GetFieldNames();
            ViewData["FieldValue"]=GetBoolValues();
            Model.ModelData = relatedRecord.ResultEntity;
        }

        public override IActionResult Save([FromServices] ILogic<FlowInstructionModel> service, FlowInstructionModel model)
        {
            return base.Save(service, model);
        }

        private List<SelectListItem> GetRoles()
        {
            var result = new List<SelectListItem>();
            var data = roleSharedService.GetAllRoles();
            if (data!=null)
            {

                result= data.ToSelectList(nameof(RoleInfo.Name), nameof(RoleInfo.Id));
            }
            return result;
        }
        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetFlowInstructions(DatatablesSentModel model,ReferralStatus? fromStatus, ReferralStatus? toStatus,FormStatus? formStatus) {

            var flowInstructionResult = flowInstructionLogic.GetGridDataByFiler(fromStatus, toStatus, formStatus, model.Start, model.Length);

            if (flowInstructionResult.ResultStatus != OperationResultStatus.Successful || flowInstructionResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[flowInstructionResult.AllMessages] });
            }
            var totalCount = flowInstructionResult?.Count ?? 0;
            return Json(new { model.Draw, recordsTotal = totalCount, recordsFiltered = totalCount, data = flowInstructionResult?.ResultEntity, error = "", result = "ok" });
        }

        private List<SelectListItem> GetFieldNames()
        {
            var result = new List<SelectListItem>();
            Type entityType = typeof(FinalProductNoncompliance);
            List<PropertyInfo> properties = entityType.GetProperties().Where(
                             prop => Attribute.IsDefined(prop, typeof(CompareField))).ToList();

            string text = string.Empty;

            foreach (var property in properties)
            {
                DescriptionAttribute? descriptionAttribute = property.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
                text =  (descriptionAttribute != null) ? descriptionAttribute.Description : property.Name;
                result.Add(new SelectListItem { Text=text, Value=property.Name });
            }
            return result;
        }
        private List<SelectListItem> GetBoolValues()
        {
            return
            [
                new() { Text = "True", Value = "true" },
                new() { Text = "False", Value = "false" }               
            ];
        }
    }
}
