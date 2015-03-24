namespace api.EModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tooltip
    {
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string field { get; set; }

        [Column("tooltip")]
        [Required]
        public string tooltip1 { get; set; }
    }
}
