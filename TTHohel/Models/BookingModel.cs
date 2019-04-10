using System;
using TTHohel.Manager;
using TTHohel.Services;
using TTHotel.Contracts.Bookings;
using TTHotel.Contracts.Clients;

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
            var client = GetClient(clientTel);

            var data = new ClientDisplayData
            {
                Client = client,
                Mode = DisplayModes.Editing,
                CameFrom = ModesEnum.Booking
            };
            Storage.Instance.ChangeClientDisplayData(data);

            NavigationManager.Instance.Navigate(ModesEnum.Client);
        }

        public static double CalculateToPay(BookingDTO bookingDTO)
        {
            if(bookingDTO == null)
                return 0;
            var calc = bookingDTO.PricePeriod * (1 - bookingDTO.ClientDiscount / 100.0) + bookingDTO.SumFees - bookingDTO.Payed;
            return Math.Max(calc, 0);
        }

        public bool Settle(BookingDTO booking)
        {

            if (HotelApiClient.GetInstance().SetBookingStatus(booking.BookingId, BookingStates.Settled))
            {
                var _ = HotelApiClient.GetInstance().GetBookingById(booking.BookingId);
                Storage.Instance.ChangeBooking(_);

                return true;
            }
            return false;
        }

        public bool Close(BookingDTO booking)
        {
            
            if (HotelApiClient.GetInstance().SetBookingStatus(booking.BookingId, BookingStates.Non_settle))
            {
                var _ = HotelApiClient.GetInstance().GetBookingById(booking.BookingId);
                Storage.Instance.ChangeBooking(_);

                return true;
            }
            return false;
        }

        public bool Cancel(BookingDTO booking)
        {

            if (HotelApiClient.GetInstance().SetBookingStatus(booking.BookingId, BookingStates.Canceled))
            {
                var _ = HotelApiClient.GetInstance().GetBookingById(booking.BookingId);
                Storage.Instance.ChangeBooking(_);

                return true;
            }
            return false;
        }

        public ClientDTO GetClient(string clientTel)
        {
            return HotelApiClient.GetInstance().GetClient(clientTel);
        }

        public bool Edit(BookingDTO booking)
        {
            if (HotelApiClient.GetInstance().EditBooking(booking))
            {
                var _ = HotelApiClient.GetInstance().GetBookingById(booking.BookingId);
                Storage.Instance.ChangeBooking(_);

                return true;
            }
            return false;
        }
    }
}
