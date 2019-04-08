using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTHohel.Manager;

namespace TTHohel.Models
{
    class PersonnelModel
    {
        public void GoBack()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Settings);
        }
    }
}
