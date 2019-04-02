using System;
using System.Collections.Generic;
using TTHohel.Manager;
using TTHohel.Services;
using TTHotel.Contracts.Clients;

namespace TTHohel.Models
{
    class AddBookingModel
    {

        public List<ClientDTO> GetClientsList()
        {
            return HotelApiClient.GetInstance().AllClients();
        }
    }
}
