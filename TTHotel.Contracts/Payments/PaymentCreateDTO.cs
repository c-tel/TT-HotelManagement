using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace TTHotel.Contracts.Payments
{
    public class PaymentCreateDTO
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentTypes Type { get; set; }
        public double Amount { get; set; }
    }
}
