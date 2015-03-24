namespace api.EModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class request_payments
    {
        public int id { get; set; }

        public int requestid { get; set; }

        [Required]
        [StringLength(255)]
        public string type { get; set; }

        public decimal? cost { get; set; }

        [StringLength(255)]
        public string pnnumber { get; set; }

        [StringLength(255)]
        public string invoice { get; set; }

        public virtual request request { get; set; }
    }
}
