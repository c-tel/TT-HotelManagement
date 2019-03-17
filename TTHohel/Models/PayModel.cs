using TTHohel.Manager;

namespace TTHohel.Models
{
    class PayModel
    {
        public void GoToMain()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Main);
        }
    }
}
