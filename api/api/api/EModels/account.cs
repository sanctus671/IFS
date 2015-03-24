namespace api.EModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class account
    {
        public account()
        {
            requests = new HashSet<request>();
        }

        public int id { get; set; }

        [StringLength(255)]
        public string number { get; set; }

        public virtual ICollection<request> requests { get; set; }
    }
}
