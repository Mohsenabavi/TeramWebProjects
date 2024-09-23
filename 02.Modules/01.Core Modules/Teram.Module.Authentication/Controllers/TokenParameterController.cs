using System;
using System.ComponentModel.DataAnnotations;
using Teram.Framework.Core.Logic;
using Teram.Module.Authentication.Models;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Teram.Module.Authentication.Controllers
{
    [Display(Description = "Token Parameter")]
    public class TokenParameterController : ControlPanelBaseController<TokenParameterModel, Entities.TokenParameter, int>
    {
        private readonly IServiceProvider serviceProvider;

        public TokenParameterController(IServiceProvider serviceProvider, ILogger<TokenParameterController> logger,
             IStringLocalizer<TokenParameterController> localizer, IStringLocalizer<SharedResource> sharedlocalizer)  
        {
            this.logger = logger;
            this.sharedLocalizer = sharedlocalizer;
            this.localizer = localizer ?? throw new ArgumentNullException(nameof(localizer)); 

            ViewBag.PageName = localizer["Token Parameters"];
            Model = new ViewInformation<TokenParameterModel>(true)
            {
                Title = localizer["Token Parameters"],
                HomePage = "/TokenParameter/Index",
                ExtraScripts = "/ExternalModule/Module/Authentication/Scripts/TokenParameter.js",
                HasGrid = true,
                EditInSamePage = true

            };

        }

        [Display(Description = "ShowPage")]
        [ControlPanelMenu("Token Parameter", ParentName = "Security", Icon = "fa-key", PanelType = PanelType.Managment, Position = Web.Core.Enums.ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View("Index", Model);
        }


        protected override void ModifyItem(ILogic<TokenParameterModel> service, int id)
        {
           
            var result = service.GetRow(id);
            if (result.ResultStatus == OperationResultStatus.Successful)
            {
                Model.ModelData = result.ResultEntity;
            }

            Model.ModelData = result.ResultEntity;
            Model.Key = result.ResultEntity.Key;
        }
   
 
    }

}