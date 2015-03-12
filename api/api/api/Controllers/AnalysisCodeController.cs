using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api.Models;
using System.Data.SqlClient;
using System.Web.Http.Cors;

namespace api.Controllers
{
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
    public class AnalysisCodeController : ApiController
    {
        // GET: api/AnalysisCode
        public IEnumerable<AnalysisCode> Get()
        {
            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs_new;Integrated Security=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(source);
            con.Open();

            SqlCommand codes = new SqlCommand("SELECT * FROM analysis_codes", con);
            codes.CommandTimeout = 0;
            SqlDataReader codesReader = codes.ExecuteReader();

            List<AnalysisCode> items = new List<AnalysisCode>();
            while (codesReader.Read())
            {
                AnalysisCode data = new AnalysisCode();


                //fill in what we have from initial SQL call
                data.id = (int)codesReader["id"];
                data.code = (string)codesReader["code"];

                items.Add(data);
            }

            return items;


        }

        // GET: api/AnalysisCode/5
        public AnalysisCode Get(int id)
        {
            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs_new;Integrated Security=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(source);
            con.Open();

            SqlCommand codes = new SqlCommand("SELECT * FROM analysis_codes WHERE id = " + id, con);
            codes.CommandTimeout = 0;
            SqlDataReader codesReader = codes.ExecuteReader();

            AnalysisCode item = new AnalysisCode();
            while (codesReader.Read())
            {

                //fill in what we have from initial SQL call
                item.id = (int)codesReader["id"];
                item.code = (string)codesReader["code"];
            }

            return item;
        }

        // POST: api/AnalysisCode
        public void Post([FromBody]AnalysisCode data)
        {
            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs_new;Integrated Security=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(source);
            con.Open();

            SqlCommand newCode = new SqlCommand(@"INSERT INTO analysis_codes (code) VALUES (@code)", con);
            newCode.Parameters.AddWithValue("@code", data.code);

            newCode.ExecuteScalar();
        }

        // PUT: api/AnalysisCode/5
        public void Put(int id, [FromBody]AnalysisCode data)
        {
            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs_new;Integrated Security=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(source);
            con.Open();

            SqlCommand updateCode = new SqlCommand(@"UPDATE analysis_codes SET code = @code WHERE id = @id", con);
            updateCode.Parameters.AddWithValue("@code", data.code);
            updateCode.Parameters.AddWithValue("@id", data.id);

            updateCode.ExecuteScalar();
        }

        // DELETE: api/AnalysisCode/5
        public void Delete(int id)
        {
            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs_new;Integrated Security=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(source);
            con.Open();

            SqlCommand deleteCode = new SqlCommand(@"DELETE FROM analysis_codes WHERE id = @id", con);
            deleteCode.Parameters.AddWithValue("@id", id);

            deleteCode.ExecuteScalar();
        }
    }
}
