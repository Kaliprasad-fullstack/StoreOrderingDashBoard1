using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreOrderingDashBoard.Models
{
    public class PODResponse
    {
        public string Item { get; set; }
        public int ItemId { get; set; }
        public decimal InvoiceQty { get; set; }
        public decimal DeliveredQty { get; set; }
        public int Reason { get; set; }
        public decimal DispatchQty { get; set; }
        public string ItemCode { get; set; }
    }
}