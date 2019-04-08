using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TTHohel.Contracts.Bookings;
using TTHohel.Services;
using TTHotel.Contracts.Rooms;

namespace TTHohel.Models
{
    public class RoomStatisticsModel
    {
        public List<RoomStatisticsDTO> GetStatistics(DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom > dateTo)
                MessageBox.Show("Початкова дата пізніше кінцевої!");
            return HotelApiClient.GetInstance().GetRoomsStats(dateFrom, dateTo);
        }
    }
}
