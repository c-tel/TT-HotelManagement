using Dapper;
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
                        "(price_period * (1 - (" +
                            "SELECT discount " +
                            "FROM clients " +
                            "WHERE tel_num = B.cl_tel_num)/100.0) + sum_fees-payed) AS debt " +
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

        private static string RegisterQuery(UserCreateDTO user)
        {
            return $"INSERT INTO personnel " +
                   $"VALUES ('{user.EmplBook}', '{user.TelNumber}', '{user.Passport}', '{user.Name}', '{user.Surname}', " +
                   $"        '{user.Patronym}', '{user.Role.ToString().ToLower()}', " +
                   $"         {DateTime.Now.AddHours(3).ToPostgresDateFormat()}, null, '{user.Login}', '{Hash(user.Password)}');";
        }

        private static string IdentifyQuery(string personBookNum)
        {
            return $"SELECT * FROM personnel " +
                   $"WHERE book_num ='{personBookNum}';";
        }


        private static string BookingByIdQuery(int id)
        {
            return $"SELECT * " +
                   $"FROM (bookings INNER JOIN (SELECT tel_num, discount " +
                                              "FROM clients) AS cls ON bookings.cl_tel_num = cls.tel_num)" +
                   $"WHERE book_num = {id}";
        }

        private static string CreateBookingQuery(BookingCreateDTO booking, string person_book)
        {
            return "INSERT INTO bookings (start_date, end_date, book_comment, room_num, cl_tel_num, pers_book) " +
                   $"VALUES ({booking.StartDate.ToPostgresDateFormat()}, {booking.EndDate.ToPostgresDateFormat()}, " +
                   $"        {(booking.BookComment != null ? $"'{booking.BookComment}'" : "null")}, {booking.BookedRoomNum}, '{booking.ClientTel}', '{person_book}');";
        }

        private static string UpdateBookingQuery(BookingUpdateDTO booking, int bookId)
        {
            return "UPDATE bookings " +
                   $"SET book_comment = {(booking.BookComment != null ? $"'{booking.BookComment}'" : "book_comment")}, " +
                       $"complaint    = {(booking.Complaint != null ? $"'{booking.Complaint}'" : "complaint")}, " +
                       $"start_date   = {(booking.StartDate != null ? booking.StartDate.Value.ToPostgresDateFormat() : "start_date")}, " +
                       $"end_date     = {(booking.EndDate != null ? booking.EndDate.Value.ToPostgresDateFormat() : "end_date")} " +
                   $"WHERE book_num = {bookId} ";
        }

        private static string SettleQuery(int bookId, string persBook)
        {
            return "UPDATE bookings " +
                   $"SET start_date_real = { DateTime.Now.AddHours(3).ToPostgresTimestampFormat() }, " +
                       $"book_state = 'settled', " +
                       $"pers_settled = '{persBook}' " +
                   $"WHERE book_num = {bookId};";
        }

        private static string CloseQuery(int bookId)
        {
            return "UPDATE bookings " +
                   $"SET end_date_real = { DateTime.Now.AddHours(3).ToPostgresTimestampFormat() } " +
                   $"WHERE book_num = {bookId};";
        }

        private static string CancelQuery(int bookId)
        {
            return "UPDATE bookings " +
                   $"SET book_state = 'canceled' " +
                   $"WHERE book_num = {bookId}; ";
        }

        private static string PaymentsQuery(int bookingId)
        {
            return "SELECT * " +
                   "FROM payments " +
                   $"WHERE book_num = {bookingId};";
        }

        private static string AddPaymentQuery(int bookingId, PaymentTypes paymentType, double amount)
        {
            return "INSERT INTO payments (payment_type, payment_date, amount, book_num) " +
                   $"VALUES ('{paymentType.ToString().ToLower()}', {DateTime.Now.AddHours(3).ToPostgresTimestampFormat()}, {amount}, {bookingId})";
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
        private static string ClientAnalyticsQuery()
        {
            return "SELECT clients.tel_num, clients.cl_name, clients.surname, clients.discount, " +
                          "COUNT(bookings.book_num) as count_booked, " +
				          "coalesce(SUM(bookings.payed), 0) as sum_payed " +
                   "FROM clients LEFT OUTER JOIN bookings ON bookings.cl_tel_num = clients.tel_num " +
                   "GROUP BY clients.tel_num, clients.cl_name, clients.surname, clients.discount " +
                   "ORDER BY sum_payed DESC; ";
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
                   $"VALUES ('{newcomer.TelNum}', '{ newcomer.Passport?? "NULL" }', '{newcomer.Name}', " +
                           $"'{newcomer.Surname}', '{newcomer.Patronym}', {newcomer.Discount});";
        }

        private static string UpdateClientQuery(ClientDTO toUpdate, string telnum)
        {
            return "UPDATE clients " +
                   $"SET tel_num  = '{toUpdate.TelNum}', " +
                       $"passport = '{toUpdate.Passport}', " +
                       $"cl_name  = '{toUpdate.Name}', " +
                       $"surname  = '{toUpdate.Surname}', " +
                       $"patronym = '{toUpdate.Patronym}', " +
                       $"discount = {toUpdate.Discount} " +
                   $"WHERE tel_num = '{telnum}'; ";
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

        private static string RoomStatisticsQuery(DateTime from, DateTime to)
        {
            var fromstr = from.ToPostgresDateFormat();
            var tostr = to.ToPostgresDateFormat();
            return "SELECT type_name AS type, room_num AS num " +
                    "FROM rooms AS R " +
                    "WHERE(SELECT sum(price_period) " +
                           "FROM bookings " +
                           "WHERE room_num = R.room_num " +
                                   $"AND(start_date BETWEEN {fromstr} AND {tostr} " +
                                        $"OR end_date BETWEEN {fromstr} AND {tostr})) = (SELECT MAX(sum_payed) " +
                                                                                             "FROM(SELECT bookings.room_num, SUM(price_period) as sum_payed " +
                                                                                                    "FROM bookings INNER JOIN rooms ON bookings.room_num = rooms.room_num " +
                                                                                                    "WHERE rooms.type_name = R.type_name " +
                                                                                                        $"AND(start_date BETWEEN {fromstr} AND {tostr} " +
                                                                                                                 $"OR end_date BETWEEN {fromstr} AND {tostr}) " +
                                                                                                    "GROUP BY bookings.room_num) AS X);";
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

        public UserDTO GetUser(string persBookNum)
        {
            var sql = IdentifyQuery(persBookNum);
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

        public void Register(UserCreateDTO user)
        {
            ExecuteInternal(RegisterQuery(user));
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
                BookedRoomNum = qRes.Room_num,
                BookingId = qRes.Book_num,
                Book_state = (BookingStates)qRes.Book_state,
                ClientTel = qRes.Cl_tel_num,
                Complaint = qRes.Complaint,
                EndDate = qRes.End_date,
                EndDateReal = qRes.End_date_real,
                Payed = qRes.Payed,
                PersBooked = GetUser(qRes.Pers_book),
                PersSettled = GetUser(qRes.Pers_settled),
                PricePeriod = qRes.Price_period,
                StartDate = qRes.Start_date,
                StartDateReal = qRes.Start_date_real,
                SumFees = qRes.Sum_fees,
                Payments = GetPayments(id).ToList(),
                ClientDiscount = qRes.Discount
            };
        }

        public void CreateBooking(BookingCreateDTO booking, string persBook)
        {
            ExecuteInternal(CreateBookingQuery(booking, persBook));
        }

        public void CreatePayment(int bookingId, PaymentCreateDTO payment)
        {
            ExecuteInternal(AddPaymentQuery(bookingId, payment.Type, payment.Amount));
        }

        public void UpdateBooking(BookingUpdateDTO booking, int bookId)
        {
            ExecuteInternal(UpdateBookingQuery(booking, bookId));
        }

        public void Settle(int bookId, string persBook)
        {
            ExecuteInternal(SettleQuery(bookId, persBook));
        }

        public void Cancel(int bookId)
        {
            ExecuteInternal(CancelQuery(bookId));
        }

        public void Close(int bookId)
        {
            ExecuteInternal(CloseQuery(bookId));
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

        public IEnumerable<ClientAnalisedDTO> GetClientAnalytics()
        {
            return QueryInternal<ClientAnalyticsQueryRes>(ClientAnalyticsQuery()).Select(c => new ClientAnalisedDTO
            {
                CountBooked = c.Count_booked,
                Discount = c.Discount,
                Name = c.Cl_name,
                SumPayed = c.Sum_payed,
                Surname = c.Surname,
                TelNum = c.Tel_num
            });
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

        public void UpdateClient(ClientDTO toCreate, string telnum)
        {
            ExecuteInternal(UpdateClientQuery(toCreate, telnum));
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

        public IEnumerable<RoomStatisticsDTO> GetRoomStats(DateTime from, DateTime to)
        {
            return QueryInternal<RoomStatisticsDTO>(RoomStatisticsQuery(from, to));
        }

        #region WRAPPERS
        private IEnumerable<T> QueryInternal<T>(string sql)
        {
            IEnumerable<T> result;
            var conn = new NpgsqlConnection(_builder.ToString())
            {
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
            if (cl == null)
                return null;
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
                Type = MapToType(room.Type_name),
                Comforts = (await QueryAsyncInternal<string>(ComfortsQuery(room.Room_num))).ToList()
            };
        }

        private RoomType MapToType(string typeName)
        {
            switch(typeName)
            {
                case "luxe":
                    return RoomType.Luxe;
                case "standard":
                    return RoomType.Standard;
                case "econom":
                    return RoomType.Econom;
                case "studio":
                    return RoomType.Studio;
                case "apartments":
                    return RoomType.Apartments;
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeName));
            }
        }

        private static PaymentDTO MapToPayment(Payment p)
        {
            return new PaymentDTO
            {
                Amount = p.Amount,
                Book_num = p.Book_num,
                Payment_date = p.Payment_date,
                Type = p.Payment_type
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
