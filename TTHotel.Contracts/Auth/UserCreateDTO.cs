using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TTHotel.Contracts.Auth
{
    public class UserCreateDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronym { get; set; }

        public string EmplBook { get; set; }
        public string TelNumber { get; set; }
        public string Passport { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public UserRoles Role { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
    }
}
