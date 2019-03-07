using System;
using TTHohel.Manger;

namespace TTHohel.Models
{
    class MainModel
    {
        public void Exit()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Login);
        }

        public void GoToSett()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Settings);
        }
    }
}
