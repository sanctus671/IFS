using api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using api.EModels;


namespace api.Controllers
{
    [EnableCors(origins: "http://localhost:8080", headers: "*", methods: "*")]
    public class MiscellaneousController : ApiController
    {
        // GET: api/Miscellaneous
        public IEnumerable<KeyValuePair<string, string>> Get()
        {

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            using (var db = new EModelsContext())
            {
                var result = db.tooltips.ToList();

                foreach (var eTooltip in result)
                {
                    string field = eTooltip.field;
                    string data = eTooltip.tooltip1;
                    dictionary.Add(field, data);

                }
            }

            return dictionary;

        }



        // GET: api/Miscellaneous/GL11223213
        public bool Get(string account)
        {

            using (var db = new EModelsContext())
            {
                var count = db.accounts.Where(acc => acc.number.Equals(account)).Count();
                if (count > 0)
                {
                    return true;
                }

            }

            return false;
        }
    }
}
