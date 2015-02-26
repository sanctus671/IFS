using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using api.Models;

namespace api.Controllers
{
    public class AnalysisCodeController : ApiController
    {
        // GET: api/AnalysisCode
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/AnalysisCode/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/AnalysisCode
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/AnalysisCode/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/AnalysisCode/5
        public void Delete(int id)
        {
        }
    }
}
