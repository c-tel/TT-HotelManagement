using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTHotel.Contracts.Bookings;

namespace TTHohel.Models
{
    class BookingModel
    {
        public event Action<BookingDTO> BookingChanged;

        public BookingModel()
        {
            Storage.Instance.BookingChanged += OnBookingChanged;
        }
        private void OnBookingChanged(BookingDTO booking)
        {
            BookingChanged?.Invoke(booking);
        }
    }
}
