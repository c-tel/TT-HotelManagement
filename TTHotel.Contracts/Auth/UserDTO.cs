using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace TTHotel.Contracts.Auth
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronym { get; set; }

        public string EmplBook { get; set; }
        public string TelNumber { get; set; }
        public string Passport { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public UserRoles Role { get; set; }

        public DateTime WorksFrom { get; set; }
        public DateTime? WorksTo { get; set; }

    }
}
