using System.Collections.Generic;
using TTHohel.Contracts.Bookings;

namespace TTHotel.Contracts.Rooms
{
    public class RoomDTO
    {
        public int Num { get; set; }
        public int Floor { get; set; }
        public int Places { get; set; }
        public double Price { get; set; }
        public RoomType Type { get; set; }
        public List<string> Comforts { get; set; }
    }
}
