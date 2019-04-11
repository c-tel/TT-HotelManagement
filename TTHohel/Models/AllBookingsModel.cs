using TTHohel.Manager;

namespace TTHohel.Models
{
    class AllBookingsModel
    {
        public void GoToMain()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Main);
        }
    }
}
