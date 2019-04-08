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

        [HttpGet("analytics")]
        public IEnumerable<ClientAnalisedDTO> GetAnalytics()
        {
            return _hotelService.GetClientAnalytics();
        }

        [HttpGet("suit")]
        public IEnumerable<ClientDTO> GetClientSuit()
        {
            return _hotelService.GetClientSuit();
        }

        [HttpGet("{telnum}")]
        public ClientDTO Get([FromRoute] string telnum)
        {
            return _hotelService.GetClient('+' + telnum);
        }

        [HttpPost()]
        public IActionResult Create([FromBody] ClientDTO client)
        {
            if (_hotelService.GetClient(client.TelNum) != null)
                return Conflict();
            _hotelService.CreateClient(client);
            return NoContent();
        }

        [HttpPut("{telnum}")]
        public ActionResult<IEnumerable<ClientDTO>> Update([FromRoute] string telnum, [FromBody] ClientDTO client)
        {
            if ('+' + telnum != client.TelNum && _hotelService.GetClient(client.TelNum) != null)
                return Conflict();
            _hotelService.UpdateClient(client, '+' + telnum);
            return NoContent();
        }


    }
}
