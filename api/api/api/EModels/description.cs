namespace api.EModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class description
    {
        public description()
        {
            request_items = new HashSet<request_items>();
        }

        public int id { get; set; }

        [Column("description")]
        [StringLength(255)]
        public string description1 { get; set; }

        [Required]
        [StringLength(255)]
        public string type { get; set; }

        public virtual ICollection<request_items> request_items { get; set; }
    }
}
