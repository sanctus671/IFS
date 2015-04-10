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
    public class AnalysisCodeController : ApiController
    {
        // GET: api/AnalysisCode
        public IEnumerable<AnalysisCode> Get()
        {
            List<AnalysisCode> returnAnalysisCodes = new List<AnalysisCode>();
            using (var db = new EModelsContext())
            {
                var result = db.analysis_codes.OrderByDescending(ac => ac.id).ToList();

                foreach (var eAnalysisCode in result)
                {

                    AnalysisCode returnAnalysisCode = new AnalysisCode();
                    returnAnalysisCode.id = eAnalysisCode.id;
                    returnAnalysisCode.code = eAnalysisCode.code;

                    returnAnalysisCodes.Add(returnAnalysisCode);
                }

            }

            return returnAnalysisCodes;


        }

        // GET: api/AnalysisCode/5
        public AnalysisCode Get(int id)
        {
            AnalysisCode returnAnalysisCode = new AnalysisCode();
            using (var db = new EModelsContext())
            {
                var eAnalysisCode = db.analysis_codes.Where(ac => ac.id.Equals(id)).Single();

                returnAnalysisCode.id = eAnalysisCode.id;
                returnAnalysisCode.code = eAnalysisCode.code;

            }
            return returnAnalysisCode;

        }

        // POST: api/AnalysisCode
        public void Post([FromBody]AnalysisCode data)
        {
            using (var db = new EModelsContext())
            {
                analysis_codes analysisCode = new analysis_codes
                {
                    code = data.code
                };
                db.analysis_codes.Add(analysisCode);
                db.SaveChanges();
            }
        }

        // PUT: api/AnalysisCode/5
        public void Put(int id, [FromBody]AnalysisCode data)
        {
            using (var db = new EModelsContext())
            {
                var eAnalysisCode = db.analysis_codes.Where(ac => ac.id.Equals(id)).SingleOrDefault();
                if (eAnalysisCode != null)
                {
                    eAnalysisCode.code = data.code;
                    db.SaveChanges();
                }
            }
        }

        // DELETE: api/AnalysisCode/5
        public void Delete(int id)
        {
            using (var db = new EModelsContext())
            {
                var eAnalysisCode = db.analysis_codes.Where(ac => ac.id.Equals(id)).SingleOrDefault();
                if (eAnalysisCode != null)
                {
                    db.analysis_codes.Remove(eAnalysisCode);
                    db.SaveChanges();
                }
            }
        }
    }
}
