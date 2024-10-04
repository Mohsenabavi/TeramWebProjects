using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities.Causation;
using Teram.QC.Module.FinalProduct.Logic;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models.CausationModels;
using Teram.ServiceContracts;
using Teram.Web.Core;
using Teram.Web.Core.ControlPanel;

namespace Teram.QC.Module.FinalProduct.Controllers
{
    public class CausationController : ControlPanelBaseController<CausationModel, Causation, int>
    {
        private readonly IFinalProductNoncomplianceLogic finalProductNoncomplianceLogic;
        private readonly ICausationLogic causationLogic;

        public CausationController(ILogger<CausationController> logger, IFinalProductNoncomplianceLogic finalProductNoncomplianceLogic
            , IStringLocalizer<CausationController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer, ICausationLogic causationLogic)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<CausationModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["models"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["models"],
                OperationColumns = true,
                HomePage = nameof(CausationController).Replace("Controller", "") + "/index",
            };
            this.finalProductNoncomplianceLogic = finalProductNoncomplianceLogic ?? throw new ArgumentNullException(nameof(finalProductNoncomplianceLogic));
            this.causationLogic = causationLogic ?? throw new ArgumentNullException(nameof(causationLogic));
        }
        public IActionResult Index()
        {
            return View(Model);
        }

        public IActionResult HasLimitedAccessForCausation()
        {
            return Content("This Is Permission Actioin");
        }
        public override IActionResult Save([FromServices] ILogic<CausationModel> service, CausationModel model)
        {
            var relatedNonComplianceResult = finalProductNoncomplianceLogic.GetById(model.FinalProductNoncomplianceId);

            relatedNonComplianceResult.ResultEntity.FormStatus = Enums.FormStatus.DeterminingReason;

            relatedNonComplianceResult.ResultEntity.LastComment = " ";

            relatedNonComplianceResult.ResultEntity.HasCausation = true;

            relatedNonComplianceResult.ResultEntity.IsTriggeredByUserAction = true;

            relatedNonComplianceResult.ResultEntity.DestinationUser = null;

            var updateResult = finalProductNoncomplianceLogic.Update(relatedNonComplianceResult.ResultEntity);

            var causationResult = causationLogic.GetByFinalProductNonComplianceId(model.FinalProductNoncomplianceId);

            if (causationResult.ResultStatus == OperationResultStatus.Successful && causationResult.ResultEntity is not null)
            {
                causationLogic.Update(model);
                return Json(new { result = "ok", message = localizer["Saved Successfully"] });
            }
            else
            {
                causationLogic.AddNew(model);
                return Json(new { result = "ok", message = localizer["Saved Successfully"] });
            }
        }
    }
}
