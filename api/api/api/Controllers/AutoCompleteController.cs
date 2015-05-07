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
    public class AutoCompleteController : ApiController
    {
        // GET: api/AutoComplete
        public IEnumerable<string> Get(string type)
        {
            List<string> items = new List<string>();
            using (var db = new EModelsContext())
            {
                if (type.Equals("descriptions"))
                {
                    var result = db.descriptions.ToList();
                    foreach (var eDescription in result)
                    {
                        items.Add(eDescription.description1);
                    }

                }
                else if (type.Equals("analysiscodes"))
                {
                    //array of strings of all analysis codes in the analysis_codes table
                    var result = db.analysis_codes.ToList();
                    foreach (var eAnalysisCode in result)
                    {
                        items.Add(eAnalysisCode.code);
                    }

                }
                else if (type.Equals("rooms"))
                {
                    //an array of strings of all rooms in the rooms table
                    var result = db.rooms.ToList();
                    foreach (var eRoom in result)
                    {
                        items.Add(eRoom.room1);
                    }


                }
                else if (type.Equals("permissions"))
                {
                    //an array of strings of all rooms in the rooms table
                    var result = db.sharepoint_permissions.ToList();
                    foreach (var ePermission in result)
                    {
                        items.Add(ePermission.type);
                    }


                }
                else if (type.Equals("groups"))
                {
                    //an array of strings of all rooms in the rooms table
                    var result = db.sharepoint_usergroups.ToList();
                    foreach (var eGroup in result)
                    {
                        items.Add(eGroup.name);
                    }


                }
            }
            return items;




        }
    }
}
