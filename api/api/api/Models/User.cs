using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models
{
    public class User
    {
        public int id { get; set; }
        public string userCode { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int? permissionid { get; set; }
        public string permission { get; set; }
        public int? groupid { get; set; }
        public string group { get; set; }
        public List<string> groupUsers { get; set; }

    }
}