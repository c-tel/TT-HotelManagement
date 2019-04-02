using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTHohel.Services;
using TTHotel.Contracts.Clients;

namespace TTHohel.Models
{
    class AllClientsModel
    {

        public List<ClientDTO> GetClientsList()
        {
            return HotelApiClient.GetInstance().AllClients();
        }

    }
}
