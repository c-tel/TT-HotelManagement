using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTHohel.Manager;
using TTHohel.Services;
using TTHotel.Contracts.Clients;

namespace TTHohel.Models
{
    class ClientModel
    {

        public bool CreateNewClient(ClientDTO clientDTO)
        {
            if (HotelApiClient.GetInstance().CreateClient(clientDTO))
            {
                Storage.Instance.ChangeAllClients(clientDTO);
                return true;
            }
            return false;
        }

        public void GoBack()
        {
            NavigationManager.Instance.Navigate(ModesEnum.AddBooking);
        }
    }
}
