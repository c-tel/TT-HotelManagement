using System;
using System.Collections.Generic;
using System.Text;

namespace TTHotel.Contracts.Bookings
{
    public class BookingCreateDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BookComment { get; set; }
        public int BookedRoomNum { get; set; }
        public string ClientTel { get; set; }
    }
}
