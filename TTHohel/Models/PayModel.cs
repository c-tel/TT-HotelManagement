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
            return HotelApiClient.GetInstance().AddPayment(id, paymentType, amount);
        }

        public void GoToBooking()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Booking);
        }
    }
}
