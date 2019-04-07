using System.ComponentModel;

namespace TTHotel.Contracts.Bookings
{
    public enum BookingStates
    {
        [Description("Заброньовано")]
        Booked,
        [Description("Поселено")]
        Settled,
        [Description("Закрито")]
        Non_settle,
        [Description("Скасовано")]
        Canceled
    }
}
