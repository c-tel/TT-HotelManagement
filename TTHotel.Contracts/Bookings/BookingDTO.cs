using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace TTHotel.Contracts.Bookings
{
    public class BookingDTO
    {
        public int BookingId{ get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? StartDateReal { get; set; }
        public DateTime? EndDateReal { get; set; }
        public double BookedPrice { get; set; }
        public double PricePeriod { get; set; }
        public double SumFees { get; set; }
        public double Payed { get; set; }
        public string BookComment { get; set; }
        public string Complaint { get; set; }
        public int BookedRoomNum { get; set; }
        public string ClientTel { get; set; }
        public string PersBook { get; set; }
        public string PersSettledBook { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public BookingStates Book_state { get; set; }
    }
}
