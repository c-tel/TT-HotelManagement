using System.Collections.Generic;
using TTHohel.Manager;
using TTHohel.Services;
using TTHotel.Contracts.Auth;

namespace TTHohel.Models
{
    class LoginModel
    {
        public static Dictionary<UserRoles, RightsEnum> RightsDictionary = new Dictionary<UserRoles, RightsEnum>
        {
            {UserRoles.Head,  RightsEnum.All},
            {UserRoles.Administrator,  RightsEnum.None}
        };

        public bool Login(string login, string pwd)
        {
            var authResp = HotelApiClient.GetInstance().Login(login, pwd);
            if (authResp == null)
                return false;

            var userRole = authResp.User.Role;
            Storage.Instance.ChangeUser(new User(RightsDictionary[userRole]));

            NavigationManager.Instance.Navigate(ModesEnum.Main);

            return true;

        }
    }
}
