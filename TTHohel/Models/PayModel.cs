using TTHohel.Manager;
using TTHohel.Services;
using TTHotel.Contracts.Payments;

namespace TTHohel.Models
{
    class PayModel
    {
        public bool AddPayment(PaymentTypes paymentType, double amount)
        {
            var id = Storage.Instance.SelectedBooking.BookingId;

            if(HotelApiClient.GetInstance().AddPayment(id, paymentType, amount))
            {
                var booking = HotelApiClient.GetInstance().GetBookingById(id);
                Storage.Instance.ChangeBooking(booking);
                return true;
            }

            return false;
        }

        public void GoToBooking()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Booking);
        }
    }
}
