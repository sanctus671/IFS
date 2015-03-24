namespace api.EModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class request
    {
        public request()
        {
            request_admin_notes = new HashSet<request_admin_notes>();
            request_items = new HashSet<request_items>();
            request_payments = new HashSet<request_payments>();
            request_permits = new HashSet<request_permits>();
            request_status = new HashSet<request_status>();
            request_suppliers = new HashSet<request_suppliers>();
        }

        public int id { get; set; }

        public int userid { get; set; }

        public int? roomid { get; set; }

        public int? accountid { get; set; }

        public int? codeid { get; set; }

        public string notes { get; set; }

        public virtual account account { get; set; }

        public virtual analysis_codes analysis_codes { get; set; }

        public virtual ICollection<request_admin_notes> request_admin_notes { get; set; }

        public virtual ICollection<request_items> request_items { get; set; }

        public virtual ICollection<request_payments> request_payments { get; set; }

        public virtual ICollection<request_permits> request_permits { get; set; }

        public virtual ICollection<request_status> request_status { get; set; }

        public virtual ICollection<request_suppliers> request_suppliers { get; set; }

        public virtual sharepoint_users sharepoint_users { get; set; }

        public virtual room room { get; set; }
    }
}
