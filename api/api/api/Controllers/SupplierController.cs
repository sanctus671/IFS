using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api.Models;
using System.Data.SqlClient;
using System.Web.Http.Cors;
using api.EModels;

namespace api.Controllers
{
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
    public class SupplierController : ApiController
    {
        // GET: api/Supplier
        public IEnumerable<Supplier> Get()
        {

            List<Supplier> returnSuppliers = new List<Supplier>();
            using (var db = new EModelsContext())
            {
                var result = db.suppliers.OrderByDescending(req => req.id).ToList();

                foreach (var eSupplier in result)
                {
                    Supplier returnSupplier = new Supplier();
                    returnSupplier.id = eSupplier.id;
                    returnSupplier.name = eSupplier.name;

                    returnSuppliers.Add(returnSupplier);
                }

            }

            return returnSuppliers;

        }

        // GET: api/Supplier/5
        public Supplier Get(int id)
        {
            Supplier returnSupplier = new Supplier();
            using (var db = new EModelsContext())
            {
                var eSupplier = db.suppliers.Where(sup => sup.id.Equals(id)).Single();
                returnSupplier.id = eSupplier.id;
                returnSupplier.name = eSupplier.name;

            }
            return returnSupplier;
        }

        // POST: api/Supplier
        public void Post([FromBody]Supplier data)
        {
            using (var db = new EModelsContext())
            {
                supplier supplier = new supplier{
                    name = data.name
                };
                db.suppliers.Add(supplier);
                db.SaveChanges();
            }

        }

        // PUT: api/Supplier/5
        public void Put(int id, [FromBody]Supplier data)
        {
            using (var db = new EModelsContext())
            {
                var eSupplier = db.suppliers.Where(sup => sup.id.Equals(id)).SingleOrDefault();
                if (eSupplier != null)
                {
                    eSupplier.name = data.name;
                    db.SaveChanges();
                }     
            }
        }

        // DELETE: api/Supplier/5
        public void Delete(int id)
        {
            using (var db = new EModelsContext())
            {
                var eSupplier = db.suppliers.Where(sup => sup.id.Equals(id)).SingleOrDefault();
                if (eSupplier != null)
                {
                    db.suppliers.Remove(eSupplier);
                    db.SaveChanges();
                }
            }
        }
    }
}
