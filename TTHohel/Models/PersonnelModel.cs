using TTHohel.Manager;
using TTHohel.Services;
using TTHohel.Tools;
using TTHotel.Contracts.Auth;

namespace TTHohel.Models
{
    
    class PersonnelModel
    {
        public void GoBack()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Settings);
        }

        public AddResult CreateNewPersonnel(UserCreateDTO user, UserRoles role)
        {
            if (!DataValidation.ValidateTelNum(user.TelNumber))
                return AddResult.InvalidInput;
            if (!DataValidation.ValidatePassport(user.Passport))
                return AddResult.InvalidInput;
            if (!DataValidation.ValidatePassport(user.EmplBook))
                return AddResult.InvalidInput;

            user.Role = role;
            var res = HotelApiClient.GetInstance().CreatePersonnel(user);

            if (res == System.Net.HttpStatusCode.NoContent)
            {
                //Storage.Instance.ChangeAllClients(clientDTO);
                return AddResult.Success;
            }
            if (res == System.Net.HttpStatusCode.Conflict)
                return AddResult.AlreadyCreated;

            return AddResult.Error;
        }
    }
}
