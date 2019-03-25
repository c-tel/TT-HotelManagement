using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace TTHotel.Contracts.Payments
{
    public class PaymentDTO
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentTypes Type { get; set; }
        public DateTime Payment_date { get; set; }
        public double Amount { get; set; }
        public int Book_num { get; set; }
    }
}
