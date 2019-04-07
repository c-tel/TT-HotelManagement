using System;
using System.Windows;
using TTHohel.Manager;
using TTHohel.Services;
using TTHotel.Contracts.Clients;

namespace TTHohel.Models
{
    class ClientModel
    {
        private ModesEnum _cameFrom;

        public event Action<RightsEnum> UserChanged;
        public event Action<ClientDisplayData> ClientDisplayChanged;

        public ClientModel()
        {
            Storage.Instance.UserChanged += OnUserChanged;
            Storage.Instance.ClientDisplayChanged += OnDisplayDataChanged;
        }

        private void OnUserChanged(User user)
        {
            UserChanged?.Invoke(user.Rights);
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

        public int SaveClient(ClientDTO client, string oldTel)
        {
            var res = HotelApiClient.GetInstance().UpdateClient(client, oldTel);

            if (res == System.Net.HttpStatusCode.NoContent)
            {
                Storage.Instance.ChangeAllClients(client);
                return 1;
            }
            if (res == System.Net.HttpStatusCode.Conflict)
                return 2;

            return 0;
        }
    }
}
