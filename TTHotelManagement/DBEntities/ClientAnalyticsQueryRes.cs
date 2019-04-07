using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTHotel.API.DBEntities
{
    public class ClientAnalyticsQueryRes
    {
        public string Tel_num { get; set; }
        public string Cl_name { get; set; }
        public string Surname { get; set; }
        public int Discount { get; set; }
        public int Count_booked { get; set; }
        public double Sum_payed { get; set; }
    }
}
