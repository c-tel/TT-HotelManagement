using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTHotel.API.DBEntities
{
    public class Booking
    {
        public int Book_num { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public DateTime Start_date_real { get; set; }
        public DateTime End_date_real { get; set; }
        public double Booked_price { get; set; }
        public double Price_period { get; set; }
        public double Sum_fees { get; set; }
        public double Payed { get; set; }
        public string Book_comment { get; set; }
        public string Complaint { get; set; }
        public int Room_num { get; set; }
        public string Cl_tel_num { get; set; }
        public string Pers_book { get; set; }
        public string Pers_settled { get; set; }
        public BookStates Book_state { get; set; }
    }
}
