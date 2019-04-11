using System;
using System.Collections.Generic;
using System.Text;

namespace TTHotel.Contracts.Bookings
{
    public class DebptInfo
    {
        public int BookNum { get; set; }
        public int RoomNum { get; set; }
        public string ClientTelNum { get; set; }
        public string ClientSurname { get; set; }
        public string ClientName { get; set; }
        public double Debpt { get; set; }
    }
}
