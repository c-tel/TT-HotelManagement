using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTHotel.Contracts.Payments;

namespace TTHotel.API.DBEntities
{
    public class Payment
    {
        public int Payment_num { get; set; }
        public PaymentTypes Type { get; set; }
        public DateTime Payment_date { get; set; }
        public double Amount { get; set; }
        public int Book_num { get; set; }
    }
}
