using TTHohel.Manager;

namespace TTHohel.Models
{
    class StatisticModel
    {
        public void GoToMain()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Main);
        }
    }
}
