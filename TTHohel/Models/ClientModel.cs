using System;
using TTHohel.Manager;
using TTHohel.Services;
using TTHohel.Tools;
using TTHotel.Contracts.Clients;

namespace TTHohel.Models
{
    class ClientModel
    {
        private ModesEnum _cameFrom;

        public event Action<RightsEnum> UserChanged;
        public event Action<ClientDisplayData> ClientDisplayChanged;

        public ClientModel()
        {
            Storage.Instance.UserChanged += OnUserChanged;
            Storage.Instance.ClientDisplayChanged += OnDisplayDataChanged;
        }

        private void OnUserChanged(User user)
        {
            UserChanged?.Invoke(user.Rights);
        }

        private void OnDisplayDataChanged(ClientDisplayData data)
        {
            _cameFrom = data.CameFrom;
            ClientDisplayChanged?.Invoke(data);
        }

        public AddResult CreateNewClient(ClientDTO clientDTO)
        {
            if (!DataValidation.ValidateTelNum(clientDTO.TelNum))
                return AddResult.InvalidInput;

            if (!string.IsNullOrEmpty(clientDTO.Passport))
                if (!DataValidation.ValidatePassport(clientDTO.Passport))
                    return AddResult.InvalidInput;

            var res = HotelApiClient.GetInstance().CreateClient(clientDTO);

            if (res == System.Net.HttpStatusCode.NoContent)
            {
                Storage.Instance.ChangeAllClients(clientDTO);
                return AddResult.Success;
                ;
            }

            if (res == System.Net.HttpStatusCode.Conflict)
                return AddResult.AlreadyCreated;

            return AddResult.Error;
        }

        public void GoBack()
        {
            NavigationManager.Instance.Navigate(_cameFrom);
        }

        public AddResult SaveClient(ClientDTO client, string oldTel)
        {
            if (!DataValidation.ValidateTelNum(client.TelNum))
                return AddResult.InvalidInput;

            if (!string.IsNullOrEmpty(client.Passport))
                if (!DataValidation.ValidatePassport(client.Passport))
                    return AddResult.InvalidInput;

            var res = HotelApiClient.GetInstance().UpdateClient(client, oldTel);

            if (res == System.Net.HttpStatusCode.NoContent)
            {
                Storage.Instance.ChangeAllClients(client);
                return AddResult.Success;
            }
            if (res == System.Net.HttpStatusCode.Conflict)
                return AddResult.AlreadyCreated;

            return AddResult.Error;
        }
    }
}
