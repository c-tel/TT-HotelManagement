using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace TTHotel.Contracts.Bookings
{
    public class BookingUpdateDTO
    {
        public int BookingId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string BookComment { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public BookingStates? Book_state { get; set; }
        public DateTime? StartDateReal { get; set; }
        public DateTime? EndDateReal { get; set; }
        public string Complaint { get; set; }
    }
}
