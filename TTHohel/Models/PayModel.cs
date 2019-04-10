using TTHohel.Manager;
using TTHohel.Services;
using TTHohel.Tools;
using TTHotel.Contracts.Payments;

namespace TTHohel.Models
{
    
    class PayModel
    {
        public PayResult AddPayment(PaymentTypes paymentType, string amount)
        {
            var valid = double.TryParse(amount, out var amountParsed);

            if (!valid)
            {
                return PayResult.InvalidInput;
            }

            if (amountParsed > BookingModel.CalculateToPay(Storage.Instance.SelectedBooking))
            {
                return PayResult.TooMuch;
            }

            var id = Storage.Instance.SelectedBooking.BookingId;

            if(HotelApiClient.GetInstance().AddPayment(id, paymentType, amountParsed))
            {
                var booking = HotelApiClient.GetInstance().GetBookingById(id);
                Storage.Instance.ChangeBooking(booking);

                return PayResult.Success;
            }

            return PayResult.Error;
        }

        public void GoToBooking()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Booking);
        }
    }
}
