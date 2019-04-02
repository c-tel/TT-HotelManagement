using System;
using System.Collections.Generic;
using TTHohel.Manager;
using TTHohel.Services;
using TTHotel.Contracts.Clients;
using TTHotel.Contracts.Rooms;

namespace TTHohel.Models
{
    class AddBookingModel
    {

        public List<ClientDTO> GetClientsList()
        {
            return HotelApiClient.GetInstance().GetAllClients();
        }

        public List<RoomDTO> GetRoomsList()
        {
            return HotelApiClient.GetInstance().GetRooms();
        }
    }
}
