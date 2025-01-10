using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DateParameter
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class OrderInbound
    {
        public DateTime? OrderDate{ get; set; }
        public string ClientPONo { get; set; }
        public string StoreCode { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double OrderQty { get; set; }
    }
    public class OrderInboundResponse
    {
        public DateTime? Date { get; set; }
        public string ClientPONo { get; set; }
        public string Status { get; set; }
    }
}
