namespace TTHotel.Contracts.Bookings
{
    public class TodayBookingDTO
    {
        public int BookNum { get; set; }
        public int RoomNum { get; set; }
        public string ClientTelNum { get; set; }
        public string ClientSurname { get; set; }
        public string ClientName { get; set; }
        public string BookComment { get; set; }
    }
}
