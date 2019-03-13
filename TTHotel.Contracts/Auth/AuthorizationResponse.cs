namespace TTHotel.Contracts.Auth
{
    public class AuthorizationResponse
    {
        public UserDTO User { get; set; }
        public string SessionKey { get; set; }
    }
}
