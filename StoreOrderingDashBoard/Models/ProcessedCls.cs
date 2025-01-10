using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreOrderingDashBoard.Models
{
    public class ProcessedCls
    {
        public long Id { get; set; }
        public string FinilizedOrderno { get; set; }
        public DateTime? OrderDate { get; set; }
        public string StoreName { get; set; }
        public string StoreCode { get; set; }
        public string WareHouseDC { get; set; }
        public int? DCID { get; set; }
        public bool IsThreshold { get; set; }
        public string LocationName { get; set; }
        public string DcName { get; set; }
        public int Quantity { get; set; }
    }
}