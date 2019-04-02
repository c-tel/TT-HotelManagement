using System.ComponentModel;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;
using TTHotel.Contracts.Bookings;

namespace TTHohel.ViewModels
{
    class BookingViewModel : INotifyPropertyChanged
    {
        #region Private Fields
        private BookingDTO _bookingDTO;
        private BookingModel Model { get; }

        private ICommand _backCommand;
        private ICommand _payCommand;
        #endregion

        public BookingViewModel()
        {
            Model = new BookingModel();
            Model.BookingChanged += OnBookingChanged;
        }

        public BookingDTO BookingDTO
        {
            get { return _bookingDTO; }
            set
            {
                if (_bookingDTO != value)
                {
                    _bookingDTO = value;
                    InvokePropertyChanged(nameof(BookingDTO));
                }
            }
        }

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
        }

        public ICommand PayCommand
        {
            get
            {
                if (_payCommand == null)
                    _payCommand = new RelayCommand<object>(PayExecute, PayCanExecute);
                return _payCommand;
            }
            set
            {
                _payCommand = value;
                InvokePropertyChanged(nameof(PayCommand));
            }
        }

        private bool PayCanExecute(object obj)
        {
            return true;
            //return BookingDTO.Payed != (BookingDTO.PricePeriod+BookingDTO.SumFees);
        }

        private void PayExecute(object obj)
        {
            Model.GoToPay();
        }
        #endregion

        private void OnBookingChanged(BookingDTO obj)
        {
            BookingDTO = obj;
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
