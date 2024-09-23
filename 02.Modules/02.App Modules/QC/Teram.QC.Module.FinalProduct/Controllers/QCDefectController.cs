using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.QC.Module.FinalProduct.Entities;
using Teram.QC.Module.FinalProduct.Logic;
using Teram.QC.Module.FinalProduct.Logic.Interfaces;
using Teram.QC.Module.FinalProduct.Models;
using Teram.QC.Module.FinalProduct.Models.ImportModels;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.QC.Module.FinalProduct.Controllers
{
    public class QCDefectController : ControlPanelBaseController<QCDefectModel, QCDefect, int>
    {
        private readonly IQCDefectLogic qCDefectLogic;

        public QCDefectController(ILogger<QCDefectController> logger
            , IStringLocalizer<QCDefectController> localizer, IQCDefectLogic qCDefectLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<QCDefectModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["QCDefect"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["QCDefect"],
                OperationColumns = true,
                HomePage = nameof(QCDefectController).Replace("Controller", "") + "/index",
                HasToolbar = true,
                ToolbarName="_adminToolbar"
            };
            this.qCDefectLogic=qCDefectLogic??throw new ArgumentNullException(nameof(qCDefectLogic));
        }

        [ControlPanelMenu("QCDefect", ParentName = "BaseInfoManagement", Icon = "fa fa-exclamation-triangle", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }

        [HttpPost]
        [ParentalAuthorize(nameof(Index))]
        public async Task<IActionResult> ImportFromExcel()
        {
            try
            {
                if (!Request.Form.Files.Any())
                {
                    return Json(new { Result = "fail", message = "هیچ فایلی انتخاب نشده است" });
                }
                var file = Request.Form.Files[0];

                var qcDefectsList = new List<QcDefectImportModel>();

                using var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                qcDefectsList = qcDefectsList.ImportFromExcel(ms).ToList();

                var insertList = new List<QCDefectModel>();

                foreach (var defect in qcDefectsList)
                {
                    insertList.Add(new QCDefectModel { Code=defect.Code, Description="-", IsActive=true, Title=defect.Title });
                }

                var result= await qCDefectLogic.BulkInsertAsync(insertList);

                return Json(new { Result = "ok" });
            }

            catch (Exception)
            {
                return Json(new { Result = "fail", message = "درج برخی از ردیف ها با خطا مواجه شده" });
            }
        }
    }

}
