using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TTHohel.Contracts.Bookings;
using TTHotel.API.Services;
using TTHotel.Contracts.Bookings;

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

        [HttpGet()]
        public IEnumerable<RoomInfo> PeriodInfo([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            return _hotelService.GetPeriodInfo(from, to);
        }

        [HttpGet("{id}")]
        public BookingDTO ById([FromRoute] int id)
        {
            return _hotelService.GetBooking(id);
        }

        
    }
}
