using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using TTHohel.Services;
using TTHohel.Tools;
using TTHotel.Contracts.Bookings;

namespace TTHohel.ViewModels
{
    class TodayBookingsViewModel : INotifyPropertyChanged
    {
        private TodayBookingDTO _selectedRoom;
        private List<TodayBookingDTO> _bookingsList;

        private DataGridCellInfo _cellInfo;
        private ICommand _bookInfoCommand;

        public TodayBookingsViewModel()
        {
            BookList = HotelApiClient.GetInstance().GetTodayBook();
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

        public TodayBookingDTO SelectedRoom
        {
            get { return _selectedRoom; }
            set
            {
                if (_selectedRoom != value)
                {
                    _selectedRoom = value;
                    InvokePropertyChanged(nameof(SelectedRoom));
                }
            }
        }

        public List<TodayBookingDTO> BookList
        {
            get { return _bookingsList; }
            set
            {
                _bookingsList = value;
                InvokePropertyChanged(nameof(_bookingsList));
            }
        }

        public ICommand BookInfoCommand
        {
            get
            {
                if (_bookInfoCommand == null)
                {
                    _bookInfoCommand = new RelayCommand<object>(ClientInfoExecute, ClientInfoCanExecute);
                }
                return _bookInfoCommand;
            }
            set
            {
                _bookInfoCommand = value;
                InvokePropertyChanged(nameof(BookInfoCommand));
            }
        }

        private bool ClientInfoCanExecute(object obj)
        {
            return true;
        }

        private void ClientInfoExecute(object obj)
        {
            var selectedSell = obj as DataGridCellInfo?;
            var selectedInfo = selectedSell.Value.Item as TodayBookingDTO;

            // GOTO BOOKING
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
