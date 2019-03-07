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
        public ActionResult<IEnumerable<Personnel>> Get()
        {
            var query = "SELECT * FROM personnel";
            IEnumerable<Personnel> res;
            using (conn)
            {
                res = conn.Query<Personnel>(query);
            }
            return new ActionResult<IEnumerable<Personnel>>(res);
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
