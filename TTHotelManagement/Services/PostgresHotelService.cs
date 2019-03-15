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
using TTHotel.Contracts.Auth;
using TTHotel.Contracts.Bookings;

namespace TTHotel.API.Services
{
    public class PostgresHotelService: IHotelService
    {
        private NpgsqlConnection _conn;
        private NpgsqlConnectionStringBuilder _builder;

        #region QUERIES

        private static string PeriodDataQuery(DateTime from, DateTime to)
        {
            var fromStr = $"'{from.ToString("yyyy-MM-dd")}'";
            var toStr = $"'{to.ToString("yyyy-MM-dd")}'";
            var res =   "SELECT rooms.room_num, room_floor, start_date, end_date, book_state, book_num, " +
                        "(price_period+service_price+sum_fees-payed) AS debt " +
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

        #region WRAPPERS
        private IEnumerable<T> QueryInternal<T>(string sql)
        {
            IEnumerable<T> result;
            _conn = new NpgsqlConnection(_builder.ToString())
            {
                // dirty hack. must be removed later
                UserCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            using (_conn)
            {
                result = _conn.Query<T>(sql);
            }
            return result;
        }

        private T QuerySingleOrDefaultInternal<T>(string sql)
        {
            T result;
            _conn = new NpgsqlConnection(_builder.ToString())
            {
                // dirty hack. must be removed later
                UserCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            using (_conn)
            {
                result = _conn.QuerySingleOrDefault<T>(sql);
            }
            return result;
        }

        #endregion

        private RoomDailyStatus MapToStatus(BookStates? state)
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
