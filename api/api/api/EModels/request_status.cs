namespace api.EModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class request_status
    {
        public int id { get; set; }

        public int requestid { get; set; }

        public int userid { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime date { get; set; }

        [Required]
        [StringLength(255)]
        public string status { get; set; }

        public virtual request request { get; set; }

        public virtual sharepoint_users sharepoint_users { get; set; }
    }
}
