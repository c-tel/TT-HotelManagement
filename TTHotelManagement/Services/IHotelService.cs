using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTHohel.Contracts.Bookings;
using TTHotel.Contracts.Auth;
using TTHotel.Contracts.Bookings;
using TTHotel.Contracts.Payments;

namespace TTHotel.API.Services
{
    public interface IHotelService
    {
        // users
        UserDTO GetUser(string login, string pwd);

        // bookings
        List<RoomInfo> GetPeriodInfo(DateTime from, DateTime to);
        BookingDTO GetBooking(int id);
        // TODO process conflicts
        void CreateBooking(BookingCreateDTO booking, string persBook);
        void UpdateBooking(BookingUpdateDTO booking, string persBook, int bookId);

        // payments
        IEnumerable<PaymentDTO> GetPayments(int bookingId);
    }
}
