using TTHohel.Manager;
using TTHohel.Services;
using TTHotel.Contracts.Auth;

namespace TTHohel.Models
{
    class PersonnelModel
    {
        public void GoBack()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Settings);
        }

        public int CreateNewPersonnel(UserCreateDTO user, UserRoles role)
        {
            user.Role = role;
            var res = HotelApiClient.GetInstance().CreatePersonnel(user);

            if (res == System.Net.HttpStatusCode.NoContent)
            {
                //Storage.Instance.ChangeAllClients(clientDTO);
                return 1;
            }
            if (res == System.Net.HttpStatusCode.Conflict)
                return 2;

            return 0;
        }
    }
}
