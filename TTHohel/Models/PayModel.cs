using System.ComponentModel;
using System.Windows;
using TTHohel.Manager;
using TTHohel.Services;
using TTHotel.Contracts.Payments;

namespace TTHohel.Models
{
    public enum PayResult
    {
        [Description("Оплату додано!")]
        Success,
        [Description("Щось пішло не так...")]
        Error,
        [Description("Забагато! :)")]
        TooMuch,
        [Description("Перевірте правильність вводу!")]
        InvalidInput
    }
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
