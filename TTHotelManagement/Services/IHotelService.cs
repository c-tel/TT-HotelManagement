using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTHohel.Contracts.Bookings;
using TTHotel.Contracts.Auth;
using TTHotel.Contracts.Bookings;
using TTHotel.Contracts.Clients;
using TTHotel.Contracts.Payments;
using TTHotel.Contracts.Rooms;

namespace TTHotel.API.Services
{
    public interface IHotelService
    {
        // users
        UserDTO GetUser(string login, string pwd);

        // bookings
        List<RoomInfo> GetPeriodInfo(DateTime from, DateTime to);
        BookingDTO GetBooking(int id);
        
        void CreateBooking(BookingCreateDTO booking, string persBook);
        void UpdateBooking(BookingUpdateDTO booking, int bookId);
        void Close(int bookId);
        void Cancel(int bookId);
        void Settle(int bookId);

        // payments
        IEnumerable<PaymentDTO> GetPayments(int bookingId);
        IEnumerable<ReportItem> GetReport(DateTime date);
        void CreatePayment(int bookingId, PaymentCreateDTO payment);

        // rooms
        IEnumerable<RoomDTO> GetRooms(DateTime? availiableFrom, DateTime? availiableTo, int guests);
        RoomDTO GetRoom(int roomNum);

        //clients
        IEnumerable<ClientDTO> GetClients();
        ClientDTO GetClient(string telnum);
        void CreateClient(ClientDTO toCreate);
        void UpdateClient(ClientDTO client, string telnum);
    }
}
