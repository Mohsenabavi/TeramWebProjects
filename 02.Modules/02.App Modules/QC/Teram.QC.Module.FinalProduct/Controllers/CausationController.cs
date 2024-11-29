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

            if (relatedNonComplianceResult.ResultEntity == null)
            {
                return Json(new { result = "fail", message = localizer["Related entity not found."] });
            }

            // Update relatedNonComplianceResult entity
            var relatedEntity = relatedNonComplianceResult.ResultEntity;
            relatedEntity.FormStatus = Enums.FormStatus.DeterminingReason;
            relatedEntity.LastComment = " ";
            relatedEntity.HasCausation = true;
            relatedEntity.IsTriggeredByUserAction = true;
            relatedEntity.DestinationUser = null;

            var causationResult = causationLogic.GetByFinalProductNonComplianceId(model.FinalProductNoncomplianceId);

            if (causationResult.ResultStatus == OperationResultStatus.Successful && causationResult.ResultEntity != null)
            {
                // Update causation logic
                var causationUpdateResult = causationLogic.Update(model);
                if (causationUpdateResult.ResultStatus != OperationResultStatus.Successful)
                {
                    // Log error details (optional)
                    return Json(new { result = "fail", message = localizer[causationUpdateResult.AllMessages] });
                }
            }
            else
            {
                // Insert new causation logic
                var causationInsertResult = causationLogic.AddNew(model);
                if (causationInsertResult.ResultStatus != OperationResultStatus.Successful)
                {
                    // Log error details (optional)
                    return Json(new { result = "fail", message = localizer[causationInsertResult.AllMessages] });
                }
            }

            // Update finalProductNoncomplianceLogic
            var updateResult = finalProductNoncomplianceLogic.Update(relatedEntity);

            if (updateResult.ResultStatus != OperationResultStatus.Successful)
            {
                // Log error details (optional)
                return Json(new { result = "fail", message = localizer[updateResult.AllMessages] });
            }

            return Json(new { result = "ok", message = localizer["Saved Successfully"] });

        }
    }
}
