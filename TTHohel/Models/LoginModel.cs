using System.Collections.Generic;
using TTHohel.Manager;
using TTHohel.Services;
using TTHotel.Contracts.Auth;

namespace TTHohel.Models
{
    class LoginModel
    {
        private static Dictionary<UserRoles, RightsEnum> rightsDictionary = new Dictionary<UserRoles, RightsEnum>
        {
            {UserRoles.Head,  RightsEnum.All},
            {UserRoles.Administrator,  RightsEnum.None}
        };

        public bool Login(string login, string pwd)
        {
            
            if (!HotelApiClient.GetInstance().Login(login, pwd))
                return false;

            var userRole = HotelApiClient.GetInstance().AuthorizationResponse.User.Role;
            HotelApiClient.GetInstance().ChangeUser(new User(rightsDictionary[userRole]));

            NavigationManager.Instance.Navigate(ModesEnum.Main);

            return true;

        }
    }
}
