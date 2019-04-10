using TTHohel.Manager;
using TTHotel.Contracts.Rooms;

namespace TTHohel.Models
{
    class SettingsModel
    {
        public void GoToMain()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Main);
        }

        public void AddPersonnel()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Personnel);
        }

        public void AddRoom()
        {
            var data = new RoomDisplayData
            {
                CameFrom = ModesEnum.Settings,
                Room = new RoomDTO(),
                Mode = DisplayModes.Creation
            };

            Storage.Instance.ChangeRoomDisplayData(data);

            NavigationManager.Instance.Navigate(ModesEnum.Room);
        }
    }
}
