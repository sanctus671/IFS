using api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace api.Controllers
{
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
    public class MiscellaneousController : ApiController
    {
        // GET: api/Miscellaneous
        public IEnumerable<KeyValuePair<string, string>> Get()
        {

            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs_new;Integrated Security=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(source);
            con.Open();

            SqlCommand tooltips = new SqlCommand("SELECT * FROM tooltips", con);
            tooltips.CommandTimeout = 0;
            SqlDataReader tooltipsReader = tooltips.ExecuteReader();

            Dictionary<string,string> dictionary = new Dictionary<string,string>();

            while (tooltipsReader.Read())
            {
                string field = (string)tooltipsReader["field"];
                string data = (string)tooltipsReader["tooltip"];
                dictionary.Add(field, data);


            }

            return dictionary;
        }

        // GET: api/Miscellaneous/GL11223213
        public bool Get(string account)
        {
            //checks account number is valid
            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs_new;Integrated Security=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(source);
            con.Open();

            SqlCommand check = new SqlCommand("SELECT count(1) FROM accounts WHERE number = '" + account + "'", con);

            return (bool)check.ExecuteScalar();

        }


    }
}
