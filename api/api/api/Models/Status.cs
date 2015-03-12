using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models
{
    public class Status
    {
        public int requestid { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
        public string status { get; set; }

        public Status Clone()
        {
            Status statusClone = new Status();
            statusClone.requestid = this.requestid;
            statusClone.name = this.name;
            statusClone.date = this.date;
            statusClone.status = this.status;

            return statusClone;
        }

    }


}