using System;
using System.Collections.Generic;
using System.Text;
using TTHohel.Manager;

namespace TTHohel.Models
{
    class AddBookingModel
    {
        public void GoToAddClient()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Pay);
        }
    }
}
