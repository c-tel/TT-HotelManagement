using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;
using TTHotel.Contracts.Clients;
using TTHotel.Contracts.Rooms;

namespace TTHohel.ViewModels
{
    class AddBookingViewModel : INotifyPropertyChanged
    {
        #region Private Fields
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private string _commentText;
        private double _periodPrice;
        private int _places;
        private string _comforts;
        private string _clientsFilter;
        private DateTime _endDateDisplay;

        private ClientDTO _selectedClient;
        private RoomDTO _selectedRoom;

        private List<ClientDTO> _clientsList;
        private List<ClientDTO> _filteredClientsList;
        private List<RoomDTO> _roomsList;

        private AddBookingModel Model { get; }

        private ICommand _createBookingCommand;
        private ICommand _addClientCommand;
        private ICommand _clientInfoCommand;
        private ICommand _backCommand;
        #endregion

        public AddBookingViewModel()
        {
            Model = new AddBookingModel();

            Model.AllClientsChanged += OnClientsChanged;

            Init();
            
            ClientsList = Model.GetClientsList();
        }

        #region Properties

        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set
            {
                if (_dateFrom != value)
                {
                    _dateFrom = value;
                    InvokePropertyChanged(nameof(DateFrom));
                    RefreshFreeRooms();
                    UpdateEndDisplayDate();
                }
            }
        }

        public DateTime DateTo
        {
            get { return _dateTo; }
            set
            {
                if (_dateTo != value)
                {
                    _dateTo = value;
                    InvokePropertyChanged(nameof(DateTo));
                    RefreshFreeRooms();
                }
            }
        }

        public DateTime StartDateDisplay { get; set; }

        public DateTime EndDateDisplay
        {
            get { return _endDateDisplay; }
            set
            {
                if (_endDateDisplay != value)
                {
                    _endDateDisplay = value;
                    InvokePropertyChanged(nameof(EndDateDisplay));
                }
            }
        }

        public string CommentText
        {
            get { return _commentText; }
            set
            {
                if (_commentText != value)
                {
                    _commentText = value;
                    InvokePropertyChanged(nameof(CommentText));
                }
            }
        }

        public double PeriodPrice
        {
            get { return _periodPrice; }
            set
            {
                _periodPrice = value;
                InvokePropertyChanged(nameof(PeriodPrice));
            }
        }

        public int Places
        {
            get { return _places; }
            set
            {
                _places = value;
                InvokePropertyChanged(nameof(Places));

                RefreshFreeRooms();
            }
        }

        public string Comforts
        {
            get { return _comforts; }
            set
            {
                _comforts = value;
                InvokePropertyChanged(nameof(Comforts));
            }
        }

        public ClientDTO SelectedClient
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

        public RoomDTO SelectedRoom
        {
            get { return _selectedRoom; }
            set
            {
                if (_selectedRoom != value)
                {
                    _selectedRoom = value;
                    InvokePropertyChanged(nameof(SelectedRoom));
                    RefreshDependentObjects();
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

        public List<ClientDTO> ClientsList
        {
            get { return _clientsList; }
            set
            {
                _clientsList = value;
                RefreshFilteredClients();
                InvokePropertyChanged(nameof(ClientsList));
            }
        }

        public List<ClientDTO> FilteredClientsList
        {
            get { return _filteredClientsList; }
            set
            {
                _filteredClientsList = value;
                InvokePropertyChanged(nameof(FilteredClientsList));
            }
        }

        public List<RoomDTO> RoomsList
        {
            get { return _roomsList; }
            set
            {
                _roomsList = value;
                InvokePropertyChanged(nameof(RoomsList));
            }
        }
        #endregion

        #region Commands

        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                    _backCommand = new RelayCommand<object>(BackExecute, BackCanExecute);
                return _backCommand;
            }
            set
            {
                _backCommand = value;
                InvokePropertyChanged(nameof(BackCommand));
            }
        }

        private bool BackCanExecute(object obj)
        {
            return true;
        }

        private void BackExecute(object obj)
        {
            Model.GoToMain();
            Init();
        }

        public ICommand CreateBookingCommand
        {
            get
            {
                if (_createBookingCommand == null)
                    _createBookingCommand = new RelayCommand<object>(CreateBookingExecute, CreateBookingCanExecute);
                return _createBookingCommand;
            }
            set
            {
                _createBookingCommand = value;
                InvokePropertyChanged(nameof(CreateBookingCommand));
            }
        }

        private bool CreateBookingCanExecute(object obj)
        {
            return SelectedRoom != null && SelectedClient != null;
        }

        private void CreateBookingExecute(object obj)
        {
            if (Model.CreateNewBooking(DateFrom, DateTo, SelectedRoom.Num, SelectedClient.TelNum, CommentText))
            {
                Model.GoToMain();
                Init();
            }
            else
                MessageBox.Show("Щось пішло не так...", "Помилка");
        }

        public ICommand AddClientCommand
        {
            get
            {
                if (_addClientCommand == null)
                    _addClientCommand = new RelayCommand<object>(AddClientExecute, AddClientCanExecute);
                return _addClientCommand;
            }
            set
            {
                _addClientCommand = value;
                InvokePropertyChanged(nameof(AddClientCommand));
            }
        }

        private bool AddClientCanExecute(object obj)
        {
            return true;
        }

        private void AddClientExecute(object obj)
        {
            Model.GoToAddClient();
        }

        public ICommand ClientInfoCommand
        {
            get
            {
                if (_clientInfoCommand == null)
                    _clientInfoCommand = new RelayCommand<object>(ClientInfoExecute, ClientInfoCanExecute);
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
            return SelectedClient != null;
        }

        private void ClientInfoExecute(object obj)
        {
            Model.GoToClient(SelectedClient);
        }
        #endregion

        #region Private Methods
        private void UpdateEndDisplayDate()
        {
            EndDateDisplay = DateFrom.AddDays(1);
            if(DateTo != null && DateTo < EndDateDisplay)
                DateTo = EndDateDisplay;
        }

        private void OnClientsChanged(ClientDTO clientDTO)
        {
            ClientsList = Model.GetClientsList();
            SelectedClient = FilteredClientsList.FirstOrDefault(x => x.TelNum == clientDTO.TelNum);
        }

        private void RefreshFreeRooms()
        {
            RoomsList = Model.GetRoomsList(DateFrom, DateTo, Places);
        }

        private void RefreshDependentObjects()
        {
            if (SelectedRoom != null)
            {
                Comforts = Model.GetSelectedRoomComforts(SelectedRoom);
                PeriodPrice = Model.CalculatePeriodPrice(DateFrom, DateTo, SelectedRoom.Price, SelectedClient);
            }
            else PeriodPrice = 0;
        }

        private void RefreshFilteredClients()
        {
            if (string.IsNullOrEmpty(ClientsFilter))
                FilteredClientsList = ClientsList;
            else
                FilteredClientsList = Model.ApplyFilter(ClientsList, ClientsFilter);
        }

        private void Init()
        {
            DateFrom = DateTime.Today.Date;
            DateTo = DateTime.Today.AddDays(1);
            SelectedClient = null;
            SelectedRoom = null;
            Places = 1;
            CommentText = null;
            StartDateDisplay = DateTime.Now;
        }
        #endregion

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
