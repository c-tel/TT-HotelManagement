using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TTHotel.Contracts.Auth;
using TTHotel.Contracts.Payments;

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
        public UserDTO PersBooked { get; set; }
        public UserDTO PersSettled { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public BookingStates Book_state { get; set; }
        public List<PaymentDTO> Payments { get; set; }
    }
}
