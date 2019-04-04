using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using TTHotel.Contracts.Payments;

namespace TTHotel.Contracts.Bookings
{
    public class ReportItem
    {
        public int RoomNum { get; set; }
        public double Amount { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentTypes PaymentType { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
