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
    public class AutoCompleteController : ApiController
    {
        // GET: api/AutoComplete
        public IEnumerable<string> Get(string type)
        {
            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs_new;Integrated Security=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(source);
            con.Open();
            //descriptions
            //analysiscodes
            //rooms
            string query = "SELECT room FROM rooms";

            if (type.Equals("descriptions", StringComparison.OrdinalIgnoreCase))
            {
                //array of string with description from descriptions table which are linked to a reqeust
                //query = "SELECT DISTINCT accounts.number FROM requests LEFT JOIN accounts ON requests.accountid = accounts.id";
                query = "SELECT description FROM descriptions";
            }
            else if (type.Equals("analysiscodes", StringComparison.OrdinalIgnoreCase))
            {
                //array of strings of all analysis codes in the analysis_codes table
                query = "SELECT code FROM analysis_codes";

            }
            else if (type.Equals("rooms", StringComparison.OrdinalIgnoreCase)){
                //an array of strings of all rooms in the rooms table
                query = "SELECT room FROM rooms";

            }


            SqlCommand queryCommand = new SqlCommand(query, con);

            SqlDataReader queryCommandReader = queryCommand.ExecuteReader();

            List<string> items = new List<string>();
            while (queryCommandReader.Read())
            {
                if (type.Equals("descriptions", StringComparison.OrdinalIgnoreCase))
                {
                    items.Add((string)queryCommandReader["description"]);
                }
                else if (type.Equals("analysiscodes", StringComparison.OrdinalIgnoreCase))
                {
                    items.Add((string)queryCommandReader["code"]);

                }
                else if (type.Equals("rooms", StringComparison.OrdinalIgnoreCase))
                {
                    items.Add((string)queryCommandReader["room"]);

                }

            }

            return items;

        }

    }
}
