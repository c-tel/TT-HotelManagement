using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTHotel.API.DBEntities
{
    public class MainPivotData
    {
        public int Room_num { get; set; }
        public int Room_floor { get; set; }
        public DateTime? Start_date { get; set; }
        public DateTime? End_date { get; set; }
        public BookStates? Book_state { get; set; }
        public double? Debt { get; set; }
    }
}
