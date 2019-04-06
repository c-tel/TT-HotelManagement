using TTHohel.Manager;

namespace TTHohel.Models
{
    class PayModel
    {
        public bool AddPayment()
        {
            return true;
        }

        public void GoToBooking()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Booking);
        }
    }
}
