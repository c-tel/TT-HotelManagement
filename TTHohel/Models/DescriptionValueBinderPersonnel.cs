using TTHotel.Contracts.Auth;

namespace TTHohel.Models
{
    class DescriptionValueBinderPersonnel
    {
        public UserRoles Value { get; set; }
        public string Description { get; set; }

        public DescriptionValueBinderPersonnel() { }

        public DescriptionValueBinderPersonnel(UserRoles value, string description)
        {
            Value = value;
            Description = description;
        }
    }
}
