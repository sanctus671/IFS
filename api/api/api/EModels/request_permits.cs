namespace api.EModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class request_permits
    {
        public int id { get; set; }

        public int requestid { get; set; }

        [Required]
        [StringLength(255)]
        public string number { get; set; }

        public virtual request request { get; set; }
    }
}
