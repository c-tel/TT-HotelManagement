using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTHohel.Models
{
    public class ReportItemModel
    {
        public int RoomNum { get; set; }
        public double Amount { get; set; }
        public string PaymentType { get; set; }
        public string PaymentTime { get; set; }
    }
}
