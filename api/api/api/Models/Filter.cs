using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models
{
    public class Filter
    {
        public string field { get; set; }
        public string option { get; set; }
        public string value { get; set; }
        public DateTime date1 { get; set; }
        public DateTime date2 { get; set; }
    }
}