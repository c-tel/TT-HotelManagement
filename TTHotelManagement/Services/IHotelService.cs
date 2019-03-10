using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTHohel.Contracts.Bookings;

namespace TTHotel.API.Services
{
    public interface IHotelService
    {
        List<RoomInfo> GetPeriodInfo(DateTime from, DateTime to);
    }
}
