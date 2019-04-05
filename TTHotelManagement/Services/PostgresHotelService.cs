﻿using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TTHohel.Contracts.Bookings;
using TTHotel.API.DBEntities;
using TTHotel.API.Helpers;
using TTHotel.Contracts.Auth;
using TTHotel.Contracts.Bookings;
using TTHotel.Contracts.Clients;
using TTHotel.Contracts.Payments;
using TTHotel.Contracts.Rooms;

namespace TTHotel.API.Services
{
    public class PostgresHotelService: IHotelService
    {
        private NpgsqlConnectionStringBuilder _builder;

        #region QUERIES

        private static string PeriodDataQuery(DateTime from, DateTime to)
        {
            var fromStr = from.ToPostgresDateFormat();
            var toStr = to.ToPostgresDateFormat();
            var res =   "SELECT rooms.room_num, room_floor, start_date, end_date, book_state, book_num, " +
                        "(price_period+sum_fees-payed) AS debt " +
                        "FROM ( " +
                            "select * " +
                            "FROM bookings " +
                            $"WHERE (end_date BETWEEN {fromStr} AND {toStr} " +
                            $"OR start_date BETWEEN {fromStr} AND {toStr}) " +
                            $"AND book_state <> 'canceled' " +
                              ") AS B RIGHT OUTER JOIN rooms ON B.room_num = rooms.room_num " +
                        "ORDER BY room_num, start_date; ";
            return res;
        }

        private static string LoginQuery(string login, string pwdHash)
        {
            return      $"SELECT * FROM personnel " +
                        $"WHERE login ='{login}' AND pwd = '{pwdHash}'";
        }

        private static string BookingByIdQuery(int id)
        {
            return $"SELECT * FROM bookings " +
                   $"WHERE book_num ={id}";
        }

        private static string CreateBookingQuery(BookingCreateDTO booking, string person_book)
        {
            return "INSERT INTO bookings (start_date, end_date, book_comment, room_num, cl_tel_num, pers_book) " +
                   $"VALUES ({booking.StartDate.ToPostgresDateFormat()}, {booking.EndDate.ToPostgresDateFormat()}, " +
                   $"        {booking.BookComment}, {booking.BookedRoomNum}, {booking.ClientTel}, {person_book});";
        }

        private static string UpdateBookingQuery(BookingCreateDTO booking, string person_book)
        {
            // ATTENTION! Fails.
            return "UPDATE bookings " +
                   $"VALUES ({booking.StartDate.ToPostgresDateFormat()}, {booking.EndDate.ToPostgresDateFormat()}, " +
                   $"        {booking.BookComment}, {booking.BookedRoomNum}, {booking.ClientTel}, {person_book};)";
        }

        private static string PaymentsQuery(int bookingId)
        {
            return "SELECT * " +
                   "FROM payments " +
                   $"WHERE book_num = {bookingId};";
        }

        private static string ReportQuery(DateTime date)
        {
            return "SELECT payment_type AS paymentType, payment_date AS paymentDate, amount, room_num AS roomNum "+
                   "FROM payments INNER JOIN bookings ON payments.book_num = bookings.book_num " +
                   $"WHERE date(payments.payment_date) = {date.ToPostgresDateFormat()};";
        }

        private static string ClientsQuery()
        {
            return "SELECT * " +
                   "FROM clients";
        }

        private static string ClientQuery(string telNum)
        {
            return "SELECT * " +
                   "FROM clients " +
                   $"WHERE tel_num = '{telNum}';";
        }

        private static string CreateClientQuery(ClientDTO newcomer)
        {
            return "INSERT INTO clients " +
                   $"VALUES ('{newcomer.TelNum}', '{newcomer.Passport}', '{newcomer.Name}', " +
                           $"'{newcomer.Surname}', '{newcomer.Patronym}', {newcomer.Discount});";
        }

        private static string AvailiableRoomsQuery(DateTime availiableFrom, DateTime availiableTo, int guests)
        {
            return "SELECT * " +
                   "FROM rooms " +
                   "WHERE NOT EXISTS(SELECT * " +
                                    "FROM bookings " +
                                    "WHERE rooms.room_num = room_num " +
                                           $"AND({availiableFrom.ToPostgresDateFormat()} BETWEEN start_date AND end_date " +
                                               $"OR {availiableTo.ToPostgresDateFormat()} BETWEEN start_date AND end_date " +
                                               $"OR start_date BETWEEN {availiableFrom.ToPostgresDateFormat()} AND {availiableTo.ToPostgresDateFormat()}" +
                                               $"OR end_date BETWEEN {availiableFrom.ToPostgresDateFormat()} AND {availiableTo.ToPostgresDateFormat()})" +
                                            "AND book_state <> 'canceled') " +
                        $"AND room_places >= {guests} " +
                    $"ORDER BY room_num;";
        }

        private static string AllRoomsQuery(int guests)
        {
            return   "SELECT * " +
                     "FROM rooms " +
                    $"WHERE room_places >= {guests} " +
                    $"ORDER BY room_num;";
        }
        private static string RoomQuery(int roomNum)
        {
            return   "SELECT * " +
                     "FROM rooms " +
                    $"WHERE room_num = {roomNum};";
        }

        private static string ComfortsQuery(int room)
        {
            return "SELECT special_comfort " +
                   "FROM comforts_room " +
                   $"WHERE room_num = {room}";
        }

        #endregion


        public PostgresHotelService()
        {
            // TODO move to Configuration

            _builder = new NpgsqlConnectionStringBuilder
            {
                Host = "ec2-54-243-128-95.compute-1.amazonaws.com",
                Port = 5432,
                Username = "tyoigfycditakg",
                Password = "1bd83bca8ab334c28d2b311b0a73543c66a3288d3327aad1f910dea63fd372cd",
                Database = "dcvtt5pjns4r4v",
                SslMode = SslMode.Require,
                UseSslStream = true,
            };
        }

        public UserDTO GetUser(string login, string pwd)
        {
            var sql = LoginQuery(login, Hash(pwd));
            var qRes = QuerySingleOrDefaultInternal<Personnel>(sql);
            if (qRes == null)
                return null;
            return new UserDTO
            {
                EmplBook = qRes.Book_num,
                Name = qRes.Pers_name,
                Role = qRes.Pers_role,
                Surname = qRes.Surname
            };
        }

        #region BOOKINGS

        public List<RoomInfo> GetPeriodInfo(DateTime from, DateTime to)
        {
            var bookingsData = QueryInternal<MainPivotData>(PeriodDataQuery(from, to));
            var infos = bookingsData.Select(b => new RoomInfo
            {
                Floor = b.Room_floor,
                RoomNumber = b.Room_num,
                DailyInfo = new List<RoomDailyInfo>()
            }).Distinct(new RoomInfoComparer()).ToList();
            foreach(var info in infos)
            {
                var currBokings = bookingsData.Where(b => b.Room_num == info.RoomNumber && b.Book_num != null).ToList();
                var currDate = from;
                while(currDate <= to)
                {
                    var currBoking = currBokings.FirstOrDefault(b => b.Start_date.Value.Date <= currDate.Date &&
                                                                                b.End_date.Value.Date >= currDate.Date);
                    var currDailyInfo = currBoking == null ? new RoomDailyInfo
                    {
                        BookDate = currDate,
                        Status = RoomDailyStatus.Free,
                        BookID = null,
                        Debt = null
                    } : new RoomDailyInfo
                    {
                        BookDate = currDate,
                        Status = MapToStatus(currBoking.Book_state),
                        BookID = currBoking.Book_num,
                        Debt = (currBoking.Book_state == BookStates.Settled && currBoking.Debt > 0) ? currBoking.Debt : null
                    };
                    info.DailyInfo.Add(currDailyInfo);
                    currDate = currDate.AddDays(1);
                }
            }

            return infos;
        }

        public BookingDTO GetBooking(int id)
        {
            var sql = BookingByIdQuery(id);
            var qRes = QuerySingleOrDefaultInternal<Booking>(sql);
            if (qRes == null)
                return null;
            return new BookingDTO
            {
                BookComment = qRes.Book_comment,
                BookedPrice = qRes.Booked_price,
                BookedRoomNum = qRes.Book_num,
                BookingId = qRes.Book_num,
                Book_state = (BookingStates)qRes.Book_state,
                ClientTel = qRes.Cl_tel_num,
                Complaint = qRes.Complaint,
                EndDate = qRes.End_date,
                EndDateReal = qRes.End_date_real,
                Payed = qRes.Payed,
                PersBook = qRes.Pers_book,
                PersSettledBook = qRes.Pers_settled,
                PricePeriod = qRes.Price_period,
                StartDate = qRes.Start_date,
                StartDateReal = qRes.Start_date_real,
                SumFees = qRes.Sum_fees
            };
        }

        public void CreateBooking(BookingCreateDTO booking, string persBook)
        {
            ExecuteInternal(CreateBookingQuery(booking, persBook));
        }

        public void UpdateBooking(BookingUpdateDTO booking, string persBook, int bookId)
        {
            // TODO
        }

        public IEnumerable<PaymentDTO> GetPayments(int bookingId)
        {
            var payments = QueryInternal<Payment>(PaymentsQuery(bookingId));
            return payments.Select(MapToPayment);
        }

        public IEnumerable<ReportItem> GetReport(DateTime date)
        {
            var reportItems = QueryInternal<ReportItem>(ReportQuery(date));
            return reportItems;
        }

        #endregion

        #region CLIENTS

        public IEnumerable<ClientDTO> GetClients()
        {
            return QueryInternal<Client>(ClientsQuery()).Select(MapToClient);
        }

        

        public ClientDTO GetClient(string telnum)
        {
            var dbres = QuerySingleOrDefaultInternal<Client>(ClientQuery(telnum));
            return MapToClient(dbres);
        }

        public void CreateClient(ClientDTO toCreate)
        {
            ExecuteInternal(CreateClientQuery(toCreate));
        }

        #endregion

        public IEnumerable<RoomDTO> GetRooms(DateTime? availiableFrom, DateTime? availiableTo, int guests)
        {
            IEnumerable<Room> rooms;
            if (availiableFrom == null && availiableTo == null)
            {
                rooms = QueryInternal<Room>(AllRoomsQuery(guests));
            }
            else
            {
                rooms = QueryInternal<Room>(AvailiableRoomsQuery(availiableFrom.Value, availiableTo.Value, guests));
            }
            var mapTasks = rooms.Select(MapToRoom);
            var all = Task.WhenAll(mapTasks);
            return all.GetAwaiter().GetResult();
        }

        public RoomDTO GetRoom(int roomNum)
        {
            string sql = RoomQuery(roomNum);
            var room = QuerySingleOrDefaultInternal<Room>(sql);
            return MapToRoom(room).GetAwaiter().GetResult();
        }

        #region WRAPPERS
        private IEnumerable<T> QueryInternal<T>(string sql)
        {
            IEnumerable<T> result;
            var conn = new NpgsqlConnection(_builder.ToString())
            {
                // dirty hack. must be removed later
                UserCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            using (conn)
            {
                result = conn.Query<T>(sql);
            }
            return result;
        }

        private async Task<IEnumerable<T>> QueryAsyncInternal<T>(string sql)
        {
            IEnumerable<T> result;
            var conn = new NpgsqlConnection(_builder.ToString())
            {
                // dirty hack. must be removed later
                UserCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            using (conn)
            {
                result = await conn.QueryAsync<T>(sql);
            }
            return result;
        }

        private T QuerySingleOrDefaultInternal<T>(string sql)
        {
            T result;
            var conn = new NpgsqlConnection(_builder.ToString())
            {
                // dirty hack. must be removed later
                UserCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            using (conn)
            {
                result = conn.QuerySingleOrDefault<T>(sql);
            }
            return result;
        }

        private void ExecuteInternal(string sql)
        {
            var conn = new NpgsqlConnection(_builder.ToString())
            {
                // dirty hack. must be removed later
                UserCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            using (conn)
            {
                conn.Execute(sql);
            }
        }

        #endregion

        #region MAPPERS

        private static RoomDailyStatus MapToStatus(BookStates? state)
        {
            switch(state)
            {
                case BookStates.Booked:
                    return RoomDailyStatus.Booked;
                case BookStates.Settled:
                    return RoomDailyStatus.Settled;
                default:
                    return RoomDailyStatus.Free;
            }
        }

        private static ClientDTO MapToClient(Client cl)
        {
            return new ClientDTO
            {
                Name = cl.Cl_name,
                Discount = cl.Discount,
                Passport = cl.Passport,
                Patronym = cl.Patronym,
                Surname = cl.Surname,
                TelNum = cl.Tel_num
            };
        }

        private async Task<RoomDTO> MapToRoom(Room room)
        {
            return new RoomDTO
            {
                Num = room.Room_num,
                Price = room.Price,
                Floor = room.Room_floor,
                Places = room.Room_places,
                Comforts = (await QueryAsyncInternal<string>(ComfortsQuery(room.Room_num))).ToList()
            };
        }

        private static PaymentDTO MapToPayment(Payment p)
        {
            return new PaymentDTO
            {
                Amount = p.Amount,
                Book_num = p.Book_num,
                Payment_date = p.Payment_date,
                Type = p.Type
            };
        }

        #endregion


        private static string Hash(string value)
        {
            var sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                var enc = Encoding.UTF8;
                var result = hash.ComputeHash(enc.GetBytes(value));

                foreach (var b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }

    internal class RoomInfoComparer : EqualityComparer<RoomInfo>
    {
        public override bool Equals(RoomInfo x, RoomInfo y)
        {
            return x.RoomNumber == y.RoomNumber;
        }

        public override int GetHashCode(RoomInfo obj)
        {
            return obj.RoomNumber;
        }
    }
}
