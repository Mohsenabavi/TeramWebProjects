using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.QC.Module.IncomingGoods.Logic.Interfaces;
using Teram.QC.Module.IncomingGoods.Models;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.QC.Module.IncomingGoods.Controllers
{
    public class SheetDirectionController : BasicControlPanelController
    {
        [ControlPanelMenu("SheetDirection", ParentName = "IncomingGoods", Icon = "fa fa-arrows-alt", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index() {         
            return View();
        }
    }

    
}
