using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Logic;
using Teram.Module.AttachmentsManagement.Enums;
using Teram.Module.AttachmentsManagement.Models;
using Teram.QC.Module.IncomingGoods.Entities;
using Teram.QC.Module.IncomingGoods.Enums;
using Teram.QC.Module.IncomingGoods.Logic;
using Teram.QC.Module.IncomingGoods.Logic.Interfaces;
using Teram.QC.Module.IncomingGoods.Models;
using Teram.QC.Module.IncomingGoods.Models.ServiceModels;
using Teram.QC.Module.IncomingGoods.Services;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Teram.Web.Core.Extensions;
using Teram.Web.Core.Model;
using Teram.Web.Core.Security;

namespace Teram.QC.Module.IncomingGoods.Controllers
{
    public class IncomingGoodsInspectionController : ControlPanelBaseController<IncomingGoodsInspectionModel, IncomingGoodsInspection, int>
    {
        private readonly IConfiguration configuration;
        private readonly IUserPrincipal userPrincipal;
        private readonly IUserSharedService userSharedService;
        private readonly IRahkaranService rahkaranService;
        private readonly IControlPlanCategoryLogic controlPlanCategoryLogic;
        private readonly IControlPlanLogic controlPlanLogic;
        private readonly IIncomingGoodsInspectionLogic incomingGoodsInspectionLogic;
        private readonly IIncomingGoodsInspectionFileLogic incomingGoodsInspectionFileLogic;
        private readonly IIncomingGoodsInspectionCartableItemLogic incomingGoodsInspectionCartableItemLogic;

        public IncomingGoodsInspectionController(ILogger<IncomingGoodsInspectionController> logger, IConfiguration configuration, IUserPrincipal userPrincipal, IUserSharedService userSharedService
            , IStringLocalizer<IncomingGoodsInspectionController> localizer, IRahkaranService rahkaranService, IControlPlanCategoryLogic controlPlanCategoryLogic,
            IStringLocalizer<SharedResource> sharedLocalizer, IControlPlanLogic controlPlanLogic, IIncomingGoodsInspectionLogic incomingGoodsInspectionLogic,
            IIncomingGoodsInspectionFileLogic incomingGoodsInspectionFileLogic, IIncomingGoodsInspectionCartableItemLogic incomingGoodsInspectionCartableItemLogic)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;


            Model = new ViewInformation<IncomingGoodsInspectionModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["IncomingGoodsInspection"],
                GridId="IncomingGoodsInspectionGrid",
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["IncomingGoodsInspection"],
                GetDataUrl = "",
                LoadAjaxData = false,
                OperationColumns = true,
                ModelData=new IncomingGoodsInspectionModel(),
                HomePage = nameof(IncomingGoodsInspectionController).Replace("Controller", "") + "/index",
                ExtraScripts="/ExternalModule/QC/Module/IncomingGoods/Scripts/IncomingGoodsInspection.js",
            };
            this.configuration=configuration??throw new ArgumentNullException(nameof(configuration));
            this.userPrincipal=userPrincipal??throw new ArgumentNullException(nameof(userPrincipal));
            this.userSharedService=userSharedService??throw new ArgumentNullException(nameof(userSharedService));
            this.rahkaranService=rahkaranService??throw new ArgumentNullException(nameof(rahkaranService));
            this.controlPlanCategoryLogic=controlPlanCategoryLogic??throw new ArgumentNullException(nameof(controlPlanCategoryLogic));
            this.controlPlanLogic=controlPlanLogic??throw new ArgumentNullException(nameof(controlPlanLogic));
            this.incomingGoodsInspectionLogic=incomingGoodsInspectionLogic??throw new ArgumentNullException(nameof(incomingGoodsInspectionLogic));
            this.incomingGoodsInspectionFileLogic=incomingGoodsInspectionFileLogic??throw new ArgumentNullException(nameof(incomingGoodsInspectionFileLogic));
            this.incomingGoodsInspectionCartableItemLogic=incomingGoodsInspectionCartableItemLogic??throw new ArgumentNullException(nameof(incomingGoodsInspectionCartableItemLogic));
        }

        [ControlPanelMenu("IncomingGoodsInspection", ParentName = "IncomingGoods", Icon = "fa fa-check-circle", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            var hasPermissionForInsertData = userPrincipal.CurrentUser.HasClaim("Permission", ":IncomingGoodsInspection:InsertFormDataPermoission");
            Model.ModelData.HasPermissionForInsertForm=hasPermissionForInsertData;
            ViewBag.Categories=GetCategories();
            return View(Model);
        }
        public IActionResult GetProductionSupervisors()
        {
            var ProductionSupervisorUsers = userSharedService.GetUsersInRole("ProductionSupervisor").Result;

            if (ProductionSupervisorUsers != null && ProductionSupervisorUsers.Count!=0)
            {

                return Json(new { result = "ok", data = ProductionSupervisorUsers });
            }
            return Json(new { result = "ok", data = "noData" });
        }
        public IActionResult AdminPermission()
        {

            return Content("This is not Actual Action");
        }
        public IActionResult ShowCommentModule(int id)
        {

            return PartialView("_Comment", id);
        }
        public IActionResult FromCreatorToProductionSupervisorCartablePermission()
        {
            return Content("This is not Actual Action");
        }
        public IActionResult FromCreatorToProductionManagerCartablePermission()
        {
            return Content("This is not Actual Action");
        }
        public IActionResult FromSupervisorToProductionMangerCartablePermission()
        {
            return Content("This is not Actual Action");
        }
        public IActionResult FromProductionManagerToQCManagerPermission()
        {
            return Content("This is not Actual Action");
        }
        public IActionResult FromProductionManagerToCreatorPermission()
        {
            return Content("This is not Actual Action");
        }
        public IActionResult FromQCManagerToCreatorCartablePermission()
        {
            return Content("This is not Actual Action");
        }
        public IActionResult InsertFormDataPermoission()
        {
            return Content("This is not Actual Action");
        }
        public IActionResult FinalApproveForm(int incomingGoodsInspectionId, bool finalApprove)
        {

            var selectedRowData = incomingGoodsInspectionLogic.GetRow(incomingGoodsInspectionId);
            if (selectedRowData.ResultStatus != OperationResultStatus.Successful || selectedRowData.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[selectedRowData.AllMessages] });
            }
            if (selectedRowData.ResultEntity.FinalApprove.HasValue && selectedRowData.ResultEntity.InspectionFormStatus==InspectionFormStatus.ProcessCompleted)
            {

                return Json(new { result = "fail", message = localizer["Form Data is Already Approved"] });
            }
            selectedRowData.ResultEntity.FinalApprove = finalApprove;
            selectedRowData.ResultEntity.InspectionFormStatus = InspectionFormStatus.ProcessCompleted;
            var updateResult= incomingGoodsInspectionLogic.Update(selectedRowData.ResultEntity);
            if (updateResult.ResultStatus != OperationResultStatus.Successful || !updateResult.ResultEntity)
            {
                return Json(new { result = "fail", message = localizer[updateResult.AllMessages] });
            }

            return Json(new { result = "ok", message = localizer["formData Approved Suucessfully"] });

        }
        public async Task<IActionResult> AddComment(int id, string comments)
        {

            var incomingGoodsInspectionResult = incomingGoodsInspectionLogic.GetById(id);
            if (incomingGoodsInspectionResult.ResultStatus != OperationResultStatus.Successful || incomingGoodsInspectionResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer["Inspection Not Found"] });
            }

            var cartableItems = incomingGoodsInspectionLogic.GetStepCartableItems(incomingGoodsInspectionResult.ResultEntity, comments);

            if (!cartableItems.hasPermission)
            {

                return Json(new { result = "fail", message = localizer["You Are Not Authorized For Referral Form Based On Form Status"] });
            }

            incomingGoodsInspectionResult.ResultEntity.InspectionFormStatus=cartableItems.InspectionFormStatus;
            var updateResult = incomingGoodsInspectionLogic.Update(incomingGoodsInspectionResult.ResultEntity);



            var addResult = await incomingGoodsInspectionCartableItemLogic.BulkInsertAsync(cartableItems.IncomingGoodsInspectionCartableItems);

            if (addResult.ResultStatus != OperationResultStatus.Successful || addResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[addResult.AllMessages] });
            }

            return Json(new { result = "ok", message = localizer["data Has Been Saved Successfully"] });
        }
        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetIncomingGoodsInspectionGridData(DatatablesSentModel model, string? qualityInspectionNumber, string? goodsTitle, string vendorName, InspectionFormStatus? inspectionFormStatus)
        {
            var isProductionManager = false;
            var listofStatus = new List<InspectionFormStatus>();

            var currentUser = userSharedService.GetUserById(userPrincipal.CurrentUserId).Result;

            var userRoles = userSharedService.GetRolesOfUser(currentUser).Result;

            if (userRoles.Contains("ProductionManager"))
            {
                isProductionManager = true;
                listofStatus.Add(InspectionFormStatus.ReferralToProductionManager);
                listofStatus.Add(InspectionFormStatus.ReferralToSupervisor);
            }

            if (userRoles.Contains("ProductionSupervisor"))
            {
                listofStatus.Add(InspectionFormStatus.ReferralToSupervisor);
            }


            var isAdmin = userPrincipal.CurrentUser.HasClaim("Permission", ":IncomingGoodsInspection:AdminPermission");

            var jobApplicantsResult = incomingGoodsInspectionLogic.GetByFilter(listofStatus, isProductionManager, isAdmin, qualityInspectionNumber, goodsTitle, vendorName, inspectionFormStatus, model.Start, model.Length);

            if (jobApplicantsResult.ResultStatus != OperationResultStatus.Successful || jobApplicantsResult.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[jobApplicantsResult.AllMessages] });
            }
            var totalCount = jobApplicantsResult?.Count ?? 0;
            return Json(new { model.Draw, recordsTotal = totalCount, recordsFiltered = totalCount, data = jobApplicantsResult?.ResultEntity, error = "", result = "ok" });
        }
        [ParentalAuthorize(nameof(Index))]
        public async Task<IActionResult> FetchRahkaranData(string number)
        {
            var result = await rahkaranService.GetQualityInspectionData(number);
            if (result.Any())
            {
                ViewBag.GoodsInfo=result;
                return Json(new
                {
                    message = "ok",
                    results = result
                });
            }
            else
            {
                return Json(new
                {
                    message = "fail",
                    results = new List<QualityInspectionResultModel>()
                });
            }
        }
        private List<SelectListItem> GetCategories()
        {

            var result = new List<SelectListItem>();
            var data = controlPlanCategoryLogic.GetAll();
            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }
            return data.ResultEntity.ToSelectList(nameof(ControlPlanCategoryModel.Title), nameof(ControlPlanCategoryModel.ControlPlanCategoryId));
        }
        private List<IncomingGoodsInspectionItemModel> GetCategoryItems(int categoryId)
        {
            var result = new List<IncomingGoodsInspectionItemModel>();
            var data = controlPlanLogic.GetByCategoryId(categoryId);
            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }
            result = data.ResultEntity.Select(x => new IncomingGoodsInspectionItemModel
            {
                ControlPlanId = x.ControlPlanId,
                ControlPlanCategoryTitle=x.ControlPlanCategoryTitle,
                ControlPlanParameter=x.ControlPlanParameter,
                QuantityDescription=x.QuantityDescription,
                AcceptanceCriteria=x.AcceptanceCriteria,
            }).ToList();
            return result;
        }
        [ParentalAuthorize(nameof(Index))]
        public IActionResult GetCategoryPartial(int categoryId)
        {
            var data = GetCategoryItems(categoryId);
            return PartialView("_ControlPlan", data);
        }
        public override IActionResult Save([FromServices] ILogic<IncomingGoodsInspectionModel> service, IncomingGoodsInspectionModel model)
        {

            if (model.IncomingGoodsInspectionId != 0)
            {
                return Json(new
                {
                    result = "fail",
                    message = localizer["Your data has not been saved"],
                    title = sharedLocalizer["SaveTitle"]
                });
            }

            model.CreatedBy=userPrincipal.CurrentUserId;
            model.CreateDate=DateTime.Now;

            if (model.IncomingGoodsInspectionItems!=null && model.IncomingGoodsInspectionItems.Any(x => x.IsMatch==null))
            {
                return Json(new { result = "fail", message = localizer["Check IsMatch And IsNot Match Options"] });
            }
            var files = Request.Form.Files;
            var incomingGoodsFiles = new List<IncomingGoodsInspectionFileModel>();
            foreach (var file in files)
            {
                var uploadResult = incomingGoodsInspectionFileLogic.SaveToDataBase(file, model.IncomingGoodsInspectionId);
                incomingGoodsFiles.Add(uploadResult.ResultEntity);
            }
            model.IncomingGoodsInspectionFiles = incomingGoodsFiles;
            var cartableItems = incomingGoodsInspectionLogic.GetStepCartableItems(model, " ");

            if (!cartableItems.hasPermission)
            {

                return Json(new { result = "fail", message = localizer["You Are Not Authorized For Referral Form Based On Form Status"] });
            }

            model.IncomingGoodsInspectionCartableItems = cartableItems.IncomingGoodsInspectionCartableItems;
            model.InspectionFormStatus=cartableItems.InspectionFormStatus;

            if (model.IncomingGoodsInspectionCartableItems==null || model.IncomingGoodsInspectionCartableItems.Count==0)
            {
                model.InspectionFormStatus=InspectionFormStatus.ProcessCompleted;
            }

            if (!model.IsSampleGoods && !model.HasFunctionalTest && !model.FinalApprove.HasValue)
            {
                return Json(new { result = "fail", message = localizer["Please Check Final Approve Status"] });
            }

            if (model.FinalApprove.HasValue)
            {
                model.InspectionFormStatus=InspectionFormStatus.ProcessCompleted;
            }


            if ((model.HasFunctionalTest || model.IsSampleGoods) && model.FinalApprove.HasValue && model.FinalApprove.Value) {

                model.FinalApprove=null;
            
            }

            var addResult = service.AddNew(model);

            if (addResult.ResultStatus != OperationResultStatus.Successful)
            {
                return Json(new
                {
                    result = "fail",
                    message = addResult.AllMessages,
                    title = sharedLocalizer["SaveTitle"]
                });
            }
            return Json(new
            {
                result = "ok",
                message = sharedLocalizer["Your data has been saved"],
                title = sharedLocalizer["SaveTitle"],
                redirectUrl = "/IncomingGoodsInspection/Index"
            });
        }
        public IActionResult GetIncomingGoodsInspectionItems(int incomingGoodsInspectionId)
        {
            var downloadurl = configuration.GetSection("Attachment").Get<AttachmentSection>().DownloadUrl;

            var result = incomingGoodsInspectionLogic.GetById(incomingGoodsInspectionId);

            var userInfo = userSharedService.GetUserById(result.ResultEntity.CreatedBy).Result;

            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity is null)
            {
                return Json(new { result = "fail", message = localizer[result.AllMessages] });
            }

            result.ResultEntity.CreatedByUserName=userInfo.Username;

            if (result.ResultEntity.IncomingGoodsInspectionFiles != null && result.ResultEntity.IncomingGoodsInspectionFiles.Any())
            {

                foreach (var item in result.ResultEntity.IncomingGoodsInspectionFiles)
                {
                    item.DownloadUrl = $"{downloadurl}{item.AttachmentId}";
                }
            }

            var userIdList = new List<Guid>();

            var userIds = result.ResultEntity.IncomingGoodsInspectionCartableItems.Select(x => x.UserId).ToList();
            var referredByIds = result.ResultEntity.IncomingGoodsInspectionCartableItems.Select(x => x.ReferredBy).ToList();

            userIdList.AddRange(userIds);
            userIdList.AddRange(referredByIds);

            var usersInformation = userSharedService.GetUserInfos(userIdList);

            foreach (var item in result.ResultEntity.IncomingGoodsInspectionCartableItems)
            {
                var currentuserInfo = usersInformation.FirstOrDefault(x => x.UserId==item.UserId);
                var currentReferredByInfo = usersInformation.FirstOrDefault(x => x.UserId==item.ReferredBy);

                item.ReferredToName=(currentuserInfo!=null) ? currentuserInfo.Username : "بدون ارجاع";
                item.ReferredByName=(currentReferredByInfo!=null) ? currentReferredByInfo.Username : "-";
            }

            return PartialView("_IncomingGoodsInspectionItems", result.ResultEntity);
        }
    }
}
