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
    public class UserController : ApiController
    {
        // GET: api/User
        public IEnumerable<User> Get()
        {
            List<User> returnUsers = new List<User>();
            using (var db = new EModelsContext())
            {
                var result = db.sharepoint_users.OrderByDescending(user => user.id).ToList();

                foreach (var eUser in result)
                {

                    User returnUser = new User();
                    returnUser.id = eUser.id;
                    returnUser.name = eUser.name;
                    returnUser.email = eUser.email;
                    returnUser.phone = eUser.phone;
                    returnUser.userCode = eUser.user_code;
                    returnUser.permissionid = eUser.permissionid;
                    returnUser.permission = eUser.sharepoint_permissions == null ? "None" : eUser.sharepoint_permissions.type;
                    returnUser.groupid = eUser.groupid;
                    returnUser.group = eUser.sharepoint_usergroups == null ? "None" : eUser.sharepoint_usergroups.name;

                    returnUsers.Add(returnUser);
                }

            }

            return returnUsers;


        }

        // GET: api/User/5
        public User Get(int id)
        {
            User returnUser = new User();
            using (var db = new EModelsContext())
            {
                var eUser = db.sharepoint_users.Where(user => user.id.Equals(id)).Single();
                returnUser.id = eUser.id;
                returnUser.name = eUser.name;
                returnUser.email = eUser.email;
                returnUser.phone = eUser.phone;
                returnUser.userCode = eUser.user_code;
                returnUser.permissionid = eUser.permissionid;
                returnUser.permission = eUser.sharepoint_permissions == null ? "None" : eUser.sharepoint_permissions.type;
                returnUser.groupid = eUser.groupid;
                returnUser.group = eUser.sharepoint_usergroups == null ? "None" : eUser.sharepoint_usergroups.name;

            }
            return returnUser;

        }

        // POST: api/User
        public void Post([FromBody]User data)
        {
            using (var db = new EModelsContext())
            {
                int permissionid = db.sharepoint_permissions.Where(perm => perm.type.Equals(data.permission)).SingleOrDefault().id;
                int groupid = db.sharepoint_usergroups.Where(ug => ug.name.Equals(data.group)).SingleOrDefault().id;

                sharepoint_users user = new sharepoint_users
                {
                    name = data.name,
                    email = data.email,
                    phone = data.phone,
                    user_code = data.userCode,
                    permissionid = permissionid,
                    groupid = groupid

                };
                db.sharepoint_users.Add(user);
                db.SaveChanges();
            }
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]User data)
        {
            using (var db = new EModelsContext())
            {


                var eUser = db.sharepoint_users.Where(user => user.id.Equals(id)).SingleOrDefault();

                int permissionid = db.sharepoint_permissions.Where(perm => perm.type.Equals(data.permission)).SingleOrDefault().id;
                int groupid = db.sharepoint_usergroups.Where(ug => ug.name.Equals(data.group)).SingleOrDefault().id;


                if (eUser != null)
                {
                    eUser.name = data.name;
                    eUser.email = data.email;
                    eUser.phone = data.phone;
                    eUser.user_code = data.userCode;
                    eUser.permissionid = permissionid;
                    eUser.groupid = data.groupid;
                    db.SaveChanges();
                }
            }
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
            using (var db = new EModelsContext())
            {
                var eUser = db.sharepoint_users.Where(user => user.id.Equals(id)).SingleOrDefault();
                if (eUser != null)
                {
                    db.sharepoint_users.Remove(eUser);
                    db.SaveChanges();
                }
            }
        }
    }
}
