using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTHohel.Models;
using TTHotel.Contracts.Bookings;

namespace TTHohel.ViewModels
{
    class BookingViewModel : INotifyPropertyChanged
    {
        private BookingDTO _bookingDTO;
        private BookingModel Model { get; }

        public BookingViewModel()
        {
            Model = new BookingModel();
            Model.BookingChanged += OnBookingChanged;
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
