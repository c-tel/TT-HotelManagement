using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TTHohel.Contracts.Bookings;
using TTHotel.API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TTHotel.API.Controllers
{
    [Route("api/bookings")]
    public class BookingsController : Controller
    {
        private IHotelService _hotelService;
        public BookingsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet("periodInfo")]
        public IEnumerable<RoomInfo> Get([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            from = DateTime.Now;
            to = DateTime.Now.AddDays(5);
            return _hotelService.GetPeriodInfo(from, to);
        }
    }
}
