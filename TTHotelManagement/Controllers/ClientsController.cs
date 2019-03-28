using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTHotel.API.Services;
using TTHotel.Contracts.Clients;

namespace TTHotel.API.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientsController: ControllerBase
    {
        private readonly IHotelService _hotelService;
        public ClientsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet()]
        public IEnumerable<ClientDTO> GetAll()
        {
            return _hotelService.GetClients();
        }

        [HttpPost()]
        public ActionResult<IEnumerable<ClientDTO>> GetAll([FromBody] ClientDTO client)
        {
            _hotelService.CreateClient(client);
            return NoContent();
        }
    }
}
