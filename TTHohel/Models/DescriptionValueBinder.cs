using TTHotel.Contracts.Payments;

namespace TTHohel.Models
{
    class DescriptionValueBinder
    {
        public PaymentTypes Value { get; set; }
        public string Description { get; set; }

        public DescriptionValueBinder() { }

        public DescriptionValueBinder(PaymentTypes value, string description)
        {
            Value = value;
            Description = description;
        }
    }
}
