using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models
{
    public class RequestPaginate
    {
        public RequestPaginate()
        {
            this.items = new List<Request>();
        }
        public int count { get; set; }
        public User user { get; set; }
        public List<Request> items { get; set; }

    }
}