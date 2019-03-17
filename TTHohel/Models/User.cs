namespace TTHohel.Models
{
    public class User
    {
        public RightsEnum Rights { get; }

        public User(RightsEnum rights)
        {
            Rights = rights;
        }
    }
}
