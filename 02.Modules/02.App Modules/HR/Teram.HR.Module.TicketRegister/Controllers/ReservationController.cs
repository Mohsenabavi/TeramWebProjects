using Microsoft.AspNetCore.Mvc;
using Teram.HR.Module.TicketRegister.Logic.Interfaces;
using Teram.HR.Module.TicketRegister.Models;
using Teram.ServiceContracts;
using Teram.Web.Core.ControlPanel;

namespace Teram.HR.Module.TicketRegister.Controllers
{
    public class ReservationController : BasicControlPanelController
    {
        private readonly IAreaLogic areaLogic;
        private readonly IAreaRowLogic areaRowLogic;
        private readonly ISeatLogic seatLogic;

        public ReservationController(IAreaLogic areaLogic, IAreaRowLogic areaRowLogic, ISeatLogic seatLogic)
        {
            this.areaLogic=areaLogic??throw new ArgumentNullException(nameof(areaLogic));
            this.areaRowLogic=areaRowLogic??throw new ArgumentNullException(nameof(areaRowLogic));
            this.seatLogic=seatLogic??throw new ArgumentNullException(nameof(seatLogic));
        }


        public IActionResult Index()
        {

            var areaRows = areaLogic.GetAll();

            return View(areaRows.ResultEntity);
        }


        public IActionResult GetAreaSeats(int id)
        {
            var areaSeats = seatLogic.GetAreaSeats(id);

            var groupByData = areaSeats.ResultEntity
              .GroupBy(x => x.AreaRowId)
              .Select(g => new SeatRowModel
              {
                  AreaRowId = g.Key,
                  Seats = g.Select(seat => new SeatModel
                  {
                      SeatId = seat.SeatId,
                      SeatNumber = seat.SeatNumber,
                      IsReserved = seat.IsReserved,
                      ReservedBy = seat.ReservedBy,
                      ReservationDate = seat.ReservationDate,
                      ReservedFor = seat.ReservedFor,
                      AreaRowId = seat.AreaRowId
                  }).OrderBy(x=>x.SeatNumber).ToList()
              }).ToList();

            return PartialView("_Seat", groupByData);
        }
    }
}
