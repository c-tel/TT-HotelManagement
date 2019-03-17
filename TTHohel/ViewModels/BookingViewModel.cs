using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TTHohel.Models;
using TTHohel.Tools;
using TTHotel.Contracts.Bookings;

namespace TTHohel.ViewModels
{
    class BookingViewModel : INotifyPropertyChanged
    {
        private BookingDTO _bookingDTO;
        private BookingModel Model { get; }

        private ICommand _backCommand;

        public BookingViewModel()
        {
            Model = new BookingModel();
            Model.BookingChanged += OnBookingChanged;
        }

        public ICommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new RelayCommand<object>(BackExecute, BackCanExecute);
                }
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

        private void OnBookingChanged(BookingDTO obj)
        {
            BookingDTO = obj;
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void InvokePropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }
    }
}
