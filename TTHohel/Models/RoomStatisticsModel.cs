using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTHohel.Contracts.Bookings;
using TTHotel.Contracts.Rooms;

namespace TTHohel.Models
{
    public class RoomStatisticsModel
    {
        public List<RoomStatisticsDTO> GetStatistics(DateTime dateFrom, DateTime dateTo)
        {
            return new List<RoomStatisticsDTO> { new RoomStatisticsDTO {Num = 1, Type = RoomType.Apartments } };
        }
    }
}
