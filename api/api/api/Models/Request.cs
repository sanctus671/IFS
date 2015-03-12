using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.Models
{
    public class Request
    {
        public int id { get; set; }
        public string status { get; set; }
        public List<Status> statusArray { get; set; }
        public DateTime date { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string type { get; set; }
        public string destinationRoom { get; set; }
        public string itemDescription { get; set; }
        public string quality { get; set; }
        public string size { get; set; }
        public int quantity { get; set; }
        public int vertere { get; set; }
        public string notes { get; set; }
        public string cas { get; set; }
        public string accountNumber { get; set; }
        public string supplier { get; set; }
        public DateTime dateSupplied { get; set; }
        public string adminName { get; set; }
        public decimal cost { get; set; }
        public string adminNotes { get; set; }
        public string analysisCode { get; set; }
        public bool permit { get; set; }
        public string permitNumber { get; set; }
        public string paymentType { get; set; }
        public string pnNumber { get; set; }
        public string invoiceDetails { get; set; }
    }
}