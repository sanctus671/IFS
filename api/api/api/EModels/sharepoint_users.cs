namespace api.EModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sharepoint_users
    {
        public sharepoint_users()
        {
            request_status = new HashSet<request_status>();
            requests = new HashSet<request>();
        }

        public int id { get; set; }

        public int? sharepointid { get; set; }

        public int? groupid { get; set; }

        public int? permissionid { get; set; }

        [StringLength(255)]
        public string user_code { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        [StringLength(255)]
        public string phone { get; set; }

        [StringLength(255)]
        public string email { get; set; }

        public virtual ICollection<request_status> request_status { get; set; }

        public virtual ICollection<request> requests { get; set; }

        public virtual sharepoint_permissions sharepoint_permissions { get; set; }

        public virtual sharepoint_usergroups sharepoint_usergroups { get; set; }
    }
}
