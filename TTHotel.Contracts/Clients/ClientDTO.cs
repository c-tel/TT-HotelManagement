using System;
using System.Collections.Generic;
using System.Text;

namespace TTHotel.Contracts.Clients
{
    public class ClientDTO
    {
        public string TelNum { get; set; }
        public string Passport { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronym { get; set; }
        public int Discount { get; set; }
    }
}
