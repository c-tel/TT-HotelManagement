using System;
using System.Collections.Generic;
using TTHohel.Manager;
using TTHohel.Services;
using TTHotel.Contracts.Clients;
using TTHotel.Contracts.Rooms;

namespace TTHohel.Models
{
    class AddBookingModel
    {

        public List<ClientDTO> GetClientsList()
        {
            return HotelApiClient.GetInstance().GetAllClients();
        }

        public List<RoomDTO> GetRoomsList(DateTime dateFrom, DateTime dateTo)
        {
            return HotelApiClient.GetInstance().GetFreeRooms(dateFrom, dateTo);
        }

        public double CalculatePeriodPrice(DateTime dateFrom, DateTime dateTo, double price)
        {
            return ((dateTo - dateFrom).TotalDays+1) * price;
        }
    }
}
