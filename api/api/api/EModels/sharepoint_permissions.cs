namespace api.EModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sharepoint_permissions
    {
        public sharepoint_permissions()
        {
            sharepoint_users = new HashSet<sharepoint_users>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string type { get; set; }

        public virtual ICollection<sharepoint_users> sharepoint_users { get; set; }
    }
}
