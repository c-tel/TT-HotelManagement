using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Services;
using TTHohel.Tools;
using TTHotel.Contracts.Clients;

namespace TTHohel.ViewModels
{
    public class ClientsListViewModel: INotifyPropertyChanged
    {
        #region Private Fields
        private string _clientsFilter;

        private ClientAnalisedDTO _selectedClient;
        private DataGridCellInfo _cellInfo;

        private List<ClientAnalisedDTO> _clientsList;
        private List<ClientAnalisedDTO> _filteredClientsList;

        private ClientsListModel Model { get; }

        private ICommand _clientInfoCommand;
        #endregion
        public ClientsListViewModel()
        {
            Model = new ClientsListModel();
            ClientsList = Model.GetClientsList();
            Model.AllClientsChanged += OnClientsChanged;
        }


        #region Properties
        public DataGridCellInfo CellInfo
        {
            get { return _cellInfo; }
            set
            {
                _cellInfo = value;
                InvokePropertyChanged(nameof(CellInfo));
            }
        }

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

        public ICommand ClientInfoCommand
        {
            get
            {
                if (_clientInfoCommand == null)
                {
                    _clientInfoCommand = new RelayCommand<object>(ClientInfoExecute, ClientInfoCanExecute);
                }
                return _clientInfoCommand;
            }
            set
            {
                _clientInfoCommand = value;
                InvokePropertyChanged(nameof(ClientInfoCommand));
            }
        }

        private bool ClientInfoCanExecute(object obj)
        {
            return true;
        }

        private void ClientInfoExecute(object obj)
        {
            var selectedSell = obj as DataGridCellInfo?;
            var selectedInfo = selectedSell.Value.Item as ClientAnalisedDTO;
            //var column = selectedSell.Value.Column.Header.ToString();

            //var dateColumn = selectedRoomInfo.DailyInfo.FirstOrDefault(x => x.BookDate.ToString("dd-MM-yyyy") == column);

            Model.GoToClient(selectedInfo);
        }

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
