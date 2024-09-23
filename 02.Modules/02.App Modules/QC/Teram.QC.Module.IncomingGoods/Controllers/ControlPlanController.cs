using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Framework.Core.Service;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Logic.Interfaces;
using Teram.QC.Module.IncomingGoods.Models;
using Teram.QC.Module.IncomingGoods.Models.ImportModels;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Teram.Web.Core.Extensions;
using Teram.Web.Core.Model;

namespace Teram.QC.Module.IncomingGoods.Controllers
{

    public class ControlPlanController : ControlPanelBaseController<ControlPlanModel, ControlPlan, int>
    {
        private readonly IControlPlanCategoryLogic controlPlanCategoryLogic;
        private readonly IControlPlanLogic controlPlanLogic;

        public ControlPlanController(ILogger<ControlPlanController> logger
            , IStringLocalizer<ControlPlanController> localizer, IControlPlanCategoryLogic controlPlanCategoryLogic, IControlPlanLogic controlPlanLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;
            Model = new ViewInformation<ControlPlanModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["ControlPlan"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["ControlPlan"],
                OperationColumns = true,
                HomePage = nameof(ControlPlanController).Replace("Controller", "") + "/index",
                HasToolbar = true,
                ToolbarName = "_adminToolbar",
                GetDataUrl = "",
                LoadAjaxData = false,
                ExtraScripts="/ExternalModule/QC/Module/IncomingGoods/Scripts/ControlPlan.js",
            };
            this.controlPlanCategoryLogic=controlPlanCategoryLogic??throw new ArgumentNullException(nameof(controlPlanCategoryLogic));
            this.controlPlanLogic=controlPlanLogic??throw new ArgumentNullException(nameof(controlPlanLogic));
        }

        [ControlPanelMenu("ControlPlan", ParentName = "BaseInfoManagement", Icon = "fa fa-tasks", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            ViewBag.Categories=GetCategories();
            return View(Model);
        }

        protected override void ModifyItem(ILogic<ControlPlanModel> service, int id)
        {
            ViewBag.Categories=GetCategories();
            base.ModifyItem(service, id);
        }

        public List<SelectListItem> GetCategories()
        {
            var result = new List<SelectListItem>();
            var data = controlPlanCategoryLogic.GetAll();
            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }
            return data.ResultEntity.ToSelectList(nameof(ControlPlanCategoryModel.Title), nameof(ControlPlanCategoryModel.ControlPlanCategoryId));
        }

        public IActionResult GetControlPlanData(DatatablesSentModel model, int? controlPlanCategoryId)
        {

            var controlPlansResult = controlPlanLogic.GetByFilter(controlPlanCategoryId,model.Start, model.Length);

            if (controlPlansResult.ResultStatus != OperationResultStatus.Successful || controlPlansResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[controlPlansResult.AllMessages] });
            }
            var totalCount = controlPlansResult?.Count ?? 0;
            return Json(new { model.Draw, recordsTotal = totalCount, recordsFiltered = totalCount, data = controlPlansResult?.ResultEntity, error = "", result = "ok" });
        }

        [HttpPost]
        [ParentalAuthorize(nameof(Index))]
        public IActionResult ImportFromExcel([FromServices] IControlPlanLogic controlPlanLogic)
        {
            try
            {
                if (!Request.Form.Files.Any())
                {
                    return Json(new { Result = "fail", message = "هیچ فایلی انتخاب نشده است" });
                }
                var file = Request.Form.Files[0];

                var controlPlansList = new List<ImportControlPlanModel>();

                using var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                controlPlansList = controlPlansList.ImportFromExcel(ms).ToList();                              

                foreach (var item in controlPlansList.OrderBy(x => x.ControlPlanCategoryTitle))
                {

                    var categortyId = 0;

                    var controlPlancategory = controlPlanCategoryLogic.GetByTitle(item.ControlPlanCategoryTitle);

                    
                    if (controlPlancategory.ResultEntity== null)
                    {
                        var insertModel = new ControlPlanCategoryModel
                        {

                            CreateDate = DateTime.Now,
                            IsActive = true,
                            Remarks = " ",
                            Title = item.ControlPlanCategoryTitle,
                        };
                        var AddCategoryResult = controlPlanCategoryLogic.AddNew(insertModel);

                        categortyId=AddCategoryResult.ResultEntity.ControlPlanCategoryId;
                    }
                    else
                    {
                        categortyId=controlPlancategory.ResultEntity.ControlPlanCategoryId;
                    }
                    var controlPlanModel = new ControlPlanModel
                    {
                        ControlPlanCategoryId = categortyId,
                        ControlPlanParameter=item.ControlPlanParameter,
                        AcceptanceCriteria = (item.AcceptanceCriteria!=null) ? item.AcceptanceCriteria : " ",
                        QuantityDescription = item.QuantityDescription,
                    };
                    var AddResult = controlPlanLogic.AddNew(controlPlanModel);
                }
                return Json(new { Result = "ok" });
            }
            catch (Exception)
            {
                return Json(new { Result = "fail", message = "درج برخی از ردیف ها با خطا مواجه شده" });
            }
        }
    }

}
