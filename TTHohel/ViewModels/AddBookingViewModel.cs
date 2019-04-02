using System;
using System.ComponentModel;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;

namespace TTHohel.ViewModels
{
    class AddBookingViewModel : INotifyPropertyChanged
    {
        #region Private Fields
        private DateTime _dateFrom;
        private DateTime _dateTo;

        //private BookingDTO _bookingDTO;
        private AddBookingModel Model { get; }

        private ICommand _addClientCommand;
        #endregion

        public AddBookingViewModel()
        {
            Model = new AddBookingModel();
        }

        #region Commands
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
