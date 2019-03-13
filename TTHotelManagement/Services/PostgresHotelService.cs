using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTHohel.Contracts.Bookings;
using TTHotel.API.DBEntities;
using TTHotel.Contracts.Auth;

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
            var res = "SELECT rooms.room_num, room_floor, start_date, end_date, book_state, book_num, " +
                " (price_period+service_price+sum_fees-payed) AS debt" +
                " FROM (select *" +
                " FROM bookings" +
                $" WHERE (end_date BETWEEN {fromStr} AND {toStr}" +
                $" OR start_date BETWEEN {fromStr} AND {toStr})" +
                $" AND book_state <> 'canceled'" +
                " ) AS B RIGHT OUTER JOIN rooms ON B.room_num = rooms.room_num" +
                " ORDER BY room_num, start_date; ";
            return res;
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

        public UserDTO GetUser(string login, string pwd)
        {
            throw new NotImplementedException();
        }

        List<RoomInfo> IHotelService.GetPeriodInfo(DateTime from, DateTime to)
        {
            throw new NotImplementedException();
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
