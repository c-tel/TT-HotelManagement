using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TTHohel.Contracts.Bookings;
using TTHotel.API.Services;
using TTHotel.Contracts.Bookings;
using TTHotel.Contracts.Payments;

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

        [HttpGet("{id}/payments")]
        public IEnumerable<PaymentDTO> PaymentsById([FromRoute] int id)
        {
            return _hotelService.GetPayments(id);
        }
        [HttpPost("{id}/payments")]
        public IActionResult Pay([FromRoute] int id, [FromBody] PaymentCreateDTO payment)
        {
            _hotelService.CreatePayment(id, payment);
            return NoContent();
        }

        [HttpGet("report")]
        public IEnumerable<ReportItem> PaymentsByDate([FromQuery] DateTime date)
        {
            return _hotelService.GetReport(date);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBooking([FromRoute] int id, [FromBody] BookingUpdateDTO updateDTO)
        {
            _hotelService.UpdateBooking(updateDTO, id);
            return NoContent();
        }

        [HttpPost()]
        public IActionResult Create([FromBody] BookingCreateDTO booking)
        {
            if (!Request.Cookies.ContainsKey("sessid"))
                return BadRequest();
            var sessid = Request.Cookies["sessid"];
            var persBook = _authService.GetAuthorized(sessid)?.EmplBook;
            if (persBook == null)
                return Unauthorized();
            _hotelService.CreateBooking(booking, persBook);
            return NoContent();
        }
    }
}
