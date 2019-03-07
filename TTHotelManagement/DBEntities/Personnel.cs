using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTHotel.Contracts.Auth;

namespace TTHotel.API.DBEntities
{
    public class Personnel
    {
        public string Book_num { get; set; }
        public string Tel_num { get; set; }
        public string Passport { get; set; }
        public string Pers_name { get; set; }
        public string Surname { get; set; }
        public string Patronym { get; set; }
        public UserRoles pers_role { get; set; }
        public DateTime Works_from { get; set; }
        public DateTime? Works_to { get; set; }
        public string Login { get; set; }
        public string Pwd { get; set; }
    }
}
