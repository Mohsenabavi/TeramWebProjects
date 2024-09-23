using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Teram.Framework.Core.Extensions;
using Teram.Framework.Core.Logic;
using Teram.Module.GeographicRegion.Enums;
using Teram.Module.GeographicRegion.Logic;
using Teram.Module.GeographicRegion.Logic.Interfaces;
using Teram.Module.GeographicRegion.Models;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;
using Teram.Web.Core.Extensions;
using Teram.Module.GeographicRegion.ExtensionMethods;
using Teram.ServiceContracts;

namespace Teram.Module.GeographicRegion.Controllers
{
    public class GeographicRegionController : ControlPanelBaseController<GeographicRegionModel, Entities.GeographicRegion, int>
    {
        private readonly IGeographicRegionLogic geographicRegionLogic;

        public GeographicRegionController(ILogger<GeographicRegionController> logger
            , IStringLocalizer<GeographicRegionController> localizer, IGeographicRegionLogic geographicRegionLogic,
            IStringLocalizer<SharedResource> sharedLocalizer)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<GeographicRegionModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["GeographicRegion"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["GeographicRegion"],
                OperationColumns = true,
                HasToolbar = true,
                ToolbarName = "_adminToolbar",
                HomePage = nameof(GeographicRegionController).Replace("Controller", "") + "/index",
            };
            this.geographicRegionLogic = geographicRegionLogic ?? throw new ArgumentNullException(nameof(geographicRegionLogic));
        }

        [ControlPanelMenu("GeographicRegion", ParentName = "BaseSystemInfo", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            ViewBag.Parents = GetAllItems();
            return View(Model);
        }

        protected override void ModifyItem(ILogic<GeographicRegionModel> service, int id)
        {
            ViewBag.Parents = GetAllItems();
            base.ModifyItem(service, id);
        }

        public IActionResult GetAllCitiesByProvinceId(int provinceId, bool list = true)
        {
            var result = geographicRegionLogic.GetByParent(provinceId);
            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity == null)
            {
                if (list)
                {
                    return Json(new
                    {
                        message = "fail",
                        results = new { Text = "", Id = "" }
                    });
                }
                return Json(new
                {
                    message = "fail",
                    results = new List<GeographicRegionModel>()
                });
            }

            if (list)
            {
                var datalist = result.ResultEntity.Select(x => new { Text = x.Name, Id = x.GeographicRegionId.ToString() }).ToList();
                return Json(new
                {
                    message = "ok",
                    results = datalist
                });
            }

            var data = result.ResultEntity.ToList();
            return Json(new
            {
                message = "ok",
                results = data
            });
        }

        public IActionResult GetAllProvince(int country = 87, bool list = true)
        {
            var result = geographicRegionLogic.GetByParent(country);
            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity == null)
            {
                if (list)
                {
                    return Json(new
                    {
                        message = "fail",
                        results = new { Text = "", Id = "" }
                    });
                }
                return Json(new
                {
                    message = "fail",
                    results = new List<GeographicRegionModel>()
                });
            }

            if (list)
            {
                var datalist = result.ResultEntity.Select(x => new { Text = x.Name, Id = x.GeographicRegionId.ToString() }).ToList();
                return Json(new
                {
                    message = "ok",
                    results = datalist
                });
            }
            var data = result.ResultEntity.ToList();
            return Json(new
            {
                message = "ok",
                results = data
            });
        }

        public IActionResult GetById(int id, bool list = true)
        {
            var result = geographicRegionLogic.GetById(id);
            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity == null)
            {
                if (list)
                {
                    return Json(new
                    {
                        message = "fail",
                        results = new { Text = "", Id = "" }
                    });
                }
                return Json(new
                {
                    message = "fail",
                    results = new GeographicRegionModel()
                });
            }
            if (list)
            {
                var datalist = new { Text = result.ResultEntity.Name, Id = result.ResultEntity.GeographicRegionId.ToString() };
                return Json(new
                {
                    message = "ok",
                    results = datalist
                });

            }
            var data = result.ResultEntity;
            return Json(new
            {
                message = "ok",
                results = data
            });
        }

        public IActionResult GetByType(GeographicType geoType, bool list = true)
        {
            var result = geographicRegionLogic.GetByType(geoType);
            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity == null)
            {
                if (list)
                {
                    return Json(new
                    {
                        message = "fail",
                        results = new { Text = "", Id = "" }
                    });
                }
                return Json(new
                {
                    message = "fail",
                    results = new List<GeographicRegionModel>()
                });
            }

            if (list)
            {
                var datalist = result.ResultEntity.Select(x => new { Text = x.Name, Id = x.GeographicRegionId.ToString() }).ToList();
                return Json(new
                {
                    message = "ok",
                    results = datalist
                });
            }
            var data = result.ResultEntity.ToList();
            return Json(new
            {
                message = "ok",
                results = data
            });
        }

        public IActionResult GetByParentId(int parentId, bool list = true)
        {
            var result = geographicRegionLogic.GetByParent(parentId);
            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity == null)
            {
                if (list)
                {
                    return Json(new
                    {
                        message = "fail",
                        results = new { Text = "", Id = "" }
                    });
                }
                return Json(new
                {
                    message = "fail",
                    results = new List<GeographicRegionModel>()
                });
            }

            if (list)
            {
                var datalist = result.ResultEntity.Select(x => new { Text = x.Name, Id = x.GeographicRegionId.ToString() }).ToList();
                return Json(new
                {
                    message = "ok",
                    results = datalist
                });
            }
            var data = result.ResultEntity.ToList();
            return Json(new
            {
                message = "ok",
                results = data
            });
        }
        public IActionResult GetParentById(int childId, bool list = true)
        {
            var result = geographicRegionLogic.GetByChildId(childId);
            if (result.ResultStatus != OperationResultStatus.Successful || result.ResultEntity == null)
            {
                if (list)
                {
                    return Json(new
                    {
                        message = "fail",
                        results = new { Text = "", Id = "" }
                    });
                }
                return Json(new
                {
                    message = "fail",
                    results = new GeographicRegionModel()
                });
            }

            if (list)
            {
                var datalist = new { Text = result.ResultEntity.Name, Id = result.ResultEntity.GeographicRegionId.ToString() };
                return Json(new
                {
                    message = "ok",
                    results = datalist
                });
            }
            var data = result.ResultEntity;
            return Json(new
            {
                message = "ok",
                results = data
            });
        }


        public List<SelectListItem> GetAllItems()
        {
            var result = new List<SelectListItem>();
            var data = geographicRegionLogic.GetAll();
            if (data.ResultStatus != OperationResultStatus.Successful || data.ResultEntity is null)
            {
                return result;
            }
            return data.ResultEntity.ToSelectList(nameof(GeographicRegionModel.Name), nameof(GeographicRegionModel.Key));
        }

        [HttpPost]
        [ParentalAuthorize(nameof(Index))]
        public IActionResult ImportFromExcel([FromServices] IGeographicRegionLogic geographicRegionLogic)
        {
            try
            {
                if (!Request.Form.Files.Any())
                {
                    return Json(new { Result = "fail", message = "هیچ فایلی انتخاب نشده است" });
                }
                var file = Request.Form.Files[0];

                var geographicRegionDetails = new List<GeographicRegionImportModel>();

                using var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                geographicRegionDetails = geographicRegionDetails.ImportFromExcel(ms).ToList();

                foreach (var item in geographicRegionDetails.OrderBy(x => x.ParentCode))
                {
                    var parentNodeResult = new BusinessOperationResult<GeographicRegionModel>();

                    if (item.ParentCode != 0)
                    {
                        parentNodeResult = geographicRegionLogic.GetParentByCode(item.ParentCode);
                    }

                    GeographicType geographicType = (GeographicType)Teram.Module.GeographicRegion.ExtensionMethods.EnumExtensions.GetValueFromDescription<GeographicType>(item.GeographicType);
                    var InsertModel = new GeographicRegionModel
                    {
                        Code = item.Code,
                        Name = item.Name.Trim(),
                        GeographicType = geographicType,
                        LatinName = item.LatinName,
                        ParentGeographicRegionId = (parentNodeResult.ResultEntity != null) ? parentNodeResult.ResultEntity.GeographicRegionId : null,
                    };
                    geographicRegionLogic.AddNew(InsertModel);
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
