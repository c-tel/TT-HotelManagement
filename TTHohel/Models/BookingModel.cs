using System;
using TTHohel.Manager;
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

        public void GoToMain()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Main);
        }

        internal void GoToPay()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Pay);
        }
    }
}
