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
                if(HotelApiClient.GetInstance().AuthorizationResponse.User.Role.ToString() == "Head")
                    NavigationManager.Instance.Navigate(ModesEnum.Main);
                else NavigationManager.Instance.Navigate(ModesEnum.AdminMain);
                return true;
            }
            return false;
        }
    }
}
