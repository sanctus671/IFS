namespace api.EModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class request_suppliers
    {
        public int id { get; set; }

        public int requestid { get; set; }

        public int supplierid { get; set; }

        public virtual request request { get; set; }

        public virtual supplier supplier { get; set; }
    }
}
