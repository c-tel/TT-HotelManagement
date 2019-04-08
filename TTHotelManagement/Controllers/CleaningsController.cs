using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTHotel.API.Services;
using TTHotel.Contracts;
using TTHotel.Contracts.Cleanings;

namespace TTHotel.API.Controllers
{
    [Route("api/cleanings")]
    [ApiController]
    public class CleaningsController : ControllerBase
    {
        private IHotelService _hotelService;
        private IAuthService _authService;

        public CleaningsController(IHotelService hotelService, IAuthService authService)
        {
            _hotelService = hotelService;
            _authService = authService;
        }

        [HttpGet()]
        public IEnumerable<CleaningDTO> Cleanings()
        {
            return _hotelService.GetCleanings();
        }
    }
}