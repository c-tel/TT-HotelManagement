namespace TTHohel.Models
{
    class User
    {
        public RightsEnum Rights { get; }

        public User(RightsEnum rights)
        {
            Rights = rights;
        }
    }
}
