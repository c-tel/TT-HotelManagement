using System;

namespace TTHotel.Contracts.Payments
{
    class PaymentDTO
    {
        public PaymentTypes Type { get; set; }
        public DateTime Payment_date { get; set; }
        public double Amount { get; set; }
        public int Book_num { get; set; }
    }
}
