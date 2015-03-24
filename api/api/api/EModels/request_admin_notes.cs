namespace api.EModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class request_admin_notes
    {
        public int id { get; set; }

        public int requestid { get; set; }

        [Required]
        public string note { get; set; }

        public virtual request request { get; set; }
    }
}
