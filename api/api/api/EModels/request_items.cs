namespace api.EModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class request_items
    {
        public int id { get; set; }

        public int requestid { get; set; }

        public int? descriptionid { get; set; }

        [StringLength(255)]
        public string cas { get; set; }

        public int? quantity { get; set; }

        [StringLength(1)]
        public string quality { get; set; }

        [StringLength(150)]
        public string size { get; set; }

        public int? vertere { get; set; }

        public virtual description description { get; set; }

        public virtual request request { get; set; }
    }
}
