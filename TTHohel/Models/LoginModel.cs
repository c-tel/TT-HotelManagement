using TTHohel.Manager;
using TTHohel.Services;

namespace TTHohel.Models
{
    class LoginModel
    {

        public bool Login(string login, string pwd)
        {
            if(HotelApiClient.GetInstance().Login(login, pwd))
            {
                NavigationManager.Instance.Navigate(ModesEnum.Main);
                return true;
            }
            return false;
        }
    }
}
