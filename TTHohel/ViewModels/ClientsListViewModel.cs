using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Services;
using TTHotel.Contracts.Clients;

namespace TTHohel.ViewModels
{
    public class ClientsListViewModel: INotifyPropertyChanged
    {
        #region Private Fields
        private string _clientsFilter;

        private ClientAnalisedDTO _selectedClient;

        private List<ClientAnalisedDTO> _clientsList;
        private List<ClientAnalisedDTO> _filteredClientsList;

        private ClientsListModel Model { get; }

        private ICommand _addClientCommand;
        private ICommand _clientInfoCommand;
        #endregion
        public ClientsListViewModel()
        {
            Model = new ClientsListModel();
            ClientsList = Model.GetClientsList();
            Model.AllClientsChanged += OnClientsChanged;
        }
        

        #region Properties
        public ClientAnalisedDTO SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                if (_selectedClient != value)
                {
                    _selectedClient = value;
                    InvokePropertyChanged(nameof(SelectedClient));
                }
            }
        }
        public string ClientsFilter
        {
            get { return _clientsFilter; }
            set
            {
                _clientsFilter = value;
                RefreshFilteredClients();
                InvokePropertyChanged(nameof(ClientsFilter));
            }
        }

        public List<ClientAnalisedDTO> ClientsList
        {
            get { return _clientsList; }
            set
            {
                _clientsList = value;
                RefreshFilteredClients();
                InvokePropertyChanged(nameof(ClientsList));
            }
        }

        public List<ClientAnalisedDTO> FilteredClientsList
        {
            get { return _filteredClientsList; }
            set
            {
                _filteredClientsList = value;
                InvokePropertyChanged(nameof(FilteredClientsList));
            }
        }
        #endregion

        private void OnClientsChanged(ClientDTO clientDTO)
        {
            ClientsList = Model.GetClientsList();
            SelectedClient = FilteredClientsList.FirstOrDefault(x => x.TelNum == clientDTO.TelNum);
        }

        private void RefreshFilteredClients()
        {
            if (string.IsNullOrEmpty(ClientsFilter))
                FilteredClientsList = ClientsList;
            else
                FilteredClientsList = Model.ApplyFilter(ClientsList, ClientsFilter);
        }

        

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void InvokePropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }
        #endregion
    }
}
