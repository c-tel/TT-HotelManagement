using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTHotel.API.DBEntities
{
    public class Room
    {
        public int Room_num { get; set; }
        public int Room_floor { get; set; }
        public int Room_places { get; set; }
        public double Price { get; set; }
        public string Type_name { get; set; }
    }
}
