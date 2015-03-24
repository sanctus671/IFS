using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models
{
    public class RequestPaginate
    {
        public int count { get; set; }
        public List<Request> items { get; set; }

    }
}