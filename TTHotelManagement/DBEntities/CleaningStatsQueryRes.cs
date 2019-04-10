using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTHotel.API.DBEntities
{
    public class CleaningStatsQueryRes
    {
        public string Book_num { get; set; }
        public string Surname { get; set; }
        public int Count_completed { get; set; }
    }
}
