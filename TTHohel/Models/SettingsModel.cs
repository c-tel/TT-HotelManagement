using TTHohel.Manager;

namespace TTHohel.Models
{
    class SettingsModel
    {
        public void GoToMain()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Main);
        }
    }
}
