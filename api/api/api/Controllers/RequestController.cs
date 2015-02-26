using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api.Models;
using System.Data.SqlClient;

namespace api.Controllers
{
    public class RequestController : ApiController
    {
        // GET: api/Request
        public IEnumerable<Request> Get()
        {

            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs;Integrated Security=True";
            SqlConnection con = new SqlConnection(source);
            con.Open();

            SqlCommand command = new SqlCommand("SELECT * FROM requisition", con);

            SqlDataReader reader = command.ExecuteReader();

            List<Request> items = new List<Request>();

            while (reader.Read())
            {
                Request data = new Request();

                data.id = (int) reader["reqID"];
                data.itemDescription = (string)reader["description"]; 
                items.Add(data);
            }

            //your code here;
            con.Close();
            return items;
        }

        // GET: api/Request/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Request
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Request/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Request/5
        public void Delete(int id)
        {
        }
    }
}
