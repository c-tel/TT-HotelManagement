using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TTHotel.API.Services;
using TTHotel.Contracts.Rooms;

namespace TTHotel.API.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomsController: ControllerBase
    {
        private readonly IHotelService _hotelService;
        public RoomsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet()]
        public ActionResult<IEnumerable<RoomDTO>> Rooms([FromQuery] DateTime? from, [FromQuery] DateTime? to, int guests)
        {
            if ((from == null && to != null) || (from != null && to == null))
                return BadRequest();
            return new ActionResult<IEnumerable<RoomDTO>>(_hotelService.GetRooms(from, to, guests));
        }

        [HttpGet("statistics")]
        public ActionResult<IEnumerable<RoomStatisticsDTO>> Statistics([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            return new ActionResult<IEnumerable<RoomStatisticsDTO>>(_hotelService.GetRoomStats(from, to));
        }

        [HttpGet("{num}")]
        public ActionResult<RoomDTO> Room([FromRoute] int num)
        {
            return new ActionResult<RoomDTO>(_hotelService.GetRoom(num));
        }

        [HttpPost()]
        public IActionResult Create([FromBody] RoomCreateDTO room)
        {
            if (_hotelService.GetRoom(room.Num) != null)
                return Conflict();
            _hotelService.CreateRoom(room);
            return NoContent();
        }
    }
}
