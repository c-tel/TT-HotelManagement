using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using TTHotel.API.DBEntities;

namespace TTHotelManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private NpgsqlConnection conn;

        public ValuesController()
        {
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
            conn = new NpgsqlConnection(builder.ToString())
            {
                // dirty hack. must be removed later
                UserCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<MainPivotData>> Get()
        {
            var query = "select rooms.room_num, room_floor, start_date, end_date, book_state, (price_period+service_price+sum_fees-payed) AS debt" +
                " from (select *" +
                " from bookings" +
                " where end_date BETWEEN \'2019-03-10\' AND \'2019-03-15\'" +
                " OR start_date BETWEEN \'2019-03-10\' AND \'2019-03-15\'" +
                " ) AS B RIGHT OUTER JOIN rooms ON B.room_num = rooms.room_num" +
                " order by room_num, room_floor; ";
            IEnumerable<MainPivotData> res;
            using (conn)
            {
                res = conn.Query<MainPivotData>(query);
            }
            return new ActionResult<IEnumerable<MainPivotData>>(res);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
