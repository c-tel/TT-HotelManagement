using System;
using System.Collections.Generic;
using System.Text;

namespace TTHotel.Contracts.Cleanings
{
    public class CleaningStatsDTO
    {
        public string BookNum { get; set; }
        public string Surname { get; set; }
        public int CountCompleted { get; set; }
    }
}
