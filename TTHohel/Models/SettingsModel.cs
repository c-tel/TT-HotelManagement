using System;
using TTHohel.Manager;

namespace TTHohel.Models
{
    class SettingsModel
    {
        public void GoToMain()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Main);
        }

        public void AddPersonnel()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Personnel);
        }

        public void AddRoom()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Room);
        }
    }
}
