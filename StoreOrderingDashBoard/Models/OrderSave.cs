using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreOrderingDashBoard.Models
{
    public class OrderSave
    {
        public int ProductId { get; set; }
        public string Qty { get; set; }
        public int storeId { get; set; }
        public long OrderheaderId { get; set; }
        public int DCID { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public string PONumber { get; set; }
    }
}