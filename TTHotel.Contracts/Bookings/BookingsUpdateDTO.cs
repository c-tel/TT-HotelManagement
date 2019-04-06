using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace TTHotel.Contracts.Bookings
{
    public class BookingUpdateDTO
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string BookComment { get; set; }
        public string Complaint { get; set; }
    }
}
