using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PODListData
    {
        public int Id { get; set; }
        public string Invoice_Number { get; set; }
        public Int64? OrderHeaderId { get; set; }
        public int custid { get; set; }

    }

    public class storeBO
    {
        public String Invoice_Number { get; set; }
        public String StoreCode { get; set; }
        public String StoreName { get; set; }
        public Int32? DeliveryStatus { get; set; }
        public Int64? PODHeaderId { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public Int32? Reason { get; set; }
        public int Delivery_Re_Attempt { get; set; }
    }
}
