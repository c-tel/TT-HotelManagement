using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using TTHohel.Manager;
using TTHohel.Services;
using TTHotel.Contracts.Clients;

namespace TTHohel.Models
{
    class ClientModel
    {
        private ModesEnum _cameFrom;

        public event Action<ClientDisplayData> ClientDisplayChanged;

        public ClientModel()
        {
            Storage.Instance.ClientDisplayChanged += OnDisplayDataChanged;
        }

        private void OnDisplayDataChanged(ClientDisplayData data)
        {
            _cameFrom = data.CameFrom;
            ClientDisplayChanged?.Invoke(data);
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
            NavigationManager.Instance.Navigate(_cameFrom);
        }

        public void SaveClient(ClientDTO client, string oldTel)
        {
            MessageBox.Show(oldTel);
        }
    }
}
