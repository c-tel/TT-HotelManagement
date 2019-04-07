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
        public event Action<ClientViewModes> ClientDisplayChanged;

        public ClientModel()
        {
            Storage.Instance.ClientDisplayChanged += OnDisplayDataChanged;
        }

        private void OnDisplayDataChanged(ClientDisplayData data)
        {
            ClientDisplayChanged?.Invoke(data.Mode);
        }

        public int CreateNewClient(ClientDTO clientDTO)
        {
            var res = HotelApiClient.GetInstance().CreateClient(clientDTO);

            if (res == System.Net.HttpStatusCode.NoContent)
            {
                Storage.Instance.ChangeAllClients(clientDTO);
                return 1;
            }
            if (res == System.Net.HttpStatusCode.Conflict)
                return 2;

            return 0;
        }

        public void GoBack()
        {
            NavigationManager.Instance.Navigate(ModesEnum.AddBooking);
        }

        internal void ChangeDisplayData(ClientDTO client, ClientViewModes info)
        {
            throw new NotImplementedException();
        }
    }
}
