using System;
using System.Collections.Generic;
using TTHohel.Manager;
using TTHohel.Services;
using TTHotel.Contracts.Rooms;

namespace TTHohel.Models
{
    public class RoomModel
    {

        public void GoBack()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Settings);
        }

        public int CreateNewRoom(RoomCreateDTO room, Contracts.Bookings.RoomType selectedType)
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
