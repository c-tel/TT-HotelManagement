using TTHohel.Manager;

namespace TTHohel.Models
{
    class PayModel
    {
        public void GoToBooking()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Booking);
        }
    }
}
