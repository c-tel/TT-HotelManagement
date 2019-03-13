using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TTHotel.Contracts.Auth
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmplBook { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public UserRoles Role { get; set; }
    }
}
