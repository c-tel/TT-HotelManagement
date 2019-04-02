using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private double _periodPrice;

        private RoomDTO _selectedRoom;

        private List<ClientDTO> _clientsList;
        private List<RoomDTO> _roomsList;

        //private BookingDTO _bookingDTO;
        private AddBookingModel Model { get; }

        //private ICommand _addClientCommand;
        #endregion

        public AddBookingViewModel()
        {
            Model = new AddBookingModel();

            DateFrom = DateTime.Today.Date;
            DateTo = DateTime.Today.AddDays(1);

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

        public double PeriodPrice
        {
            get { return _periodPrice; }
            set
            {
                _periodPrice = value;
                InvokePropertyChanged(nameof(PeriodPrice));
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
                    RefreshPeriodPrice();
                }
            }
        }

        public List<ClientDTO> ClientsList
        {
            get { return _clientsList; }
            set
            {
                _clientsList = value;
                InvokePropertyChanged(nameof(ClientsList));
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

        #region Private Methods
        private void RefreshFreeRooms()
        {
            RoomsList = Model.GetRoomsList(DateFrom, DateTo);
        }

        private void RefreshPeriodPrice()
        {
            if (SelectedRoom != null)
                PeriodPrice = Model.CalculatePeriodPrice(DateFrom, DateTo, SelectedRoom.Price);
            else PeriodPrice = 0;
        }
        #endregion

        #region Commands
        //public ICommand AddClientCommand
        //{
        //    get
        //    {
        //        if (_addClientCommand == null)
        //            _addClientCommand = new RelayCommand<object>(AddClientExecute, AddClientCanExecute);
        //        return _addClientCommand;
        //    }
        //    set
        //    {
        //        _addClientCommand = value;
        //        InvokePropertyChanged(nameof(AddClientCommand));
        //    }
        //}

        //private bool AddClientCanExecute(object obj)
        //{
        //    return true;
        //}

        //private void AddClientExecute(object obj)
        //{
        //    Model.GoToAddClient();
        //}
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
