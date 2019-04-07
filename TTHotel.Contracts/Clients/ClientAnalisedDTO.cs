using System;
using System.Collections.Generic;
using System.Text;

namespace TTHotel.Contracts.Clients
{
    public class ClientAnalisedDTO
    {
        public string TelNum { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int CountBooked { get; set; }
        public int Discount { get; set; }
        public double SumPayed { get; set; }
    }
}
