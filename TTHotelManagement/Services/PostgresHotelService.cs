using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTHohel.Contracts.Bookings;
using TTHotel.API.DBEntities;

namespace TTHotel.API.Services
{
    public class PostgresHotelService: IHotelService
    {
        private NpgsqlConnection _conn;

        #region QUERIES

        private static string PeriodDataQuery(DateTime from, DateTime to)
        {
            var fromStr = $"'{from.ToString("yyyy-mm-dd")}'";
            var toStr = $"'{from.ToString("yyyy-mm-dd")}'";
            return "SELECT rooms.room_num, room_floor, start_date, end_date, book_state," +
                " (price_period+service_price+sum_fees-payed) AS debt" +
                " FROM (select *" +
                " FROM bookings" +
                " WHERE end_date BETWEEN \'2019-03-10\' AND \'2019-03-15\'" +
                " OR start_date BETWEEN \'2019-03-10\' AND \'2019-03-15\'" +
                " ) AS B RIGHT OUTER JOIN rooms ON B.room_num = rooms.room_num" +
                " ORDER BY room_num, room_floor; ";
        }

        #endregion


        public PostgresHotelService()
        {
            // TODO move to Configuration

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = "ec2-54-243-128-95.compute-1.amazonaws.com",
                Port = 5432,
                Username = "tyoigfycditakg",
                Password = "1bd83bca8ab334c28d2b311b0a73543c66a3288d3327aad1f910dea63fd372cd",
                Database = "dcvtt5pjns4r4v",
                SslMode = SslMode.Require,
                UseSslStream = true,
            };
            _conn = new NpgsqlConnection(builder.ToString())
            {
                // dirty hack. must be removed later
                UserCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
        }

        public List<RoomInfo> GetPeriodInfo(DateTime from, DateTime to)
        {
            var bookingsData = QueryInternal<MainPivotData>("");
            return null;
        }

        private IEnumerable<T> QueryInternal<T>(string sql)
        {
            IEnumerable<T> result;
            using (_conn)
            {
                result = _conn.Query<T>(sql);
            }
            return result;
        }
    }
}
