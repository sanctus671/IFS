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
    public class SupplierController : ApiController
    {
        // GET: api/Supplier
        public IEnumerable<Supplier> Get()
        {

            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs_new;Integrated Security=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(source);
            con.Open();

            SqlCommand suppliers = new SqlCommand("SELECT * FROM suppliers ORDER BY id", con);
            suppliers.CommandTimeout = 0;
            SqlDataReader suppliersReader = suppliers.ExecuteReader();

            List<Supplier> items = new List<Supplier>();
            while (suppliersReader.Read())
            {
                Supplier data = new Supplier();


                //fill in what we have from initial SQL call
                data.id = (int)suppliersReader["id"];
                data.name = (string)suppliersReader["name"];

                items.Add(data);
            }

            return items;
        }

        // GET: api/Supplier/5
        public Supplier Get(int id)
        {
            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs_new;Integrated Security=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(source);
            con.Open();

            SqlCommand suppliers = new SqlCommand("SELECT * FROM suppliers WHERE id = " + id, con);
            suppliers.CommandTimeout = 0;
            SqlDataReader suppliersReader = suppliers.ExecuteReader();

            Supplier item = new Supplier();
            while (suppliersReader.Read())
            {

                //fill in what we have from initial SQL call
                item.id = (int)suppliersReader["id"];
                item.name = (string)suppliersReader["name"];
            }

            return item;
        }

        // POST: api/Supplier
        public void Post([FromBody]Supplier data)
        {
            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs_new;Integrated Security=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(source);
            con.Open();

            SqlCommand newSupplier = new SqlCommand(@"INSERT INTO suppliers (name) VALUES (@name)", con);
            newSupplier.Parameters.AddWithValue("@name", data.name);

            newSupplier.ExecuteScalar();
        }

        // PUT: api/Supplier/5
        public void Put(int id, [FromBody]Supplier data)
        {
            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs_new;Integrated Security=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(source);
            con.Open();

            SqlCommand updateSupplier = new SqlCommand(@"UPDATE suppliers SET name = @name WHERE id = @id", con);
            updateSupplier.Parameters.AddWithValue("@name", data.name);
            updateSupplier.Parameters.AddWithValue("@id", data.id);

            updateSupplier.ExecuteScalar();
        }

        // DELETE: api/Supplier/5
        public void Delete(int id)
        {
            string source = @"Data Source=TAYLOR-HP;Initial Catalog=ifs_new;Integrated Security=True;MultipleActiveResultSets=true";
            SqlConnection con = new SqlConnection(source);
            con.Open();

            SqlCommand deleteSupplier = new SqlCommand(@"DELETE FROM suppliers WHERE id = @id", con);
            deleteSupplier.Parameters.AddWithValue("@id", id);

            deleteSupplier.ExecuteScalar();
        }
    }
}
