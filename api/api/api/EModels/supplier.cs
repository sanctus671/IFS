namespace api.EModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class supplier
    {
        public supplier()
        {
            request_suppliers = new HashSet<request_suppliers>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public virtual ICollection<request_suppliers> request_suppliers { get; set; }
    }
}
