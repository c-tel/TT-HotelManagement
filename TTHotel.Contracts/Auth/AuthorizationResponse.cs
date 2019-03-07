using System;
using System.Collections.Generic;
using System.Text;

namespace TTHotel.Contracts.Auth
{
    public class AuthorizationResponse
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public UserRoles UserRole { get; set; }
        public string SessionKey { get; set; }
    }
}
