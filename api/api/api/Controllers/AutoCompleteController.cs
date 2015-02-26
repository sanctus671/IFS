using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.Controllers
{
    public class AutoCompleteController : ApiController
    {
        // GET: api/AutoComplete
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/AutoComplete/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/AutoComplete
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/AutoComplete/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/AutoComplete/5
        public void Delete(int id)
        {
        }
    }
}
