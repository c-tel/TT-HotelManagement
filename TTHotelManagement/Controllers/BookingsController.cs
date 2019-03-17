using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TTHohel.Contracts.Bookings;
using TTHotel.API.Services;
using TTHotel.Contracts.Bookings;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TTHotel.API.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingsController : Controller
    {
        private IHotelService _hotelService;
        private IAuthService _authService;
        public BookingsController(IHotelService hotelService, IAuthService authService)
        {
            _hotelService = hotelService;
            _authService = authService;
        }

        [HttpGet()]
        public IEnumerable<RoomInfo> PeriodInfo([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            //CultureInfo provider = CultureInfo.InvariantCulture;
            //Convert.ToDateTime(from);
            return _hotelService.GetPeriodInfo(from, to);
        }

        [HttpGet("{id}")]
        public BookingDTO ById([FromRoute] int id)
        {
            return _hotelService.GetBooking(id);
        }

        [HttpPost()]
        public IActionResult Create([FromBody] BookingCreateDTO booking)
        {
            if (!Request.Headers.ContainsKey("sessid"))
                return BadRequest();
            var sessid = Request.Headers["sessid"];
            var persBook = _authService.GetAuthorized(sessid)?.EmplBook;
            if (persBook == null)
                return Unauthorized();
            _hotelService.CreateBooking(booking, persBook);
            return NoContent();
        }

        
    }
}
