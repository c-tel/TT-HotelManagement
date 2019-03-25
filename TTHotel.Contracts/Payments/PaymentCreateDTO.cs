using System;
using System.Collections.Generic;
using System.Text;

namespace TTHotel.Contracts.Payments
{
    public class PaymentCreateDTO
    {
        public PaymentTypes Type { get; set; }
        public double Amount { get; set; }
        public int BookingId { get; set; }
    }
}
