using System.ComponentModel;

namespace TTHotel.Contracts.Payments
{
    public enum PaymentTypes
    {
        [Description("Готівка")]
        Cash,
        [Description("Картка")]
        Card
    }
}
