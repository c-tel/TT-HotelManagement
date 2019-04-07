using System;
using TTHohel.Manager;
using TTHohel.Services;
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

        public void GoToClient(string clientTel)
        {
            var client = HotelApiClient.GetInstance().GetClient(clientTel);

            var data = new ClientDisplayData
            {
                Client = client,
                Mode = ClientViewModes.Editing,
                CameFrom = ModesEnum.Booking
            };
            Storage.Instance.ChangeClientDisplayData(data);

            NavigationManager.Instance.Navigate(ModesEnum.Client);
        }

        public double CalculateToPay(BookingDTO bookingDTO)
        {
            if(bookingDTO == null)
                return 0;
            var calc = bookingDTO.PricePeriod * (1 - bookingDTO.ClientDiscount / 100.0) + bookingDTO.SumFees - bookingDTO.Payed;
            return Math.Max(calc, 0);
        }

        public bool Settle(BookingDTO booking)
        {
            if (HotelApiClient.GetInstance().SetBookingToSettled(booking.BookingId))
            {
                var _ = HotelApiClient.GetInstance().GetBookingById(booking.BookingId);
                Storage.Instance.ChangeBooking(_);

                return true;
            }
            return false;
        }
    }
}
