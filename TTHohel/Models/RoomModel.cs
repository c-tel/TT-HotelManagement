using TTHohel.Manager;
using TTHohel.Services;
using TTHotel.Contracts.Rooms;

namespace TTHohel.Models
{
    public class RoomModel
    {
        private RoomDisplayData _data;

        public RoomModel()
        {
            _data = Storage.Instance.RoomData;
        }

        public void GoBack()
        {
            NavigationManager.Instance.Navigate(_data.CameFrom);
        }


        public RoomDTO GetRoom()
        {
            return _data.Room;
        }

        public DisplayModes GetMode()
        {
            return _data.Mode;
        }

        public int CreateNewRoom(RoomDTO room, Contracts.Bookings.RoomType selectedType)
        {
            room.Type = selectedType;
            var res = HotelApiClient.GetInstance().CreateRoom(room);

            if (res == System.Net.HttpStatusCode.NoContent)
            {

                Storage.Instance.ChangeBookings();
                return 1;
            }
            if (res == System.Net.HttpStatusCode.Conflict)
                return 2;

            return 0;
        }
    }
}
