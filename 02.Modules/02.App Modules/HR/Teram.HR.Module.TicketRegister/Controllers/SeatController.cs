using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Teram.HR.Module.TicketRegister.Entities;
using Teram.HR.Module.TicketRegister.Logic.Interfaces;
using Teram.HR.Module.TicketRegister.Models;
using Teram.Web.Core;
using Teram.Web.Core.Attributes;
using Teram.Web.Core.ControlPanel;
using Teram.Web.Core.Enums;

namespace Teram.HR.Module.TicketRegister.Controllers
{

    [ControlPanelMenu("TicketRegister", Name = "TicketRegister", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar, Order = 4)]
    public class SeatController : ControlPanelBaseController<SeatModel, Seat, int>
    {
        private readonly ISeatLogic seatLogic;

        public SeatController(ILogger<SeatController> logger
            , IStringLocalizer<SeatController> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer, ISeatLogic seatLogic)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.sharedLocalizer = sharedLocalizer;

            Model = new ViewInformation<SeatModel>
            {
                EditInSamePage = true,
                GridTitle = localizer["Seats"],
                HasGrid = true,
                HasManagmentForm = true,
                Title = localizer["Seats"],
                OperationColumns = true,
                HomePage = nameof(SeatController).Replace("Controller", "") + "/index",
            };
            this.seatLogic=seatLogic??throw new ArgumentNullException(nameof(seatLogic));
        }

        [ControlPanelMenu("Seats", ParentName = "TicketRegister", Icon = "fa fa-shopping-bag", PanelType = PanelType.User, Position = ControlPanelMenuPosition.LeftSideBar)]
        public IActionResult Index()
        {
            return View(Model);
        }

        public IActionResult CreateSeats(int id)
        {
            seatLogic.CreateSeats(id);
            return View();
        }
    }
}
