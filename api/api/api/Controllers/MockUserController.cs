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
    public class MockUserController : ApiController
    {
        // GET: api/MockUser
        public User Get(int userCode)
        {

            User user = new User()
            {
                id = 1,
                userCode = "1",
                permission = "admin"
            };

            return user;

        }

    }
}
