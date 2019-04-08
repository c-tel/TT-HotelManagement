using System;
using System.Collections.Generic;
using System.Linq;
using TTHohel.Manager;
using TTHohel.Services;
using TTHotel.Contracts.Clients;

namespace TTHohel.Models
{
    public class ClientsListModel
    {
        public event Action<ClientDTO> AllClientsChanged;

        public ClientsListModel()
        {
            Storage.Instance.AllClientsChanged += OnClientsChanged;
        }

        private void OnClientsChanged(ClientDTO clientDTO)
        {
            AllClientsChanged?.Invoke(clientDTO);
        }

        public List<ClientAnalisedDTO> ApplyFilter(List<ClientAnalisedDTO> clientsList, string clientsFilter)
        {
            var lowerFilter = clientsFilter.ToLower();
            return clientsList
                    .Where(c => c.Name.ToLower().Contains(lowerFilter) ||
                                c.Surname.ToLower().Contains(lowerFilter) ||
                                c.TelNum.ToLower().Contains(lowerFilter))
                    .ToList();
        }

        public List<ClientAnalisedDTO> GetClientsList()
        {
            return HotelApiClient.GetInstance().GetAnalisedClients();
        }

        public List<ClientDTO> GetClientsSuitList()
        {
            return HotelApiClient.GetInstance().GetAnalisedSuitClients();
        }

        public void GoToClient(ClientAnalisedDTO selectedClient)
        {
            var client = HotelApiClient.GetInstance().GetClient(selectedClient.TelNum);

            var data = new ClientDisplayData
            {
                Client = client,
                Mode = ClientViewModes.Editing,
                CameFrom = ModesEnum.Statistic
            };
            Storage.Instance.ChangeClientDisplayData(data);

            NavigationManager.Instance.Navigate(ModesEnum.Client);
        }
    }
}
