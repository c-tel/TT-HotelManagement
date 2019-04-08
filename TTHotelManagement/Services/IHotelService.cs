using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTHohel.Contracts.Bookings;
using TTHotel.Contracts;
using TTHotel.Contracts.Auth;
using TTHotel.Contracts.Bookings;
using TTHotel.Contracts.Cleanings;
using TTHotel.Contracts.Clients;
using TTHotel.Contracts.Payments;
using TTHotel.Contracts.Rooms;

namespace TTHotel.API.Services
{
    public interface IHotelService
    {
        // users
        UserDTO GetUser(string login, string pwd);
        UserDTO GetUser(string emplBook);
        void Register(UserCreateDTO user);

        // bookings
        List<RoomInfo> GetPeriodInfo(DateTime from, DateTime to);
        BookingDTO GetBooking(int id);
        
        void CreateBooking(BookingCreateDTO booking, string persBook);
        void UpdateBooking(BookingUpdateDTO booking, int bookId);
        void Close(int bookId);
        void Cancel(int bookId);
        void Settle(int bookId, string pers_book);

        // payments
        IEnumerable<PaymentDTO> GetPayments(int bookingId);
        IEnumerable<ReportItem> GetReport(DateTime date);
        void CreatePayment(int bookingId, PaymentCreateDTO payment);

        // rooms
        IEnumerable<RoomDTO> GetRooms(DateTime? availiableFrom, DateTime? availiableTo, int guests);
        IEnumerable<RoomStatisticsDTO> GetRoomStats(DateTime from, DateTime to);
        RoomDTO GetRoom(int roomNum);
        void CreateRoom(RoomCreateDTO room);
        IEnumerable<CleaningDTO> GetCleanings();
        //clients
        IEnumerable<ClientDTO> GetClients();
        IEnumerable<ClientAnalisedDTO> GetClientAnalytics();
        ClientDTO GetClient(string telnum);
        void CreateClient(ClientDTO toCreate);
        void UpdateClient(ClientDTO client, string telnum);
    }
}
