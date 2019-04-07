using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
