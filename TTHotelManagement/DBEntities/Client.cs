using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTHotel.API.DBEntities
{
    public class Client
    {
        public string Tel_num { get; set; }
        public string Passport { get; set; }
        public string Cl_name { get; set; }
        public string Surname { get; set; }
        public string Patronym { get; set; }
        public int Discount { get; set; }
    }
}
