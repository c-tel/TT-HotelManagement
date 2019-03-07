using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TTHotel.Contracts.Auth
{
    public class AuthorizationResponse
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public UserRoles UserRole { get; set; }
        public string SessionKey { get; set; }
    }
}
