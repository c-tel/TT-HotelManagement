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

        public void GoToPay()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Pay);
        }

        public void GoToClient()
        {
            // TODO
            NavigationManager.Instance.Navigate(ModesEnum.AddClient);
        }

        public double CalculateToPay(BookingDTO bookingDTO)
        {
            if(bookingDTO != null)
                return bookingDTO.PricePeriod + bookingDTO.SumFees - bookingDTO.Payed;
            return 0;
        }

    }
}
